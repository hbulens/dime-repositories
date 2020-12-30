using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Dime.Repositories
{
    [ExcludeFromCodeCoverage]
    public static class OrderLinq
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        [Obsolete("In a next release this will be internal. Fetch a utility library instead.", false)]
        public static IOrderedQueryable<T> OrderDescending<T>(this IEnumerable<T> query, string propertyName)
        {
            LinqOrderHelper<T> helper = new("OrderByDescending", propertyName);
            return helper.GetAsQueryable(query);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        [Obsolete("In a next release this will be internal. Fetch a utility library instead.", false)]
        public static IOrderedQueryable<T> Order<T>(this IEnumerable<T> query, string propertyName)
        {
            LinqOrderHelper<T> helper = new("OrderBy", propertyName);
            return helper.GetAsQueryable(query);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        [Obsolete("In a next release this will be internal. Fetch a utility library instead.", false)]
        public static IOrderedQueryable<TSource> ThenBy<TSource>(this IEnumerable<TSource> source, string property)
        {
            LinqOrderHelper<TSource> helper = new("ThenBy", property);
            return helper.GetAsQueryable(source);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        [Obsolete("In a next release this will be internal. Fetch a utility library instead.", false)]
        public static IOrderedQueryable<TSource> ThenByDescending<TSource>(this IEnumerable<TSource> source, string property)
        {
            LinqOrderHelper<TSource> helper = new("ThenByDescending", property);
            return helper.GetAsQueryable(source);
        }
    }
}