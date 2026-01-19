using Modules.Users.Application.Abstractions.Identity;
using Microsoft.Extensions.Logging;
using Modules.Users.Application.Exceptions;
using Modules.Users.Domain.Entities;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Application.Dtos.Requests;
using Modules.Users.Application.Abstractions;
using Modules.Users.Application.Repositories;
using Modules.Users.Application.Interfaces;

namespace Modules.Users.Application.Services;

public class UserService(
IUserRepository userRepository,
IGenericRepository<User, Guid> UserRepo,
IGenericRepository<Profile, Guid> ProfileRepo,
IIdentityProviderService identityProviderService,
ILogger<UserService> logger,
IUnitOfWork unitOfWork) : IUserService
{
    public async Task<Guid> CreateUserAsync(CreateUserRequestDto createUserRequestDto, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByEmail(createUserRequestDto.Email);
        if (user != null)
        {
            logger.LogWarning("User creation failed. Email already exists: {Email}", createUserRequestDto.Email);
            throw new ConflictException("User.Conflict.Email", createUserRequestDto.Email);
        }
        string userIdentitfier = await identityProviderService.RegisterUserAsync(new UserModel(createUserRequestDto.Email, createUserRequestDto.Password, createUserRequestDto.FirstName, createUserRequestDto.LastName), cancellationToken);
        user = User.Create(createUserRequestDto.Email, createUserRequestDto.FirstName, createUserRequestDto.LastName, userIdentitfier);
        UserRepo.Add(user);
        var Profile = new Profile { Id = user.Id };
        ProfileRepo.Add(Profile);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return user.Id;
    }

    public async Task<LoginUserResponse> LoginUserAsync(LoginUserRequestDto loginUserRequestDto, CancellationToken cancellationToken = default)
    {
        return await identityProviderService.LoginUserAsync(loginUserRequestDto.Email, loginUserRequestDto.Password, cancellationToken);
    }

    public async Task<LoginUserResponse> RefreshUserAsnc(RefreshTokenRequestDto refreshTokenRequestDto, CancellationToken cancellationToken = default)
    {
        return await identityProviderService.RefreshUserAsync(refreshTokenRequestDto.Token, cancellationToken);
    }

}
