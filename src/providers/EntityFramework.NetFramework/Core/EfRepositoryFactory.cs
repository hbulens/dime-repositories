using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;

namespace Dime.Repositories
{
    /// <summary>
    ///  Factory class that is responsible for generating repositories
    /// </summary>
    /// <typeparam name="TContext">The DbContext implementation</typeparam>
    [ExcludeFromCodeCoverage]
    public class EfRepositoryFactory<TContext> : IMultiTenantEfRepositoryFactory where TContext : DbContext
    {
        #region Constructor

        /// <summary>
        /// Constructor that only accepts the DbContext Factory and uses the default repository configuration
        /// </summary>
        /// <param name="contextFactory"></param>
        public EfRepositoryFactory(IMultiTenantDbContextFactory<TContext> contextFactory)
            : this(contextFactory, GetDefaultRepositoryConfiguration())
        {
        }

        /// <summary>
        /// Constructor that accepts the DbContext Factory and uses custom repository configuration
        /// </summary>
        /// <param name="contextFactory">The factory that actually generates the DbContext instance</param>
        /// <param name="repositoryConfiguration">The configuration for the repository</param>
        public EfRepositoryFactory(
            IMultiTenantDbContextFactory<TContext> contextFactory,
            IMultiTenantRepositoryConfiguration repositoryConfiguration)
        {
            ContextFactory = contextFactory;
            RepositoryConfiguration = repositoryConfiguration;
        }

        #endregion Constructor

        #region Properties

        protected IMultiTenantDbContextFactory<TContext> ContextFactory { get; }
        public IMultiTenantRepositoryConfiguration RepositoryConfiguration { get; set; }

        public DbContext Context { get; private set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public virtual IRepository<TEntity> Create<TEntity>() where TEntity : class, new()
            => new EfRepository<TEntity, TContext>(ContextFactory, RepositoryConfiguration);

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="connection">The connection.</param>
        /// <returns></returns>
        public virtual IRepository<TEntity> Create<TEntity>(string connection) where TEntity : class, new()
            => new EfRepository<TEntity, TContext>(ContextFactory, RepositoryConfiguration);

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity</typeparam>
        /// <param name="tenant">The tenant's identifier</param>
        /// <param name="connection">The database connection string</param>
        /// <returns></returns>
        public virtual IRepository<TEntity> Create<TEntity>(string tenant, string connection) where TEntity : class, new()
        {
            ContextFactory.Connection = connection;
            ContextFactory.Tenant = tenant;
            return new EfRepository<TEntity, TContext>(ContextFactory, RepositoryConfiguration);
        }

        /// <summary>
        /// Default settings for the repository
        /// </summary>
        /// <returns></returns>
        private static IMultiTenantRepositoryConfiguration GetDefaultRepositoryConfiguration()
            => new RepositoryConfiguration
            {
                SaveInBatch = false,
                Cached = true,
                SaveStrategy = ConcurrencyStrategy.ClientFirst
            };

        #endregion Methods
    }
}