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
        public virtual TEntity Create(TEntity entity)
        {
            using TContext ctx = Context;
            ctx.Entry(entity).State = EntityState.Added;
            TEntity createdItem = ctx.Set<TEntity>().Add(entity);
            SaveChanges(ctx);

            return createdItem;
        }

        /// <summary>
        /// Save a new item to the data store
        /// </summary>
        /// <param name="entity">The disconnected entity to store</param>
        /// <param name="condition">The predicate to validate before creating the entity</param>
        /// <returns>The connected entity</returns>
        public virtual TEntity Create(TEntity entity, Expression<Func<TEntity, bool>> condition)
        {
            using TContext ctx = Context;
            ctx.Entry(entity).State = EntityState.Added;
            TEntity createdItem = ctx.Set<TEntity>().AddIfNotExists(entity, condition);
            SaveChanges(ctx);

            return createdItem;
        }

        /// <summary>
        /// Save a new item to the data store and provide the chance to execute additional logic before saving
        /// </summary>
        /// <param name="entity">The disconnected entity to store</param>
        /// <param name="beforeSaveAction">The Func to execute before anything is done</param>
        /// <returns>The connected entity</returns>
        public virtual TEntity Create(TEntity entity, Func<TEntity, TContext, Task> beforeSaveAction)
        {
            using TContext ctx = Context;
            beforeSaveAction(entity, ctx);

            ctx.Entry(entity).State = EntityState.Added;
            TEntity createdItem = ctx.Set<TEntity>().Add(entity);
            SaveChanges(ctx);

            return createdItem;
        }

        /// <summary>
        /// Save a new item to the data store
        /// </summary>
        /// <param name="entity">The disconnected entity to store</param>
        /// <param name="commit">Indicates whether or not SaveChanges should be executed</param>
        /// <returns>The connected entity</returns>
        public virtual TEntity Create(TEntity entity, bool commit)
        {
            using TContext ctx = Context;
            ctx.Entry(entity).State = EntityState.Added;
            TEntity createdItem = ctx.Set<TEntity>().Add(entity);

            if (commit)
                SaveChanges(ctx);

            return createdItem;
        }

        /// <summary>
        /// Save new items to the data store
        /// </summary>
        /// <param name="entities">The disconnected entities to store</param>
        /// <returns>The connected entities</returns>
        public virtual IQueryable<TEntity> Create(IQueryable<TEntity> entities)
        {
            if (!entities.Any())
                return entities;

            List<TEntity> newEntities = new List<TEntity>();
            List<TEntity> entitiesToCreate = entities.ToList();
            using TContext ctx = Context;
            foreach (TEntity entity in entitiesToCreate)
            {
                ctx.Entry(entity).State = EntityState.Added;
                TEntity newEntity = ctx.Set<TEntity>().Add(entity);
                newEntities.Add(newEntity);
            }

            SaveChanges(ctx);

            return newEntities.AsQueryable();
        }
    }
}