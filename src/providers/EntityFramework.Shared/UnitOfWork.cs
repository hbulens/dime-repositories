using System;
using System.Diagnostics.CodeAnalysis;
#if NET461
using System.Data.Entity;
#else
using Microsoft.EntityFrameworkCore;
#endif

namespace Dime.Repositories
{
    /// <summary>
    /// Unit of Work class responsible for generating repositories
    /// </summary>
    [ExcludeFromCodeCoverage]
    [Obsolete("Will be removed in 2.0.0")]
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="repositoryFactory"></param>
        public UnitOfWork(INamedEfRepositoryFactory repositoryFactory)
        {
            RepositoryFactory = repositoryFactory;
        }

        private INamedEfRepositoryFactory RepositoryFactory { get; }

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
        
        public IRepository<T> GetRepository<T>(string tenant, string connection) where T : class, new()
            => RepositoryFactory.Create<T>(connection);

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
        public async System.Threading.Tasks.Task<bool> SaveChangesAsync()
        {
            int result = await RepositoryFactory.Context.SaveChangesAsync().ConfigureAwait(false);
            return 0 < result;
        }
    }
}