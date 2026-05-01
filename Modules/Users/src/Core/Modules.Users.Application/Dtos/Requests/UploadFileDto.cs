using Microsoft.AspNetCore.Http;


namespace Modules.Users.Application.Dtos.Requests
{
    public class UploadFileDto
    {
        public IFormFile File { get; set; } = default!;
    }
}
