using System;
using System.Collections.Generic;
using System.Text;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Application.Mappers
{
    public static class SupportRequestResponseMapper
    {
        public static SupportRequestResponseDto ToResponseDto(this SupportRequest supportRequest)
        {
            return new SupportRequestResponseDto
            {
                Id = supportRequest.Id,
                User = supportRequest.user.ToResponseDto(),
                Subject = supportRequest.Subject,
                Description = supportRequest.Description,
                Status = supportRequest.Status.ToString(),
                CreatedAtUTC = supportRequest.CreatedAtUTC
            };
        }
    }
}
