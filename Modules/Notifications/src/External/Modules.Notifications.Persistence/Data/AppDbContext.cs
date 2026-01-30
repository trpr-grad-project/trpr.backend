using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Modules.Notifications.Domain.Entities.Outbox;
using Modules.Notifications.Application.Abstractions;
using Common.Application;
using Modules.Notifications.Domain.Entities.Inbox;

namespace Modules.Notifications.Persistence.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IUnitOfWork, IAppDbContext
{
    public virtual DbSet<OutboxMessage> OutboxMessages { get; set; }
    public virtual DbSet<OutboxConsumerMessage> OutboxConsumerMessages { get; set; }
    public virtual DbSet<InboxMessage> InboxMessages { get; set; }
    public virtual DbSet<InboxConsumerMessage> InboxConsumerMessages { get; set; }
    private IDbContextTransaction? _transaction;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema.Notifications);
        modelBuilder.ApplyConfigurationsFromAssembly
        (AssemblyRefrence.Assembly);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
            return;

        _transaction = await Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            throw new InvalidOperationException("No active transaction to commit.");

        await SaveChangesAsync(cancellationToken);
        await _transaction.CommitAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            return;

        await _transaction.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }
}
