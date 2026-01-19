using Modules.Users.Application.Abstractions.Identity;
using Modules.Users.Application.Dtos.Responses;

namespace Modules.Users.Application.Interfaces;

public interface IIdentityProviderService
{
    Task<string> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default);
    Task<LoginUserResponse> RefreshUserAsync(string token, CancellationToken cancellationToken = default);
    Task<LoginUserResponse> LoginUserAsync(string email, string password, CancellationToken cancellationToken = default);
}
