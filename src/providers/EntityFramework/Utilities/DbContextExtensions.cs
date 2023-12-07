using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Dime.Repositories
{
    [ExcludeFromCodeCoverage]
    internal static class DbContextExtensions
    {
        internal static int Count<TEntity>(this DbContext ctx, Expression<Func<TEntity, bool>> query = null)
            where TEntity : class
            => query == null
                ? ctx.Set<TEntity>().AsExpandable().AsNoTracking().Count()
                : ctx.Set<TEntity>().AsExpandable().AsNoTracking().Count(query);

        internal static Task<int> CountAsync<TEntity>(this DbContext ctx, Expression<Func<TEntity, bool>> query = null)
            where TEntity : class
            => query == null
                ? ctx.Set<TEntity>().AsExpandable().AsNoTracking().CountAsync()
                : ctx.Set<TEntity>().AsExpandable().AsNoTracking().CountAsync(query);
    }
}