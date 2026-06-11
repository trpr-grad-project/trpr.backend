using Microsoft.Extensions.DependencyInjection;

namespace Modules.Payments.Application.Abstractions
{
    public class RepositoryFactory(IServiceProvider serviceProvider)
    {
        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            return serviceProvider.GetRequiredService<IRepository<TEntity>>();
        }
    }
}