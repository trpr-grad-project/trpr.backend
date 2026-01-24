using System.Net.Http.Json;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Infrastructure.Dtos.Requests;
using Modules.Users.Infrastructure.Dtos.Responses;

namespace Modules.Users.Infrastructure.Clients;

internal sealed class AdminKeyCloakClient(HttpClient httpClient)
{
    internal async Task<LoginResponseRepresentation> ImpersonateUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(
            $"users/{userId}/impersonation",
            null,
            cancellationToken);

        httpResponseMessage.EnsureSuccessStatusCode();

        return await httpResponseMessage.Content.ReadFromJsonAsync<LoginResponseRepresentation>(cancellationToken: cancellationToken) ?? throw new InvalidOperationException("Failed to read authorization token from response.");
    }
    internal async Task RemoveUserAsync(Guid identityId, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage httpResponseMessage = await httpClient.DeleteAsync(
            $"users/{identityId}",
            cancellationToken);

        httpResponseMessage.EnsureSuccessStatusCode();
    }
    internal async Task ToggleUserAsync(string identityId, bool Enabled, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage httpResponseMessage = await httpClient.PutAsJsonAsync(
            $"users/{identityId}",
            new EnableUserRepresentation(Enabled),
            cancellationToken);

        httpResponseMessage.EnsureSuccessStatusCode();
    }
    internal async Task<string> RegisterUserAsync(UserRepresentation user, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync(
            "users",
            user,
            cancellationToken);

        httpResponseMessage.EnsureSuccessStatusCode();

        return ExtractIdentityIdFromLocationHeader(httpResponseMessage);
    }
    private static string ExtractIdentityIdFromLocationHeader(
        HttpResponseMessage httpResponseMessage)
    {
        const string usersSegmentName = "users/";

        string? locationHeader = httpResponseMessage.Headers.Location?.PathAndQuery;

        if (locationHeader is null)
        {
            throw new InvalidOperationException("Location header is null");
        }

        int userSegmentValueIndex = locationHeader.IndexOf(
            usersSegmentName,
            StringComparison.InvariantCultureIgnoreCase);

        string identityId = locationHeader.Substring(userSegmentValueIndex + usersSegmentName.Length);

        return identityId;
    }
}
