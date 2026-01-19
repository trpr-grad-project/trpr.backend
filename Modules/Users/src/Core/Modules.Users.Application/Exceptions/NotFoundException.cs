using Microsoft.AspNetCore.Http;
using Modules.Users.Domain.Abstractions;

namespace Modules.Users.Application.Exceptions
{
    public class NotFoundException(string code, params object[] args) : LocalizedHttpException(code, StatusCodes.Status404NotFound, args)
    {
        public string Code { get; init; } = code;
    }
}
