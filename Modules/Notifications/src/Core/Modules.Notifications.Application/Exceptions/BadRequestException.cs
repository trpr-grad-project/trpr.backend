using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Modules.Notifications.Domain.Abstractions;

namespace Modules.Notifications.Application.Exceptions
{
    public class BadRequestException : LocalizedHttpException
    {
        public BadRequestException(string errorCode, params object[] args) : base(errorCode, StatusCodes.Status400BadRequest, args)
        {
        }
    }
}