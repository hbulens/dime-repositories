using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
        /// <summary>
        /// Updates the existing entity.
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <param name="commitChanges">Indication whether or not the SaveChangesAsync should be called during this call</param>
        /// <returns></returns>
        public virtual async Task<TEntity> UpdateAsync(TEntity entity, bool commitChanges = true)
        {
            using (TContext ctx = Context)
            {
                ctx.Set<TEntity>().Attach(entity);
                ctx.Entry(entity).State = EntityState.Modified;

                if (commitChanges)
                    await SaveChangesAsync(ctx);

                return entity;
            }
        }

        /// <summary>
        /// Updates the entities
        /// </summary>
        /// <param name="entities">The entities to update</param>
        /// <param name="commitChanges">Indication whether or not the SaveChangesAsync should be called during this call</param>
        /// <returns></returns>
        public async Task UpdateAsync(IEnumerable<TEntity> entities, bool commitChanges = true)
        {
            using (TContext ctx = Context)
            {
                foreach (TEntity entity in entities)
                {
                    ctx.Set<TEntity>().Attach(entity);
                    ctx.Entry(entity).State = EntityState.Modified;
                }

                await SaveChangesAsync(ctx);
            }
        }

        /// <summary>
        /// Updates the existing entity.
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <param name="properties">The properties of the entity to update</param>
        /// <returns>The updated entity</returns>
        public virtual async Task<TEntity> UpdateAsync(TEntity entity, params string[] properties)
        {
            using (TContext ctx = Context)
            {
                ctx.Set<TEntity>().Attach(entity);
                DbEntityEntry<TEntity> entry = ctx.Entry(entity);

                foreach (string property in properties)
                {
                    if (entry.Member(property) is DbComplexPropertyEntry)
                    {
                        entry.ComplexProperty(property).IsModified = true;
                    }
                    else
                    {
                        entry.Property(property).IsModified = true;
                    }
                }

                ctx.Entry(entity).State = EntityState.Modified;
                await SaveChangesAsync(ctx);
                return entity;
            }
        }

        /// <summary>
        /// Updates the existing entity.
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <param name="properties">The properties of the entity to update</param>
        /// <returns>The updated entity</returns>
        public virtual async Task<TEntity> UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            using (TContext ctx = Context)
            {
                ctx.Set<TEntity>().Attach(entity);
                DbEntityEntry<TEntity> entry = ctx.Entry(entity);

                foreach (Expression<Func<TEntity, object>> property in properties)
                {
                    entry.Property(property).IsModified = true;
                }

                ctx.Entry(entity).State = EntityState.Modified;

                await SaveChangesAsync(ctx);

                return entity;
            }
        }
    }
}