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
            using TContext ctx = Context;
            TEntity item = ctx.Set<TEntity>().Find(id);
            if (item == default(TEntity))
                return;

            ctx.Set<TEntity>().Remove(item);
            SaveChanges(Context);
        }

        public virtual void Delete(long id, bool commit)
        {
            using TContext ctx = Context;
            TEntity item = ctx.Set<TEntity>().Find(id);
            if (item == default(TEntity))
                return;

            ctx.Set<TEntity>().Remove(item);
            if (commit)
                SaveChanges(Context);
        }

        public virtual void Delete(TEntity entity)
        {
            using TContext ctx = Context;
            ctx.Set<TEntity>().Attach(entity);
            ctx.Set<TEntity>().Remove(entity);
            SaveChanges(Context);
        }

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

            SaveChanges(Context);
        }

        public virtual void Delete(TEntity entity, bool commit)
        {
            using TContext ctx = Context;
            ctx.Set<TEntity>().Attach(entity);
            ctx.Set<TEntity>().Remove(entity);

            if (commit)
                SaveChanges(Context);
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> where)
        {
            using TContext ctx = Context;
            IEnumerable<TEntity> entities = ctx.Set<TEntity>().With(where).AsNoTracking().ToList();
            if (entities == null)
                return;

            foreach (TEntity item in entities)
                ctx.Set<TEntity>().Attach(item);

            ctx.Set<TEntity>().RemoveRange(entities);
            SaveChanges(Context);
        }
    }
}