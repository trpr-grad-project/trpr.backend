using Microsoft.EntityFrameworkCore;
using Modules.Payments.Domain.Entities;

namespace Modules.Payments.Application.Abstractions;

public interface IPaymentsDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
}
