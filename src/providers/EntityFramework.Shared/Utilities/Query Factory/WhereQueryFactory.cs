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
        internal static IQueryable<TSource> With<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
            => predicate == null ? source : source.Where(predicate);

        /// <summary>
        /// Wrapper around LINQ WHERE
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        internal static TSource WithFirst<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
            => predicate == null ? source.FirstOrDefault() : source.FirstOrDefault(predicate);
    }
}