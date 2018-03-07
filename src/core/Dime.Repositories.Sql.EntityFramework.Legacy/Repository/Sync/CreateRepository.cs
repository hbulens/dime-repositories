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
        public virtual TEntity Create(TEntity entity)
        {
            return this.Create(entity, true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commitChanges"></param>
        /// <returns></returns>
        public virtual TEntity Create(TEntity entity, bool commitChanges)
        {
            using (TContext ctx = this.Context)
            {
                ctx.Entry(entity).State = EntityState.Added;
                TEntity createdEntity = ctx.Set<TEntity>().Add(entity);

                if (commitChanges)
                    this.SaveChanges(ctx);

                return createdEntity;
            }
        }
    }
}