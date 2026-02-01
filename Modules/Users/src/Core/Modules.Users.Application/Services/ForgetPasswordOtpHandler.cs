using Modules.Users.Application.Abstractions;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Application.Exceptions;
using Modules.Users.Application.Interfaces;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Application.Services;

public class ForgetPasswordOtpHandler(IGenericRepository<User, Guid> UserRepo, IGenericRepository<Token, Guid> TokenRepo, IUnitOfWork unitOfWork, IIdentityProviderService identityProviderService) : ITokenHandler
{
    public async Task<LoginUserResponseDto> VerifyOtpAsync(Token token, User user, string value, CancellationToken cancellationToken = default)
    {
        if (token.Value != value)
            throw new BadRequestException("Otp.Invalid", token.Id);

        token.IsRevoked = true;
        UserRepo.Update(user);
        TokenRepo.Update(token);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return await identityProviderService.ImpersonateUserAsync(user.UserName, cancellationToken);
    }

}
