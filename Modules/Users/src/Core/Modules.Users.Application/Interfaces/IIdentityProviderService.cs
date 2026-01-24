using Modules.Users.Application.Abstractions.Identity;
using Modules.Users.Application.Dtos.Responses;

namespace Modules.Users.Application.Interfaces;

public interface IIdentityProviderService
{
    Task<string> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default);
    Task RemoveUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task EnableUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<LoginUserResponseDto> ImpersonateUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<LoginUserResponseDto> RefreshUserAsync(string token, CancellationToken cancellationToken = default);
    Task<LoginUserResponseDto> LoginUserAsync(string username, string password, CancellationToken cancellationToken = default);
}
