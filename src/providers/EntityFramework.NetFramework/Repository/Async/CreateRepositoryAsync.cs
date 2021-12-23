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
        /// Save a new item to the data store
        /// </summary>
        /// <param name="entity">The disconnected entity to store</param>
        /// <returns>The connected entity</returns>
        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Added;
            TEntity createdItem = Context.Set<TEntity>().Add(entity);
            await SaveChangesAsync(Context).ConfigureAwait(false);

            return createdItem;
        }

        /// <summary>
        /// Save a new item to the data store
        /// </summary>
        /// <param name="entity">The disconnected entity to store</param>
        /// <param name="condition">The predicate to validate before creating the entity</param>
        /// <returns>The connected entity</returns>
        public virtual async Task<TEntity> CreateAsync(TEntity entity, Expression<Func<TEntity, bool>> condition)
        {
            Context.Entry(entity).State = EntityState.Added;
            TEntity createdItem = Context.Set<TEntity>().AddIfNotExists(entity, condition);
            await SaveChangesAsync(Context).ConfigureAwait(false);

            return createdItem;
        }

        /// <summary>
        /// Save a new item to the data store and provide the chance to execute additional logic before saving
        /// </summary>
        /// <param name="entity">The disconnected entity to store</param>
        /// <param name="beforeSaveAction">The Func to execute before anything is done</param>
        /// <returns>The connected entity</returns>
        public virtual async Task<TEntity> CreateAsync(TEntity entity, Func<TEntity, TContext, Task> beforeSaveAction)
        {
            await beforeSaveAction(entity, Context).ConfigureAwait(false);

            Context.Entry(entity).State = EntityState.Added;
            TEntity createdItem = Context.Set<TEntity>().Add(entity);
            await SaveChangesAsync(Context).ConfigureAwait(false);

            return createdItem;
        }

        /// <summary>
        /// Save a new item to the data store
        /// </summary>
        /// <param name="entity">The disconnected entity to store</param>
        /// <param name="commit">Indicates whether or not SaveChangesAsync should be executed</param>
        /// <returns>The connected entity</returns>
        public virtual async Task<TEntity> CreateAsync(TEntity entity, bool commit)
        {
            Context.Entry(entity).State = EntityState.Added;
            TEntity createdItem = Context.Set<TEntity>().Add(entity);

            if (commit)
                await SaveChangesAsync(Context).ConfigureAwait(false);

            return createdItem;
        }

        /// <summary>
        /// Save new items to the data store
        /// </summary>
        /// <param name="entities">The disconnected entities to store</param>
        /// <returns>The connected entities</returns>
        public virtual async Task<IQueryable<TEntity>> CreateAsync(IQueryable<TEntity> entities)
        {
            if (!entities.Any())
                return entities;

            List<TEntity> newEntities = new();

            foreach (TEntity entity in entities.ToList())
            {
                Context.Entry(entity).State = EntityState.Added;
                TEntity newEntity = Context.Set<TEntity>().Add(entity);
                newEntities.Add(newEntity);
            }

            await SaveChangesAsync(Context).ConfigureAwait(false);

            return newEntities.AsQueryable();
        }
    }
}