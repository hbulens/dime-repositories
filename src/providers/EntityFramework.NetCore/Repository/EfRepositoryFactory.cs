using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Dime.Repositories
{
    [ExcludeFromCodeCoverage]
    public class EfRepositoryFactory<TContext> : IRepositoryFactory<RepositoryConfiguration>
        where TContext : DbContext
    {
        public EfRepositoryFactory(INamedDbContextFactory<TContext> contextFactory)
            : this(contextFactory, GetDefaultRepositoryConfiguration())
        {
        }

        public EfRepositoryFactory(
            INamedDbContextFactory<TContext> contextFactory,
            RepositoryConfiguration repositoryConfiguration)
        {
            ContextFactory = contextFactory;
            RepositoryConfiguration = repositoryConfiguration;
        }

        protected INamedDbContextFactory<TContext> ContextFactory { get; }
        public RepositoryConfiguration RepositoryConfiguration { get; set; }

        public virtual IRepository<TEntity> Create<TEntity>() where TEntity : class, new()
            => Create<TEntity>(RepositoryConfiguration);

        public virtual IRepository<TEntity> Create<TEntity>(RepositoryConfiguration opts) where TEntity : class, new()
        => new EfRepository<TEntity, TContext>(
            !string.IsNullOrEmpty(opts.Connection) ? opts : RepositoryConfiguration,
                ContextFactory);

        private static RepositoryConfiguration GetDefaultRepositoryConfiguration()
            => new()
            {
                SaveInBatch = false,
                Cached = true,
                SaveStrategy = ConcurrencyStrategy.ClientFirst
            };
    }
}