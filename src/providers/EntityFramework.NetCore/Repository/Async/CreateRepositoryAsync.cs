using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

#if NET461

using System.Data.Entity;

#else

using Microsoft.EntityFrameworkCore;

#endif

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            await using TContext ctx = Context;
            ctx.Entry(entity).State = EntityState.Added;
            TEntity createdItem = ctx.Set<TEntity>().Add(entity)?.Entity;
            await SaveChangesAsync(ctx);

            return createdItem;
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity, Expression<Func<TEntity, bool>> condition)
        {
            await using TContext ctx = Context;
            ctx.Entry(entity).State = EntityState.Added;
            TEntity createdItem = ctx.Set<TEntity>().AddIfNotExists(entity, condition);
            await SaveChangesAsync(ctx);

            return createdItem;
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity, Func<TEntity, TContext, Task> beforeSaveAction)
        {
            await using TContext ctx = Context;
            await beforeSaveAction(entity, ctx);

            ctx.Entry(entity).State = EntityState.Added;
            TEntity createdItem = ctx.Set<TEntity>().Add(entity)?.Entity;
            await SaveChangesAsync(ctx);

            return createdItem;
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity, bool commit)
        {
            await using TContext ctx = Context;
            ctx.Entry(entity).State = EntityState.Added;
            TEntity createdItem = ctx.Set<TEntity>().Add(entity)?.Entity;

            if (commit)
                await SaveChangesAsync(ctx);

            return createdItem;
        }

        public virtual async Task<IQueryable<TEntity>> CreateAsync(IQueryable<TEntity> entities)
        {
            if (!entities.Any())
                return entities;

            List<TEntity> newEntities = new();
            await using TContext ctx = Context;
            foreach (TEntity entity in entities.ToList())
            {
                ctx.Entry(entity).State = EntityState.Added;
                TEntity newEntity = ctx.Set<TEntity>().Add(entity)?.Entity;
                newEntities.Add(newEntity);
            }

            await SaveChangesAsync(ctx);

            return newEntities.AsQueryable();
        }
    }
}