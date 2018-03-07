using System;
using System.Threading.Tasks;

namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ICommandRepositoryAsync<TEntity> :
        ICreateRepository<TEntity>,
        IUpdateRepository<TEntity>,
        IDeleteRepository<TEntity>,
        IDisposable where TEntity : class
    {
        /// <summary>
        /// Save any changes to the TContext
        /// </summary>
        Task<bool> SaveChangesAsync();
    }
}