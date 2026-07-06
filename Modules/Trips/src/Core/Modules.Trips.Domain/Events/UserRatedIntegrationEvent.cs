using System;
using System.Collections.Generic;
using System.Text;
using Common.Domain;

namespace Modules.Users.Domain.Events
{
    public class UserRatedIntegrationEvent : DomainEvent
    {
        public UserRatedIntegrationEvent(Guid id, double value1, int value2)
        {
            Id = id;
            Value1 = value1;
            Value2 = value2;
        }

        public double Value1 { get; }
        public int Value2 { get; }
    }
}
