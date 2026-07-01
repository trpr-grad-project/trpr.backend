using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Modules.Conversations.Application.Abstractions;
using Modules.Conversations.Infrastructure.Data;

namespace Modules.Conversations.Infrastructure.Repositories
{
    public class Repository<TEntity>(ConversationsDbContext context) : IRepository<TEntity>
        where TEntity : class
    {
        public void Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
        }

        public void Delete(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
        }

        public void Attach(TEntity entity)
        {
            context.Set<TEntity>().Attach(entity);
        }

        public async Task<TEntity?> GetForUpdateAsync(
    object id)
        {
            var entityType = context.Model.FindEntityType(typeof(TEntity))
                ?? throw new InvalidOperationException();

            var table = entityType.GetTableName();
            var schema = entityType.GetSchema();

            var key = entityType.FindPrimaryKey()
                ?? throw new InvalidOperationException();

            if (key.Properties.Count != 1)
                throw new NotSupportedException(
                    "Composite keys are not supported.");

            var keyProperty = key.Properties[0];

            var storeObject =
                StoreObjectIdentifier.Table(table!, schema);

            var keyColumn =
                keyProperty.GetColumnName(storeObject);

            var sql =
                $"""
                SELECT *
                FROM "{schema}"."{table}"
                WHERE "{keyColumn}" = @p0
                FOR UPDATE
                """;

            return await context.Set<TEntity>()
                .FromSqlRaw(sql, id!)
                .SingleOrDefaultAsync();
        }

        public async Task<ICollection<TEntity>> GetByExpWhereAsync(params Expression<Func<TEntity, bool>>[] filters)
        {
            IQueryable<TEntity> query = context.Set<TEntity>();
            foreach (var filter in filters)
                query = query.Where(filter);
            return await query.ToListAsync();
        }

        public Task<TEntity?> GetFirstOrDefaultByFilter(Expression<Func<TEntity, bool>> filter, params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includes)
        {
            var query = context.Set<TEntity>().Where(filter);
            foreach (var include in includes)
                query = include(query);
            return query.FirstOrDefaultAsync();
        }

        public IQueryable<TEntity> GetQueryable()
        {
            return context.Set<TEntity>().AsQueryable();
        }

        public void Update(TEntity entity)
        {
            context.Set<TEntity>().Update(entity);
        }
    }
}