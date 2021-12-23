using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
        public virtual TEntity Create(TEntity entity)
        {
            using TContext ctx = Context;
            ctx.Entry(entity).State = EntityState.Added;
            TEntity createdItem = ctx.Set<TEntity>().Add(entity);
            SaveChanges(Context);

            return createdItem;
        }

        public virtual TEntity Create(TEntity entity, Expression<Func<TEntity, bool>> condition)
        {
            using TContext ctx = Context;
            ctx.Entry(entity).State = EntityState.Added;
            TEntity createdItem = ctx.Set<TEntity>().AddIfNotExists(entity, condition);
            SaveChanges(Context);

            return createdItem;
        }

        public virtual TEntity Create(TEntity entity, Func<TEntity, TContext, Task> beforeSaveAction)
        {
            beforeSaveAction(entity, Context);

            using TContext ctx = Context;
            ctx.Entry(entity).State = EntityState.Added;
            TEntity createdItem = ctx.Set<TEntity>().Add(entity);
            SaveChanges(Context);

            return createdItem;
        }

        public virtual TEntity Create(TEntity entity, bool commit)
        {
            using TContext ctx = Context;
            ctx.Entry(entity).State = EntityState.Added;
            TEntity createdItem = ctx.Set<TEntity>().Add(entity);

            if (commit)
                SaveChanges(Context);

            return createdItem;
        }

        public virtual IQueryable<TEntity> Create(IQueryable<TEntity> entities)
        {
            if (!entities.Any())
                return entities;

            List<TEntity> newEntities = new();
            List<TEntity> entitiesToCreate = entities.ToList();

            using TContext ctx = Context;
            foreach (TEntity entity in entitiesToCreate)
            {
                ctx.Entry(entity).State = EntityState.Added;
                TEntity newEntity = ctx.Set<TEntity>().Add(entity);
                newEntities.Add(newEntity);
            }

            SaveChanges(Context);

            return newEntities.AsQueryable();
        }
    }
}