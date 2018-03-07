using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dime.Repositories
{
    /// <summary>
    /// Definition for a repository which supports the capability to create items to the data store
    /// </summary>
    /// <typeparam name="TEntity">The collection type</typeparam>
    public interface ICreateRepository<TEntity> : IDisposable where TEntity : class
    {
        /// <summary>
        /// Save a new item to the data store
        /// </summary>
        /// <param name="entity">The disconnected entity to store</param>
        /// <returns>The connected entity</returns>
        TEntity Create(TEntity entity);

        /// <summary>
        /// Save a new item to the data store
        /// </summary>
        /// <param name="entity">The disconnected entity to store</param>
        /// <param name="commit">Indicates whether or not SaveChangesAsync should be executed</param>
        /// <returns>The connected entity</returns>
        TEntity Create(TEntity entity, bool commit);

        /// <summary>
        /// Save a new item to the data store
        /// </summary>
        /// <param name="entity">The disconnected entity to store</param>
        /// <returns>The connected entity</returns>
        Task<TEntity> CreateAsync(TEntity entity);

        /// <summary>
        /// Save a new item to the data store
        /// </summary>
        /// <param name="entity">The disconnected entity to store</param>
        /// <param name="predicate">The predicate to validate before creating the entity</param>
        /// <returns></returns>
        Task<TEntity> CreateAsync(TEntity entity, Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Save a new item to the data store
        /// </summary>
        /// <param name="entity">The disconnected entity to store</param>
        /// <param name="commit">Indicates whether or not SaveChangesAsync should be executed</param>
        /// <returns>The connected entity</returns>
        Task<TEntity> CreateAsync(TEntity entity, bool commit);

        /// <summary>
        /// Save new items to the data store
        /// </summary>
        /// <param name="entities">The disconnected entities to store</param>
        /// <returns>The connected entity</returns>
        Task<IQueryable<TEntity>> CreateAsync(IQueryable<TEntity> entities);
    }
}