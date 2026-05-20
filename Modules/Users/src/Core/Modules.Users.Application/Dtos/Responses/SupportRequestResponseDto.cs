namespace Modules.Users.Application.Dtos.Responses;

public class SupportRequestResponseDto
{
    public Guid Id { get; set; }
    public UserResponseDto User { get; set; } = new();
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAtUTC { get; set; }
}
