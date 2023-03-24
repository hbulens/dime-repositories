using Microsoft.EntityFrameworkCore;

namespace Dime.Repositories
{
    public interface IEfRepositoryFactory<TContext, in TOpts> : IRepositoryFactory<RepositoryConfiguration> where TContext : DbContext
    {
        void SetContextFactory(IDbContextFactory<TContext> contextFactory);
    }
}