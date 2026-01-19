using System.Net;
using Modules.Users.Application.Exceptions;
using Microsoft.Extensions.Logging;
using Modules.Users.Infrastructure.Dtos.Requests;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Application.Abstractions.Identity;
using Modules.Users.Application.Interfaces;
using Modules.Users.Infrastructure.Clients;

namespace Modules.Users.Infrastructure.Services;

internal sealed class IdentityProviderService(AdminKeyCloakClient adminKeyCloakClient, TokenKeyCloackCLient tokenKeyCloackCLient, ILogger<IdentityProviderService> logger)
    : IIdentityProviderService
{
    // POST /admin/realms/{realm}/users
    public async Task<LoginUserResponse> LoginUserAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            var authResponse = await tokenKeyCloackCLient.LoginUserAsync(email, password, cancellationToken);
            return new LoginUserResponse()
            {
                AccessToken = authResponse.AccessToken,
                RefreshToken = authResponse.RefreshToken
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

    public Task<LoginUserResponse> RefreshUserAsync(string token, CancellationToken cancellationToken = default)
    {
        var authResponse = tokenKeyCloackCLient.RefreshTokenAsync(token, cancellationToken);
        return Task.FromResult(new LoginUserResponse()
        {
            AccessToken = authResponse.Result.AccessToken,
            RefreshToken = authResponse.Result.RefreshToken
        });
    }


    public async Task<string> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default)
    {
        var userRepresentation = new UserRepresentation(
            user.Email,
            user.Email,
            user.FirstName,
            user.LastName,
            true,
            true,
            [new CredentialRepresentation("password", user.Password, false)]);

        try
        {
            string identityId = await adminKeyCloakClient.RegisterUserAsync(userRepresentation, cancellationToken);
            return identityId;
        }
        catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.Conflict)
        {
            logger.LogError("User registration failed {exception}", exception);
            throw new ConflictException("User.Conflict.Email", user.Email);
        }
    }
}
