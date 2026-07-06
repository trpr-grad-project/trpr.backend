namespace Modules.Users.Application.Dtos.Requests;



public class UpdateProfileBulkRequestDto
{
    public string? Bio { get; set; }
    public List<int>? LanguageIds { get; set; }
    public List<int>? InterestIds { get; set; }
    public List<int>? VibeIds { get; set; }
    public string? AvatarUrl { get; set; }
}
