using System.Threading.Tasks;

namespace Dime.Repositories
{
    /// <summary>
    /// Interface that obligates the retrieval and storage of repositories
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Creates a repository for the requested type
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <returns>An instantiated and connected repository</returns>
        IRepository<T> GetRepository<T>() where T : class, new();

        /// <summary>
        /// Creates a repository for the requested type
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="connection">The connection string to the data store</param>
        /// <returns>An instantiated and connected repository</returns>
        IRepository<T> GetRepository<T>(string connection) where T : class, new();

        /// <summary>
        /// Creates a repository for the requested type
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="tenant">The tenant identifier</param>
        /// <param name="connection">The connection string to the data store</param>
        /// <returns>An instantiated and connected repository</returns>
        IRepository<T> GetRepository<T>(string tenant, string connection) where T : class, new();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        bool SaveChanges();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveChangesAsync();
    }
}