using Modules.Users.Domain.ValueObjects;

namespace Modules.Users.Application.Dtos.Requests;

public class GetSupportRequestsRequestDto
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public SupportStatus? Status { get; set; }
    public string? SubjectSearch { get; set; }
    public string? NameSearch { get; set; }
}
