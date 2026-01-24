using Microsoft.AspNetCore.Http;
using Modules.Notifications.Domain.Abstractions;

namespace Modules.Notifications.Application.Exceptions;

public class ExpiredException(string code, params object[] args) : LocalizedHttpException(code, StatusCodes.Status410Gone, args)
{
    public string Code { get; } = code;
}
