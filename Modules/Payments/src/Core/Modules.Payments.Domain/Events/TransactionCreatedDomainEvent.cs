using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Domain;

namespace Modules.Payments.Domain.Events
{
    public class TransactionCreatedDomainEvent(Guid transactionId) : DomainEvent
    {
        public Guid TransactionId { get; set; } = transactionId;
    }
}