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
        /// <param name="source">The source.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        /// <history>
        /// [HB] 17/08/2015 - Create
        /// </history>
        public static IQueryable<IGrouping<TKey, TSource>> WithGroup<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> predicate)
        {
            return source.GroupBy(predicate).AsQueryable();
        }

        /// <summary>
        /// Wrapper around LINQ WHERE
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        /// <history>
        /// [HB] 17/08/2015 - Create
        /// </history>
        public static IQueryable<IGrouping<TKey, TSource>> WithGroup<TSource, TKey>(this IQueryable<TSource> source, Func<TSource, TKey> predicate)
        {
            return source.GroupBy(predicate).AsQueryable();
        }

        /// <summary>
        /// Wrapper around LINQ WHERE
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        /// <history>
        /// [HB] 17/08/2015 - Create
        /// </history>
        public static IQueryable<IGrouping<TKey, TSource>> WithGroup<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> predicate)
        {
            return source.GroupBy(predicate).AsQueryable();
        }
    }
}