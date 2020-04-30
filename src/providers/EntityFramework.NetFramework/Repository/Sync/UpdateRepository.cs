using System;

#if NET461

using System.Data.Entity;
using System.Data.Entity.Infrastructure;

#else
using Microsoft.EntityFrameworkCore;
#endif

using System.Linq.Expressions;

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
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