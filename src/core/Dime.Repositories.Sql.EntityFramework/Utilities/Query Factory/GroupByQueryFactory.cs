using System;
using System.Linq;
using System.Linq.Expressions;

namespace Dime.Repositories
{
    internal static partial class QueryFactory
    {
        /// <summary>
        /// Wrapper around LINQ WHERE
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static IQueryable<IGrouping<TKey, TSource>> WithGroup<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> predicate)
            => source.GroupBy(predicate);

        /// <summary>
        /// Wrapper around LINQ WHERE
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static IQueryable<IGrouping<TKey, TSource>> WithGroup<TSource, TKey>(this IQueryable<TSource> source, Func<TSource, TKey> predicate)
            => source.GroupBy(predicate).AsQueryable();

        /// <summary>
        /// Wrapper around LINQ WHERE
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static IQueryable<IGrouping<TKey, TSource>> WithGroup<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> predicate)
            => source.GroupBy(predicate).AsQueryable();
    }
}