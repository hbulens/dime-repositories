using System.Data.Entity;

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual TEntity Update(TEntity entity)
        {
            using (TContext ctx = Context)
            {
                ctx.Set<TEntity>().Attach(entity);
                ctx.Entry(entity).State = EntityState.Modified;
                SaveChanges(ctx);

                return entity;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commitChanges"></param>
        /// <returns></returns>
        public virtual TEntity Update(TEntity entity, bool commitChanges = true)
        {
            using (TContext context = Context)
            {
                context.Set<TEntity>().Attach(entity);
                context.Entry(entity).State = EntityState.Modified;

                if (commitChanges)
                {
                    SaveChanges(context);
                }

                return entity;
            }
        }
    }
}