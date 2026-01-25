using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Application.EventBus
{
    public interface IIntegrationEvent
    {
        public Guid Id { get; }
        public DateTime CreatedOnUtc { get; }
    }
}