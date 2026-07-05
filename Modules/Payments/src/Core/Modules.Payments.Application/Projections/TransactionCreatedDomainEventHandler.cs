using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Application.DomainEvents;
using Common.Application.Exceptions;
using Modules.Notifications.Contracts.Contracts;
using Modules.Notifications.Contracts.Dtos;
using Modules.Payments.Application.Abstractions;
using Modules.Payments.Domain.Entities;
using Modules.Payments.Domain.Events;

namespace Modules.Payments.Application.Projections
{
    public class TransactionCreatedDomainEventHandler(
        RepositoryFactory repositoryFactory,
        INotifiyContract notifiyContract
    ) : IDomainEventHandler<TransactionCreatedDomainEvent>
    {
        public async Task HandleAsync(TransactionCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            var transaction = await repositoryFactory
                .Repository<Transaction>()
                .GetFirstOrDefaultByFilter(x => x.Id == domainEvent.TransactionId)
                ?? throw new NotFoundException("Transaction.NotFound");

            await notifiyContract.NotifyUsersAsync(
                new NotifyUsersRequestDto(
                    $"Transaction Completed",
                    $"Transaction with id {transaction.Id}\n"
                    + (!string.IsNullOrEmpty(transaction.RefNumber) ? "" : $"For Trip with reference #{transaction.RefNumber}\n")
                    + $"Note : {transaction.Note}",
                    [transaction.UserID]
                ), cancellationToken
            );
        }
    }
}