using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
        public virtual void Delete(long id)
        {
            TEntity item = Context.Set<TEntity>().Find(id);
            if (item == default(TEntity))
                return;

            Context.Set<TEntity>().Remove(item);
            SaveChanges(Context);
        }

        public virtual void Delete(long id, bool commit)
        {
            TEntity item = Context.Set<TEntity>().Find(id);
            if (item == default(TEntity))
                return;

            Context.Set<TEntity>().Remove(item);
            if (commit)
                SaveChanges(Context);
        }

        public virtual void Delete(TEntity entity)
        {
            Context.Set<TEntity>().Attach(entity);
            Context.Set<TEntity>().Remove(entity);
            SaveChanges(Context);
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            if (!entities.Any())
                return;


            foreach (TEntity entity in entities)
            {
                Context.Set<TEntity>().Attach(entity);
                Context.Set<TEntity>().Remove(entity);
            }

            SaveChanges(Context);
        }

        public virtual void Delete(TEntity entity, bool commit)
        {
            Context.Set<TEntity>().Attach(entity);
            Context.Set<TEntity>().Remove(entity);

            if (commit)
                SaveChanges(Context);
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> where)
        {
            IEnumerable<TEntity> entities = Context.Set<TEntity>().With(where).AsNoTracking().ToList();
            if (entities == null)
                return;

            foreach (TEntity item in entities)
                Context.Set<TEntity>().Attach(item);

            Context.Set<TEntity>().RemoveRange(entities);
            SaveChanges(Context);
        }
    }
}