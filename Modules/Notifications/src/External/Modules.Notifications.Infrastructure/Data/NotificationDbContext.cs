using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Modules.Notifications.Application.Abstractions;
using Common.Application;
using Common.Domain;
using Modules.Notifications.Infrastructure.Outbox;
using Modules.Notifications.Infrastructure.Inbox;
using Modules.Notifications.Domain.Entities;
using Modules.Notifications.Domain.Abstractions;


namespace Modules.Notifications.Infrastructure.Data;

public class NotificationDbContext(DbContextOptions<NotificationDbContext> options) : DbContext(options), IUnitOfWork, INotificationDbContext
{
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Template> Templates { get; set; }
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
    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    // Async version
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        // This tracks all changed entries and updates the timestamps
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is Entity &&
                        (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (Template)entry.Entity;
            if (entry.State == EntityState.Added)
            {
                entity.CreatedAtUTC = DateTime.UtcNow;
                entity.UpdatedAtUTC = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entity.UpdatedAtUTC = DateTime.UtcNow;
            }
        }
    }
}
