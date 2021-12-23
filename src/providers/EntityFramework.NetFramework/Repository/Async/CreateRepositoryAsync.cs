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
        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            using TContext ctx = Context;
            ctx.Entry(entity).State = EntityState.Added;
            TEntity createdItem = ctx.Set<TEntity>().Add(entity);
            await SaveChangesAsync(ctx).ConfigureAwait(false);

            return createdItem;
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity, Expression<Func<TEntity, bool>> condition)
        {
            using TContext ctx = Context;
            ctx.Entry(entity).State = EntityState.Added;
            TEntity createdItem = ctx.Set<TEntity>().AddIfNotExists(entity, condition);
            await SaveChangesAsync(ctx).ConfigureAwait(false);

            return createdItem;
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity, Func<TEntity, TContext, Task> beforeSaveAction)
        {
            using TContext ctx = Context;

            await beforeSaveAction(entity, ctx).ConfigureAwait(false);

            ctx.Entry(entity).State = EntityState.Added;
            TEntity createdItem = ctx.Set<TEntity>().Add(entity);
            await SaveChangesAsync(ctx).ConfigureAwait(false);

            return createdItem;
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity, bool commit)
        {
            using TContext ctx = Context;
            ctx.Entry(entity).State = EntityState.Added;
            TEntity createdItem = ctx.Set<TEntity>().Add(entity);

            if (commit)
                await SaveChangesAsync(ctx).ConfigureAwait(false);

            return createdItem;
        }

        public virtual async Task<IQueryable<TEntity>> CreateAsync(IQueryable<TEntity> entities)
        {
            if (!entities.Any())
                return entities;

            List<TEntity> newEntities = new();

            using TContext ctx = Context;
            foreach (TEntity entity in entities.ToList())
            {
                ctx.Entry(entity).State = EntityState.Added;
                TEntity newEntity = ctx.Set<TEntity>().Add(entity);
                newEntities.Add(newEntity);
            }

            await SaveChangesAsync(ctx).ConfigureAwait(false);

            return newEntities.AsQueryable();
        }
    }
}