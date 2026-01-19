using System.Text.Json.Serialization;
using Modules.Users.Application.Helpers;

namespace Modules.Users.Application.Dtos.Requests;

public class LoginUserRequestDto
{
    public string Email { get; set; } = string.Empty;
    [JsonConverter(typeof(MaskedStringConverter))]
    public string Password { get; set; } = string.Empty;
}