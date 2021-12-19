using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Dime.Repositories
{
    [ExcludeFromCodeCoverage]
    internal static class OrderLinq
    {
        internal static IOrderedQueryable<T> SortBy<T>(this IEnumerable<T> query, Order<T> order)
        {
            string verb = order.IsAscending ? "OrderBy" : "OrderByDescending";
            LinqOrderHelper<T> helper = new(verb, order.Property);
            return helper.GetAsQueryable(query);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        internal static IOrderedQueryable<TSource> ThenBy<TSource>(this IEnumerable<TSource> source, Order<TSource> order)
        {
            string verb = order.IsAscending ? "ThenBy" : "ThenByDescending";
            LinqOrderHelper<TSource> helper = new(verb, order.Property);
            return helper.GetAsQueryable(source);
        }
    }
}