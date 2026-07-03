using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modules.Notifications.Application.Dtos.Requests
{
    public class QueryNotificationsRequestDto
    {
        public int? PageSize { get; set; } = 20;
        public Guid? LastNotificationId { get; set; }
    }
}