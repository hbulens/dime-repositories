using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Dime.Repositories
{
    [ExcludeFromCodeCoverage]
    internal static class EFExtensions
    {
        private static IEntityType GetEntityType<T>(DbContext context)
            => context.Model.FindEntityType(typeof(T));

        internal static IQueryable<TEntity> Include<TEntity>(this IQueryable<TEntity> query, DbContext context, params string[] includes) where TEntity : class
        {
            if (includes == null)
                return query;

            List<string> includeList = new();
            if (includes.Length != 0)
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

        internal static IQueryable<TResult> IncludeView<TEntity, TResult>(this IQueryable<TResult> query, DbContext context, params string[] includes)
            where TEntity : class
            where TResult : class
        {
            if (includes != null && includes.Length != 0)
                return includes.Where(include => include != null)
                    .Aggregate(query, (current, include) => current.Include(context, include));

            return GetEntityType<TEntity>(context)
                .GetNavigations()
                .Aggregate(query, (current, navigationProperty) => current.Include(context, navigationProperty.Name));
        }

        public static Task<List<TSource>> ToListAsyncSafe<TSource>(this IQueryable<TSource> source)
        {
            ArgumentNullException.ThrowIfNull(source);
            return source is not IAsyncEnumerable<TSource> ? Task.FromResult(source.ToList()) : source.ToListAsync();
        }
    }
}