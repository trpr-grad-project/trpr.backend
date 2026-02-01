using Modules.Users.Application.Abstractions.Identity;
using Microsoft.Extensions.Logging;
using Modules.Users.Application.Exceptions;
using Modules.Users.Domain.Entities;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Application.Dtos.Requests;
using Modules.Users.Application.Abstractions;
using Modules.Users.Application.Repositories;
using Modules.Users.Application.Interfaces;
using Modules.Users.Application.Factories;
using Modules.Users.Domain.ValueObjects;
using System.Net.Mail;

namespace Modules.Users.Application.Services;

public class UserService(
IUserRepository userRepository,
IGenericRepository<User, Guid> UserRepo,
IGenericRepository<Token, Guid> TokenRepo,
TokenFactory tokenFactory,
IIdentityProviderService identityProviderService,
OtpHandlerFactory otpHandlerFactory,
ILogger<UserService> logger,
IUnitOfWork unitOfWork)
{
    public async Task<OtpResponseDto> CreateUserAsync(CreateUserRequestDto createUserRequestDto, CancellationToken cancellationToken = default)
    {
        User? user = await userRepository.GetByIdentifier(createUserRequestDto.Identifier);
        if (user != null && user.IsVerified)
        {
            logger.LogWarning("User creation failed. Identifier already exists: {Identifier}", user.Id);
            throw new ConflictException("User.Conflict", user.Id);
        }
        await RemoveUnVerifiedUserAsync(user, cancellationToken);
        var emailAdress = MailAddress.TryCreate(createUserRequestDto.Identifier, out var _) ? createUserRequestDto.Identifier : string.Format("{0}@trpr.com", createUserRequestDto.Identifier);
        string userIdentitfier = await identityProviderService.RegisterUserAsync(
            new UserModel(
                createUserRequestDto.Identifier,
                emailAdress,
                createUserRequestDto.Password,
                createUserRequestDto.FirstName,
                createUserRequestDto.LastName),
            cancellationToken);
        user = User.Create(
                createUserRequestDto.Identifier,
                createUserRequestDto.FirstName,
                createUserRequestDto.LastName,
                userIdentitfier);
        UserRepo.Add(user);
        var token = tokenFactory.CreateToken(TokenType.Otp, user);
        TokenRepo.Add(token);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return new OtpResponseDto { OtpId = token.Id };
    }
    public async Task<OtpResponseDto> ForgetPasswordAsync(ForgetPasswordRequestIdentifierDto forgetPasswordRequestIdentifierDto, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdentifier(forgetPasswordRequestIdentifierDto.Identifier) ?? throw new NotFoundException("User.NotFound", forgetPasswordRequestIdentifierDto.Identifier);
        var token = tokenFactory.CreateToken(TokenType.ForgetPasswordOtp, user);
        TokenRepo.Add(token);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return new OtpResponseDto { OtpId = token.Id };
    }

    public async Task<LoginUserResponseDto> LoginUserAsync(LoginUserRequestDto loginUserRequestDto, CancellationToken cancellationToken = default)
    {

        var user = await userRepository.GetByIdentifier(loginUserRequestDto.Identifier) ?? throw new NotFoundException("User.NotFound", loginUserRequestDto.Identifier);
        var loginResponse = await identityProviderService.LoginUserAsync(
            loginUserRequestDto.Identifier,
            loginUserRequestDto.Password,
            cancellationToken);
        loginResponse.ProfileSetupCompleted = user.Profile != null;
        return loginResponse;
    }

    public async Task<LoginUserResponseDto> RefreshUserAsnc(RefreshTokenRequestDto refreshTokenRequestDto, CancellationToken cancellationToken = default)
    {
        return await identityProviderService.RefreshUserAsync(
            refreshTokenRequestDto.Token,
            cancellationToken);
    }

    public async Task<LoginUserResponseDto> VerifyOtpAsync(VerifyOtpRequestDto verifyOtpRequestDto, CancellationToken cancellationToken = default)
    {
        try
        {
            await unitOfWork.BeginTransactionAsync(cancellationToken);
            var otpToken = await TokenRepo.GetByIdForUpdate(verifyOtpRequestDto.Identifier);
            if (
                otpToken == null ||
                otpToken.Expiration < DateTime.UtcNow ||
                otpToken.IsRevoked)
            {
                logger.LogWarning(
                    "OTP verification failed. Invalid or expired OTP ID: {OtpId}",
                    verifyOtpRequestDto.Identifier);
                throw new BadRequestException("Otp.Invalid", verifyOtpRequestDto.Identifier);
            }
            User user =
                await UserRepo.GetById(otpToken.UserId) ??
                throw new NotFoundException("User.NotFound", otpToken.UserId);
            await unitOfWork
                .SaveChangesAsync(cancellationToken);
            var otpHandler = otpHandlerFactory
                .CreateOtpHandler(otpToken.Type);
            var loginResponse = await otpHandler
                .VerifyOtpAsync(otpToken, user, verifyOtpRequestDto.Value, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return loginResponse;
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    private async Task RemoveUnVerifiedUserAsync(User? user, CancellationToken cancellationToken)
    {
        if (user == null || user.IsVerified) return;
        logger.LogInformation("User with identifier {Identifier} exists but is not verified. Proceeding to re-register.", user.Id);
        UserRepo.Remove(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        await identityProviderService.RemoveUserAsync(user.Id, cancellationToken);
    }

}
