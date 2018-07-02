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
        ///
        /// </summary>
        /// <param name="id"></param>
        public virtual void Delete(long id)
        {
            using (TContext context = Context)
            {
                TEntity item = context.Set<TEntity>().Find(id);
                context.Set<TEntity>().Remove(item);
                SaveChanges(context);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(TEntity entity)
        {
            using (TContext context = Context)
            {
                if (context.Entry(entity).State == EntityState.Detached)
                    context.Set<TEntity>().Attach(entity);

                context.Set<TEntity>().Remove(entity);
                SaveChanges(context);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        public virtual void Delete(Expression<Func<TEntity, bool>> where)
        {
            using (TContext context = Context)
            {
                IEnumerable<TEntity> objects = context.Set<TEntity>().Where(where).AsNoTracking().AsEnumerable();
                foreach (TEntity item in objects)
                {
                    context.Set<TEntity>().Remove(item);
                }

                SaveChanges(context);
            }
        }
    }
}