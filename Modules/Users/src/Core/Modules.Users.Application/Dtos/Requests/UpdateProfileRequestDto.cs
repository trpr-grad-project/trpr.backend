namespace Modules.Users.Application.Dtos.Requests;

public class UpdateProfileBulkRequestDto
{
    public string? Bio { get; set; }
    public List<Guid>? LanguageIds { get; set; }
    public List<Guid>? InterestIds { get; set; }
    public List<Guid>? VibeIds { get; set; }
}
