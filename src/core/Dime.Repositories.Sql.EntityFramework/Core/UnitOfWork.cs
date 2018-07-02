using System.Threading.Tasks;

namespace Dime.Repositories
{
    /// <summary>
    /// Unit of Work class responsible for generating repositories
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        #region Constructor

        /// <summary>
        ///
        /// </summary>
        /// <param name="repositoryFactory"></param>
        public UnitOfWork(IMultiTenantEfRepositoryFactory repositoryFactory)
        {
            RepositoryFactory = repositoryFactory;
        }

        #endregion Constructor

        #region Properties

        private IMultiTenantEfRepositoryFactory RepositoryFactory { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IRepository<T> GetRepository<T>() where T : class, new()
            => RepositoryFactory.Create<T>();

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">The connection.</param>
        /// <returns></returns>
        public IRepository<T> GetRepository<T>(string connection) where T : class, new()
            => RepositoryFactory.Create<T>(connection);

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tenant">The tenant.</param>
        /// <param name="connection">The connection.</param>
        /// <returns></returns>
        public IRepository<T> GetRepository<T>(string tenant, string connection) where T : class, new()
            => RepositoryFactory.Create<T>(tenant, connection);

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            int result = RepositoryFactory.Context.SaveChanges();
            return 0 < result;
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveChangesAsync()
        {
            int result = await RepositoryFactory.Context.SaveChangesAsync();
            return 0 < result;
        }

        #endregion Methods
    }
}