using System;
using System.Collections.Generic;

#if NET461

using System.Data.Entity;

#else

using Microsoft.EntityFrameworkCore;

#endif

using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
        public virtual async Task DeleteAsync(object? id)
        {
            await using TContext ctx = Context;
            TEntity item = await ctx.Set<TEntity>().FindAsync(id).ConfigureAwait(false);
            if (item != default(TEntity))
            {
                ctx.Set<TEntity>().Remove(item);
                await SaveChangesAsync(ctx).ConfigureAwait(false);
            }
        }

        public virtual async Task DeleteAsync(IEnumerable<object?> ids)
        {
            await using TContext ctx = Context;
            foreach (object id in ids.Distinct().ToList())
            {
                TEntity item = await ctx.Set<TEntity>().FindAsync(id).ConfigureAwait(false);
                if (item == default(TEntity))
                    continue;

                ctx.Set<TEntity>().Remove(item);
            }

            await SaveChangesAsync(ctx).ConfigureAwait(false);
        }

        public virtual async Task DeleteAsync(object? id, bool commit)
        {
            await using TContext ctx = Context;
            TEntity item = await ctx.Set<TEntity>().FindAsync(id).ConfigureAwait(false);
            if (item != default(TEntity))
            {
                ctx.Set<TEntity>().Remove(item);
                if (commit)
                    await SaveChangesAsync(ctx).ConfigureAwait(false);
            }
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            await using TContext ctx = Context;
            ctx.Set<TEntity>().Attach(entity);
            ctx.Set<TEntity>().Remove(entity);
            await SaveChangesAsync(ctx).ConfigureAwait(false);
        }

        public async Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            if (!entities.Any())
                return;

            await using TContext ctx = Context;
            foreach (TEntity entity in entities)
            {
                ctx.Set<TEntity>().Attach(entity);
                ctx.Set<TEntity>().Remove(entity);
            }

            await SaveChangesAsync(ctx).ConfigureAwait(false);
        }

        public virtual async Task DeleteAsync(TEntity entity, bool commit)
        {
            await using TContext ctx = Context;
            ctx.Set<TEntity>().Attach(entity);
            ctx.Set<TEntity>().Remove(entity);

            if (commit)
                await SaveChangesAsync(ctx).ConfigureAwait(false);
        }

        public virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> where)
        {
            await using TContext ctx = Context;
            IEnumerable<TEntity> entities = ctx.Set<TEntity>().With(where).AsNoTracking().ToList();
            if (entities.Any())
            {
                foreach (TEntity item in entities)
                    ctx.Set<TEntity>().Attach(item);

                ctx.Set<TEntity>().RemoveRange(entities);
                await SaveChangesAsync(ctx).ConfigureAwait(false);
            }
        }
    }
}