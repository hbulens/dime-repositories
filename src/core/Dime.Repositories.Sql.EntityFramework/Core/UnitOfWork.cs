using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Dime.Repositories
{
    /// <summary>
    /// Unit of Work class responsible for generating repositories
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext, new()
    {
        #region Constructor

        /// <summary>
        ///
        /// </summary>
        /// <param name="repositoryFactory"></param>
        public UnitOfWork(IMultiTenantEfRepositoryFactory repositoryFactory)
        {
            this.RepositoryFactory = repositoryFactory;
        }

        #endregion Constructor

        #region Properties

        private IMultiTenantEfRepositoryFactory RepositoryFactory { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">The connection.</param>
        /// <returns></returns>
        public IRepository<T> GetRepository<T>() where T : class, new()
        {
            return this.RepositoryFactory.Create<T>();
        }

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">The connection.</param>
        /// <returns></returns>
        public IRepository<T> GetRepository<T>(string connection) where T : class, new()
        {
            return this.RepositoryFactory.Create<T>(connection);
        }

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tenant">The tenant.</param>
        /// <param name="connection">The connection.</param>
        /// <returns></returns>
        public IRepository<T> GetRepository<T>(string tenant, string connection) where T : class, new()
        {
            return this.RepositoryFactory.Create<T>(tenant, connection);
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            int result = this.RepositoryFactory.Context.SaveChanges();
            return 0 < result;
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveChangesAsync()
        {
            int result = await this.RepositoryFactory.Context.SaveChangesAsync();
            return 0 < result;
        }

        #endregion Methods
    }
}