namespace Modules.Users.Infrastructure.Dtos.Requests;

internal sealed record UserRepresentation(
    string Username,
    string FirstName,
    string LastName,
    bool Enabled,
    CredentialRepresentation[] Credentials);
