using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
        /// <summary>
        /// Removes the record from the data store by its identifier
        /// </summary>
        /// <param name="id">The identifier of the entity</param>
        /// <returns>Void</returns>
        public virtual void Delete(long id)
        {
            using TContext ctx = Context;
            TEntity item = ctx.Set<TEntity>().Find(id);
            if (item == default(TEntity))
                return;

            ctx.Set<TEntity>().Remove(item);
            SaveChanges(ctx);
        }

        /// <summary>
        /// Removes the record from the data store by its identifier
        /// </summary>
        /// <param name="id">The identifier of the entity</param>
        /// <param name="commit">Indicates whether or not SaveChanges should be called during this call</param>
        /// <returns>Void</returns>
        public virtual void Delete(long id, bool commit)
        {
            using TContext ctx = Context;
            TEntity item = ctx.Set<TEntity>().Find(id);
            if (item == default(TEntity))
                return;

            ctx.Set<TEntity>().Remove(item);
            if (commit)
                SaveChanges(ctx);
        }

        /// <summary>
        /// Removes the record from the data store
        /// </summary>
        /// <param name="entity">The disconnected entity to remove</param>
        /// <returns>Void</returns>
        public virtual void Delete(TEntity entity)
        {
            using TContext ctx = Context;
            ctx.Set<TEntity>().Attach(entity);
            ctx.Set<TEntity>().Remove(entity);
            SaveChanges(ctx);
        }

        /// <summary>
        /// Removes the records
        /// </summary>
        /// <param name="entities">The disconnected entities to remove</param>
        /// <returns>Void</returns>
        public void Delete(IEnumerable<TEntity> entities)
        {
            if (!entities.Any())
                return;

            using TContext ctx = Context;
            foreach (TEntity entity in entities)
            {
                ctx.Set<TEntity>().Attach(entity);
                ctx.Set<TEntity>().Remove(entity);
            }

            SaveChanges(ctx);
        }

        /// <summary>
        /// Removes the record from the data store
        /// </summary>
        /// <param name="entity">The disconnected entity to remove</param>
        /// <param name="commit">Indicates whether or not SaveChanges should be called during this call</param>
        /// <returns></returns>
        public virtual void Delete(TEntity entity, bool commit)
        {
            using TContext ctx = Context;
            ctx.Set<TEntity>().Attach(entity);
            ctx.Set<TEntity>().Remove(entity);

            if (commit)
                SaveChanges(ctx);
        }

        /// <summary>
        /// Removes the record from the data store
        /// </summary>
        /// <param name="where">The expression to execute against the data store</param>
        /// <returns>Void</returns>
        public virtual void Delete(Expression<Func<TEntity, bool>> where)
        {
            using TContext ctx = Context;
            IEnumerable<TEntity> entities = ctx.Set<TEntity>().With(where).AsNoTracking().ToList();
            if (entities == null)
                return;

            foreach (TEntity item in entities)
                ctx.Set<TEntity>().Attach(item);

            ctx.Set<TEntity>().RemoveRange(entities);
            SaveChanges(ctx);
        }
    }
}