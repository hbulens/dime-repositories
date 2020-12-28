using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
        /// <summary>
        /// Updates the entities
        /// </summary>
        /// <param name="entity">The entities to update</param>
        /// <returns></returns>
        public TEntity Update(TEntity entity) => Update(entity, true);

        /// <summary>
        /// Updates the existing entity.
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <param name="commitChanges">Indication whether or not the SaveChanges should be called during this call</param>
        /// <returns></returns>
        public virtual TEntity Update(TEntity entity, bool commitChanges = true)
        {
            using TContext ctx = Context;
            ctx.Set<TEntity>().Attach(entity);
            ctx.Entry(entity).State = EntityState.Modified;

            if (commitChanges)
                SaveChanges(ctx);

            return entity;
        }

        /// <summary>
        /// Updates the entities
        /// </summary>
        /// <param name="entities">The entities to update</param>
        /// <param name="commitChanges">Indication whether or not the SaveChanges should be called during this call</param>
        /// <returns></returns>
        public void Update(IEnumerable<TEntity> entities, bool commitChanges = true)
        {
            if (!entities.Any())
                return;

            using TContext ctx = Context;
            foreach (TEntity entity in entities)
            {
                ctx.Set<TEntity>().Attach(entity);
                ctx.Entry(entity).State = EntityState.Modified;
            }

            SaveChanges(ctx);
        }

        /// <summary>
        /// Updates the existing entity.
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <param name="properties">The properties of the entity to update</param>
        /// <returns>The updated entity</returns>
        public virtual TEntity Update(TEntity entity, params string[] properties)
        {
            using TContext ctx = Context;
            ctx.Set<TEntity>().Attach(entity);
            DbEntityEntry<TEntity> entry = ctx.Entry(entity);

            foreach (string property in properties)
            {
                if (entry.Member(property) is DbComplexPropertyEntry)
                    entry.ComplexProperty(property).IsModified = true;
                else
                    entry.Property(property).IsModified = true;
            }

            ctx.Entry(entity).State = EntityState.Modified;
            SaveChanges(ctx);
            return entity;
        }

        /// <summary>
        /// Updates the existing entity.
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <param name="properties">The properties of the entity to update</param>
        /// <returns>The updated entity</returns>
        public virtual TEntity Update(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            using TContext ctx = Context;
            ctx.Set<TEntity>().Attach(entity);
            DbEntityEntry<TEntity> entry = ctx.Entry(entity);

            foreach (Expression<Func<TEntity, object>> property in properties)
                entry.Property(property).IsModified = true;

            ctx.Entry(entity).State = EntityState.Modified;

            SaveChanges(ctx);

            return entity;
        }
    }
}