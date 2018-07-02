using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;

namespace Dime.Repositories
{
    internal static partial class QueryFactory
    {
        /// <summary>
        /// Withes the specified selector.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        /// <history>
        /// [HB] 17/08/2015 - Create
        /// </history>
        internal static IQueryable<TResult> WithSelect<TSource, TResult, TKey>(this IQueryable<IGrouping<TKey, TSource>> source, Expression<Func<IGrouping<TKey, TSource>, IEnumerable<TResult>>> selector)
        {
            return selector == null ? default(IQueryable<TResult>) : source.SelectMany(selector.Expand());
        }

        /// <summary>
        /// Withes the specified selector.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        /// <history>
        /// [HB] 17/08/2015 - Create
        /// </history>
        internal static IQueryable<TResult> WithSelect<TSource, TResult>(this IQueryable<IGrouping<object, TSource>> source, Expression<Func<IGrouping<object, TSource>, int, TResult>> selector)
        {
            return selector == null ? default(IQueryable<TResult>) : source.Select(selector.Expand());
        }

        /// <summary>
        /// Withes the specified selector.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        /// <history>
        /// [HB] 17/08/2015 - Create
        /// </history>
        internal static IQueryable<TResult> WithSelect<TSource, TResult>(this IOrderedEnumerable<TSource> source, Func<TSource, TResult> selector)
            where TSource : class
            where TResult : class
        {
            return selector == null ? default(IQueryable<TResult>) : source.Select(selector).AsQueryable();
        }

        /// <summary>
        /// Withes the specified selector.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        /// <history>
        /// [HB] 17/08/2015 - Create
        /// </history>
        internal static IQueryable<TResult> WithSelect<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
            where TSource : class
        {
            return selector == null ? default(IQueryable<TResult>) : source.Select(selector.Expand()).AsQueryable();
        }

        /// <summary>
        /// Withes the specified selector.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        /// <history>
        /// [HB] 17/08/2015 - Create
        /// </history>
        internal static IQueryable<TResult> WithSelect<TSource, TResult>(this IQueryable<TSource> source, Func<TSource, TResult> selector)
            where TSource : class
            where TResult : class
        {
            return selector == null ? default(IQueryable<TResult>) : source.Select(selector).AsQueryable();
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        internal static TResult WithFirstSelect<TSource, TResult>(this TSource source, Expression<Func<TSource, TResult>> selector)
            where TSource : class
            where TResult : class
        {
            return selector == null ? default(TResult) : new List<TSource>() { source }.AsQueryable().Select(selector.Expand()).FirstOrDefault();
        }
    }
}