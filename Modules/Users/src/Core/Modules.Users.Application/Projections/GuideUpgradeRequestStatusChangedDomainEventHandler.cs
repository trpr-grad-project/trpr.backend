using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Application.DomainEvents;
using Modules.Notifications.Contracts.Contracts;
using Modules.Notifications.Contracts.Dtos;
using Modules.Users.Domain.Events;
using Modules.Users.Domain.ValueObjects;

namespace Modules.Users.Application.Projections
{
    public class GuideUpgradeRequestStatusChangedDomainEventHandler(INotifiyContract notifyContract) : IDomainEventHandler<GuideUpgradeRequestStatusChangedDomainEvent>
    {
        public async Task HandleAsync(GuideUpgradeRequestStatusChangedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            await notifyContract.NotifyAsync(new SystemNotifyRequestDto(
                NotifyEmail: true,
                NotifyPhone: true,
                NotifySystem: true,
                TemplateType: TemplateTypeSwitch(domainEvent.NewStatus),
                ToEmails: [],
                ToPhoneNumbers: [],
                KeyValuePairs: new Dictionary<string, string>
                {
                    { "rejectionReason" , domainEvent.RejectionReason ?? string.Empty }
                }
            ), cancellationToken);
        }
        private TemplateType TemplateTypeSwitch(ApproveStatus status)
        {
            return status switch
            {
                ApproveStatus.Approved => TemplateType.ApprovalMessage,
                ApproveStatus.Rejected => TemplateType.RejectionMessage,
                _ => throw new NotSupportedException($"Unsupported approve status: {status}")
            };
        }
    }
}
