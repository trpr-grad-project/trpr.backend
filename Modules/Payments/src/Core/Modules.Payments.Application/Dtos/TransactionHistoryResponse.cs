using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Modules.Payments.Domain.ValueObjects;

namespace Modules.Payments.Application.Dtos
{
    public sealed record TransactionHistoryResponse(Guid Id, string? RefNumber, ActionStatus Status, double Amount, DateTime CreatedAtUtc, string? Note);
}