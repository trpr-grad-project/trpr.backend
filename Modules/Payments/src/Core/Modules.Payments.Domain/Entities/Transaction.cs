using Modules.Payments.Domain.Abstractions;
using Modules.Payments.Domain.Events;
using Modules.Payments.Domain.ValueObjects;

namespace Modules.Payments.Domain.Entities;

public class Transaction : Entity
{
    public Guid Id { get; private set; }
    public string? RefNumber { get; private set; }
    public ActionStatus Status { get; private set; } = ActionStatus.Pay;
    public Guid UserID { get; set; }
    public string Note { get; set; } = string.Empty;
    public double Balance { get; set; }
    public User User { get; set; } = default!;

    public static Transaction Create(string? refNumber, Guid userId, double balance, string Note, ActionStatus status)
    {
        var transaction = new Transaction
        {
            Id = Guid.CreateVersion7(),
            RefNumber = refNumber,
            UserID = userId,
            Balance = balance,
            Status = status,
            Note = Note
        };
        transaction.RaiseDomainEvent(new TransactionCreatedDomainEvent(transaction.Id));
        return transaction;
    }
}
