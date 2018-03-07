using Microsoft.EntityFrameworkCore;

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
            using (TContext ctx = this.Context)
            {
                ctx.Set<TEntity>().Attach(entity);
                ctx.Entry(entity).State = EntityState.Modified;
                this.SaveChanges(ctx);

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
            using (TContext context = this.Context)
            {
                context.Set<TEntity>().Attach(entity);
                context.Entry(entity).State = EntityState.Modified;

                if (commitChanges)
                {
                    this.SaveChanges(context);
                }

                return entity;
            }
        }
    }
}