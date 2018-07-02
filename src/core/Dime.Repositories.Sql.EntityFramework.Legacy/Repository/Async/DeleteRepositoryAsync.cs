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
        /// <history>
        /// [HB] 18/01/2016 - Only delete if records exists in data store
        /// </history>
        public virtual async Task DeleteAsync(long id)
        {
            using (TContext ctx = Context)
            {
                TEntity item = await ctx.Set<TEntity>().FindAsync(id);
                if (item != default(TEntity))
                {
                    ctx.Set<TEntity>().Remove(item);
                    await SaveChangesAsync(ctx);
                }
            }
        }

        /// <summary>
        /// Removes the record from the data store by its identifier
        /// </summary>
        /// <param name="id">The identifier of the entity</param>
        /// <param name="commit">Indicates whether or not SaveChangesAsync should be called during this call</param>
        /// <returns>Void</returns>
        /// <history>
        /// [HB] 18/01/2016 - Only delete if records exists in data store
        /// </history>
        public virtual async Task DeleteAsync(long id, bool commit)
        {
            using (TContext ctx = Context)
            {
                TEntity item = await ctx.Set<TEntity>().FindAsync(id);
                if (item != default(TEntity))
                {
                    ctx.Set<TEntity>().Remove(item);
                    if (commit)
                        await SaveChangesAsync(ctx);
                }
            }
        }

        /// <summary>
        /// Removes the record from the data store
        /// </summary>
        /// <param name="entity">The disconnected entity to remove</param>
        /// <returns>Void</returns>
        public virtual async Task DeleteAsync(TEntity entity)
        {
            using (TContext ctx = Context)
            {
                ctx.Set<TEntity>().Attach(entity);
                ctx.Set<TEntity>().Remove(entity);
                await SaveChangesAsync(ctx);
            }
        }

        /// <summary>
        /// Removes the records
        /// </summary>
        /// <param name="entities">The disconnected entities to remove</param>
        /// <returns>Void</returns>
        public async Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            using (TContext ctx = Context)
            {
                foreach (TEntity entity in entities)
                {
                    ctx.Set<TEntity>().Attach(entity);
                    ctx.Set<TEntity>().Remove(entity);
                }

                await SaveChangesAsync(ctx);
            }
        }

        /// <summary>
        /// Removes the record from the data store
        /// </summary>
        /// <param name="entity">The disconnected entity to remove</param>
        /// <param name="commit">Indicates whether or not SaveChangesAsync should be called during this call</param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(TEntity entity, bool commit)
        {
            using (TContext ctx = Context)
            {
                ctx.Set<TEntity>().Attach(entity);
                ctx.Set<TEntity>().Remove(entity);

                if (commit)
                    await SaveChangesAsync(ctx);
            }
        }

        /// <summary>
        /// Removes the record from the data store
        /// </summary>
        /// <param name="where">The expression to execute against the data store</param>
        /// <returns>Void</returns>
        public virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> where)
        {
            using (TContext ctx = Context)
            {
                IEnumerable<TEntity> entities = ctx.Set<TEntity>().With(where).AsNoTracking().ToList();
                if (entities != null)
                {
                    foreach (TEntity item in entities)
                    {
                        ctx.Set<TEntity>().Attach(item);
                    }

                    ctx.Set<TEntity>().RemoveRange(entities);
                    await SaveChangesAsync(ctx);
                }
            }
        }
    }
}