using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
        /// <summary>
        /// Removes the record from the data store by its identifier
        /// </summary>
        /// <param name="id">The identifier of the entity</param>
        /// <returns>Void</returns>
        public virtual async Task DeleteAsync(long id)
        {     
            TEntity item = await Context.Set<TEntity>().FindAsync(id).ConfigureAwait(false);
            if (item != default(TEntity))
            {
                Context.Set<TEntity>().Remove(item);
                await SaveChangesAsync(Context).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Removes the record from the data store by its identifiers
        /// </summary>
        /// <param name="ids">The identifiers of the entities</param>
        /// <returns>Void</returns>
        public virtual async Task DeleteAsync(IEnumerable<long> ids)
        {    
            foreach (long id in ids.Distinct().ToList())
            {
                TEntity item = await Context.Set<TEntity>().FindAsync(id).ConfigureAwait(false);
                if (item == default(TEntity))
                    continue;

                Context.Set<TEntity>().Remove(item);
            }

            await SaveChangesAsync(Context).ConfigureAwait(false);
        }

        /// <summary>
        /// Removes the record from the data store by its identifier
        /// </summary>
        /// <param name="id">The identifier of the entity</param>
        /// <param name="commit">Indicates whether or not SaveChangesAsync should be called during this call</param>
        /// <returns>Void</returns>
        public virtual async Task DeleteAsync(long id, bool commit)
        {
            TEntity item = await Context.Set<TEntity>().FindAsync(id).ConfigureAwait(false);
            if (item != default(TEntity))
            {
                Context.Set<TEntity>().Remove(item);
                if (commit)
                    await SaveChangesAsync(Context).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Removes the record from the data store
        /// </summary>
        /// <param name="entity">The disconnected entity to remove</param>
        /// <returns>Void</returns>
        public virtual async Task DeleteAsync(TEntity entity)
        {
            Context.Set<TEntity>().Attach(entity);
            Context.Set<TEntity>().Remove(entity);
            await SaveChangesAsync(Context).ConfigureAwait(false);
        }

        /// <summary>
        /// Removes the records
        /// </summary>
        /// <param name="entities">The disconnected entities to remove</param>
        /// <returns>Void</returns>
        public async Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            if (!entities.Any())
                return;
    
            foreach (TEntity entity in entities)
            {
                Context.Set<TEntity>().Attach(entity);
                Context.Set<TEntity>().Remove(entity);
            }

            await SaveChangesAsync(Context).ConfigureAwait(false);
        }

        /// <summary>
        /// Removes the record from the data store
        /// </summary>
        /// <param name="entity">The disconnected entity to remove</param>
        /// <param name="commit">Indicates whether or not SaveChangesAsync should be called during this call</param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(TEntity entity, bool commit)
        {
            Context.Set<TEntity>().Attach(entity);
            Context.Set<TEntity>().Remove(entity);

            if (commit)
                await SaveChangesAsync(Context).ConfigureAwait(false);
        }

        /// <summary>
        /// Removes the record from the data store
        /// </summary>
        /// <param name="where">The expression to execute against the data store</param>
        /// <returns>Void</returns>
        public virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> where)
        {
            IEnumerable<TEntity> entities = Context.Set<TEntity>().With(where).AsNoTracking().ToList();
            if (entities.Any())
            {
                foreach (TEntity item in entities)
                    Context.Set<TEntity>().Attach(item);

                Context.Set<TEntity>().RemoveRange(entities);
                await SaveChangesAsync(Context).ConfigureAwait(false);
            }
        }
    }
}