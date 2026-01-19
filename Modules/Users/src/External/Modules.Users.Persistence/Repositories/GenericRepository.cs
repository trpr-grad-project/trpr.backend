using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Metadata;
using Modules.Users.Application.Abstractions;
using Modules.Users.Persistence.Data;

namespace Modules.Users.Persistence.Repositories;

public class GenericRepository<T, TKey>(AppDbContext context, ILogger<GenericRepository<T, TKey>> logger) : IGenericRepository<T, TKey> where T : class
{
    public async Task<T?> GetById(TKey id)
    {
        logger.LogInformation("Fetching entity of type {EntityType} with Id {Id}", typeof(T).Name, id);
        var entity = await context.Set<T>().FindAsync(id);
        logger.LogInformation("Fetched entity of type {EntityType}: {@Result}", typeof(T).Name, entity);
        return entity;
    }

    public async Task<T?> GetByIdForUpdate(TKey id)
    {
        logger.LogInformation("Fetching entity of type {EntityType} with Id {Id} for update", typeof(T).Name, id);

        if (context.Database.CurrentTransaction == null)
            throw new InvalidOperationException("FOR UPDATE requires an active transaction.");

        var entityType = context.Model.FindEntityType(typeof(T));

        var tableName = entityType!.GetTableName();
        var keyProperty = entityType!.FindPrimaryKey()?.Properties.FirstOrDefault();
        var storeObject = StoreObjectIdentifier.Create(entityType, StoreObjectType.Table);
        var keyColumnName = keyProperty!.GetColumnName(storeObject!.Value);

        var sql = $"SELECT * FROM \"{tableName}\" WHERE \"{keyColumnName}\" = @p0 FOR UPDATE";

        var entity = await context.Set<T>().FromSqlRaw(sql, id).FirstOrDefaultAsync();

        logger.LogInformation("Fetched entity of type {EntityType} for update: {@Result}", typeof(T).Name, entity);

        return entity;
    }

    public void Add(T entity)
    {
        logger.LogInformation("Adding entity of type {EntityType}: {@Entity}", typeof(T).Name, entity);
        context.Set<T>().Add(entity);
    }
    public void Remove(T entity)
    {
        logger.LogInformation("Removing entity of type {EntityType}: {@Entity}", typeof(T).Name, entity);
        context.Set<T>().Remove(entity);
    }
    public void Update(T entity)
    {
        logger.LogInformation("Updating entity of type {EntityType}: {@Entity}", typeof(T).Name, entity);
        context.Set<T>().Update(entity);
    }
}
