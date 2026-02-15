using System.Net;
using Microsoft.Extensions.Logging;
using Modules.Users.Infrastructure.Dtos.Requests;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Application.Abstractions.Identity;
using Modules.Users.Application.Interfaces;
using Modules.Users.Infrastructure.Clients;
using Common.Application.Exceptions;

namespace Modules.Users.Infrastructure.Services;

internal sealed class IdentityProviderService(AdminKeyCloakClient adminKeyCloakClient, TokenKeyCloackCLient tokenKeyCloackCLient, ILogger<IdentityProviderService> logger)
    : IIdentityProviderService
{

    public async Task<LoginUserResponseDto> ImpersonateUserAsync(string username, CancellationToken cancellationToken = default)
    {
        var authResponse = await tokenKeyCloackCLient.ImpersonateUserAsync(username, cancellationToken);
        return new LoginUserResponseDto()
        {
            AccessToken = authResponse.AccessToken,
            RefreshToken = authResponse.RefreshToken,
            ProfileSetupCompleted = false
        };
    }

    // POST /admin/realms/{realm}/users
    public async Task<LoginUserResponseDto> LoginUserAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            var authResponse = await tokenKeyCloackCLient.LoginUserAsync(email, password, cancellationToken);
            return new LoginUserResponseDto()
            {
                AccessToken = authResponse.AccessToken,
                RefreshToken = authResponse.RefreshToken,
                ProfileSetupCompleted = false
            };
        }
        catch (HttpRequestException exception)
        {
            switch (exception.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new NotAuthorizedException("Invalid.Creds");
            }
            throw new NotImplementedException("Unhandled Status Code At the LoginUserAsync in IdentityProviderService with message" + exception.Message);
        }
    }

    public Task<LoginUserResponseDto> RefreshUserAsync(string token, CancellationToken cancellationToken = default)
    {
        var authResponse = tokenKeyCloackCLient.RefreshTokenAsync(token, cancellationToken);
        return Task.FromResult(new LoginUserResponseDto()
        {
            AccessToken = authResponse.Result.AccessToken,
            RefreshToken = authResponse.Result.RefreshToken,
            ProfileSetupCompleted = false
        });
    }


    public async Task<string> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default)
    {

        try
        {
            var userRepresentation = new UserRepresentation(
                user.UserName,
                user.Email,
                true,
                user.FirstName,
                user.LastName,
                false,
                [new CredentialRepresentation("password", user.Password, false)]);

            string identityId = await adminKeyCloakClient.RegisterUserAsync(userRepresentation, cancellationToken);
            return identityId;
        }
        catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.Conflict)
        {
            logger.LogError("User registration failed {exception}", exception);
            throw new ConflictException("User.Conflict", user.UserName);
        }
    }

    public Task RemoveUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            return adminKeyCloakClient.RemoveUserAsync(userId, cancellationToken);
        }
        catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.NotFound)
        {
            logger.LogError("User removal failed {exception}", exception);
            throw new NotFoundException("User.NotFound", userId);
        }
    }

    public Task EnableUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            return adminKeyCloakClient.ToggleUserAsync(userId.ToString(), true, cancellationToken);
        }
        catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.NotFound)
        {
            logger.LogError("Toggle enable/disable user failed {exception}", exception);
            throw new NotFoundException("User.NotFound", userId);
        }
    }

    public async Task UpdatePassword(Guid userId, string password, CancellationToken cancellationToken = default)
    {
        await adminKeyCloakClient.ResetPasswordAsync(userId, password, cancellationToken);
    }
}
