namespace Modules.Users.Application.Dtos.Requests;

public class SearchPostRequestDto
{
    public string? SearchTerm { get; set; } = null;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}