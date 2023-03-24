using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Dime.Repositories
{
    [ExcludeFromCodeCoverage]
    public class EfRepositoryFactory<TContext> : IEfRepositoryFactory<TContext, RepositoryConfiguration>
        where TContext : DbContext
    {
        public EfRepositoryFactory(IDbContextFactory<TContext> contextFactory)
            : this(contextFactory, new RepositoryConfiguration())
        {
        }

        public EfRepositoryFactory(IDbContextFactory<TContext> contextFactory, RepositoryConfiguration repositoryConfiguration)
        {
            ContextFactory = contextFactory;
            RepositoryConfiguration = repositoryConfiguration;
        }

        private IDbContextFactory<TContext> ContextFactory { get; set; }
        private RepositoryConfiguration RepositoryConfiguration { get; set; }

        public virtual IRepository<TEntity> Create<TEntity>() where TEntity : class, new()
            => Create<TEntity>(RepositoryConfiguration);

        public virtual IRepository<TEntity> Create<TEntity>(RepositoryConfiguration opts) where TEntity : class, new()
            => new EfRepository<TEntity, TContext>(ContextFactory, opts);

        public void SetContextFactory(IDbContextFactory<TContext> contextFactory)
        {
            ContextFactory = contextFactory;
        }
    }
}