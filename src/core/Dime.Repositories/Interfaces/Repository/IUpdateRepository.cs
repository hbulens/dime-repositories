using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dime.Repositories
{
    /// <summary>
    /// Definition for a repository which supports the capability to update items in the data store
    /// </summary>
    /// <typeparam name="TEntity">The collection type</typeparam>
    public interface IUpdateRepository<TEntity> : IDisposable where TEntity : class
    {
        /// <summary>
        /// Updates the existing entity.
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <param name="commitChanges">Indication whether or not the SaveChangesAsync should be called during this call</param>
        /// <returns>The updated entity</returns>
        TEntity Update(TEntity entity);

        /// <summary>
        /// Updates the existing entity.
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <param name="commitChanges">Indication whether or not the SaveChangesAsync should be called during this call</param>
        /// <returns>The updated entity</returns>
        TEntity Update(TEntity entity, bool commitChanges = true);

        /// <summary>
        /// Updates the existing entity.
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <param name="commitChanges">Indication whether or not the SaveChangesAsync should be called during this call</param>
        /// <returns>The updated entity</returns>
        Task<TEntity> UpdateAsync(TEntity entity, bool commitChanges = true);

        /// <summary>
        /// Updates the entities
        /// </summary>
        /// <param name="entities">The entities to update</param>
        /// <param name="commitChanges">Indication whether or not the SaveChangesAsync should be called during this call</param>
        /// <returns>Void</returns>
        Task UpdateAsync(IEnumerable<TEntity> entities, bool commitChanges = true);

        /// <summary>
        /// Updates the existing entity.
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <param name="properties">The properties of the entity to update</param>
        /// <returns>The updated entity</returns>
        Task<TEntity> UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties);

        /// <summary>
        /// Updates the existing entity.
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <param name="properties">The properties of the entity to update</param>
        /// <returns>The updated entity</returns>
        Task<TEntity> UpdateAsync(TEntity entity, params string[] properties);
    }
}