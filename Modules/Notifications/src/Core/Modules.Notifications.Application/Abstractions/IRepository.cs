using System.Linq.Expressions;

namespace Modules.Notifications.Application.Abstractions
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        public void Add(TEntity entity);
        public void Update(TEntity entity);
        public void Delete(TEntity entity);
        public void Attach(TEntity entity);
        public Task<TEntity?> GetFirstOrDefaultByFilter(Expression<Func<TEntity, bool>> filter, params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includes);
        public Task<ICollection<TEntity>> GetByExpWhereAsync(params Expression<Func<TEntity, bool>>[] filters);
        public Task<List<TEntity>> GetForUpdateAsync<T>(T[] ids);
        public IQueryable<TEntity> GetQueryable();
    }
}