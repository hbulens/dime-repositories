using System;
using System.Data.Entity;
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
        /// <param name="Context"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        internal static int Count<TEntity>(this DbContext Context, Expression<Func<TEntity, bool>> query = null)
            where TEntity : class
            => query == null
                ? Context.Set<TEntity>().AsExpandable().AsNoTracking().Count()
                : Context.Set<TEntity>().AsExpandable().AsNoTracking().Count(query);
    }
}