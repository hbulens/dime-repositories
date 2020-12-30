using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Dime.Repositories
{
    [ExcludeFromCodeCoverage]
    internal static class EFExtensions
    {
        private static IEntityType GetEntityType<T>(DbContext context)
            => context.Model.FindEntityType(typeof(T));

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="context"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        internal static IQueryable<TEntity> Include<TEntity>(this IQueryable<TEntity> query, DbContext context, params string[] includes) where TEntity : class
        {
            // Do a safety check first
            if (includes == null)
                return query;

            List<string> includeList = new();
            if (includes.Any())
                return includes
                    .Where(x => !string.IsNullOrEmpty(x) && !includeList.Contains(x))
                    .Aggregate(query, (current, include) => current.Include(include));

            IEnumerable<INavigation> navigationProperties = context.Model.FindEntityType(typeof(TEntity)).GetNavigations();
            if (navigationProperties == null)
                return query;

            foreach (INavigation navigationProperty in navigationProperties)
            {
                if (includeList.Contains(navigationProperty.Name))
                    continue;

                includeList.Add(navigationProperty.Name);
                query = query.Include(navigationProperty.Name);
            }

            return query;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="includes"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        internal static IQueryable<TResult> IncludeView<TEntity, TResult>(this IQueryable<TResult> query, DbContext context, params string[] includes)
            where TEntity : class
            where TResult : class
        {
            if (includes != null && includes.Any())
                return includes.Where(include => include != null)
                    .Aggregate(query, (current, include) => Include(current, context, include));

            return GetEntityType<TEntity>(context)
                .GetNavigations()
                .Aggregate(query, (current, navigationProperty) => Include(current, context, navigationProperty.Name));
        }
    }
}