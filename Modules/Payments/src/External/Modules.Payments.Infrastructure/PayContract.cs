using Common.Application.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Modules.Payments.Application.Abstractions;
using Modules.Payments.Contracts.Contracts;
using Modules.Payments.Domain.Entities;
using Modules.Payments.Domain.ValueObjects;

namespace Modules.Payments.Infrastructure;

public class PayContract(
    RepositoryFactory repositoryFactory,
    IUnitOfWork unitOfWork) : IPayContract
{
    public async Task Pay(string? refNumber, Guid userId, double balance, string Note)
    {
        var user = await repositoryFactory
                .Repository<User>()
                .GetQueryable()
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("User.NotFound");

        var transaction = Transaction.Create(refNumber, userId, balance, Note, ActionStatus.Pay);

        user.Charge(balance);

        repositoryFactory.Repository<Transaction>().Add(transaction);

        await unitOfWork.SaveChangesAsync();
    }

    public async Task Gain(string? refNumber, Guid userId, double balance, string Note)
    {
        var user = await repositoryFactory
                .Repository<User>()
                .GetQueryable()
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("User.NotFound");

        var transaction = Transaction.Create(refNumber, userId, balance, Note, ActionStatus.Gain);

        user.Recive(balance);

        repositoryFactory.Repository<Transaction>().Add(transaction);

        await unitOfWork.SaveChangesAsync();
    }

}
