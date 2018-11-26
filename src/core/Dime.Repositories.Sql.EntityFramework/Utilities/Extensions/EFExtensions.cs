using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Dime.Repositories
{
    internal static class EFExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="query"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        internal static IQueryable<TEntity> Include<TEntity>(this IQueryable<TEntity> query, DbContext context, params string[] includes) where TEntity : class
        {
            // Do a safety check first
            if (includes == null)
                return query;

            List<string> includeList = new List<string>();
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
    }
}