using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;

namespace Dime.Repositories
{
    /// <summary>
    ///  Factory class that is responsible for generating repositories
    /// </summary>
    /// <typeparam name="TContext">The DbContext implementation</typeparam>
    [ExcludeFromCodeCoverage]
    public class EfRepositoryFactory<TContext> : IRepositoryFactory, IRepositoryFactory<RepositoryConfiguration>
        where TContext : DbContext
    {
        /// <summary>
        /// Constructor that only accepts the DbContext Factory and uses the default repository configuration
        /// </summary>
        /// <param name="contextFactory"></param>
        public EfRepositoryFactory(INamedDbContextFactory<TContext> contextFactory)
            : this(contextFactory, new RepositoryConfiguration())
        {
        }

        /// <summary>
        /// Constructor that accepts the DbContext Factory and uses custom repository configuration
        /// </summary>
        /// <param name="contextFactory">The factory that actually generates the DbContext instance</param>
        /// <param name="repositoryConfiguration">The configuration for the repository</param>
        public EfRepositoryFactory(
            INamedDbContextFactory<TContext> contextFactory,
            RepositoryConfiguration repositoryConfiguration)
        {
            ContextFactory = contextFactory;
            RepositoryConfiguration = repositoryConfiguration;
        }

        protected INamedDbContextFactory<TContext> ContextFactory { get; }
        public RepositoryConfiguration RepositoryConfiguration { get; }

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public virtual IRepository<TEntity> Create<TEntity>() where TEntity : class, new()
            => Create<TEntity>(RepositoryConfiguration);

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="opts">The parameters.</param>
        /// <returns></returns>
        public virtual IRepository<TEntity> Create<TEntity>(RepositoryConfiguration opts) where TEntity : class, new()
        {
            TContext dbContext = ContextFactory.Create(opts.Connection ?? RepositoryConfiguration.Connection);
            return new EfRepository<TEntity, TContext>(dbContext, RepositoryConfiguration, ContextFactory);
        }
    }
}