using System;

#if NET461

using System.Data.Entity;

#else

using Microsoft.EntityFrameworkCore;

#endif

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;

namespace Dime.Repositories
{
    [ExcludeFromCodeCoverage]
    internal static class DbContextExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="ctx"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        internal static int Count<TEntity>(this DbContext ctx, Expression<Func<TEntity, bool>> query = null)
            where TEntity : class
            => query == null
                ? ctx.Set<TEntity>().AsExpandable().AsNoTracking().Count()
                : ctx.Set<TEntity>().AsExpandable().AsNoTracking().Count(query);
    }
}