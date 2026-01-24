using Microsoft.AspNetCore.Http;
using Modules.Notifications.Domain.Abstractions;

namespace Modules.Notifications.Application.Exceptions
{
    public class NotFoundException(string code, params object[] args) : LocalizedHttpException(code, StatusCodes.Status404NotFound, args)
    {
        public string Code { get; init; } = code;
    }
}
