using System.Collections.Generic;

#if NET461

using System.Data.Entity;
using System.Data.Entity.Infrastructure;

#else

using Microsoft.EntityFrameworkCore;

#endif

using System.Linq;

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
    }
}