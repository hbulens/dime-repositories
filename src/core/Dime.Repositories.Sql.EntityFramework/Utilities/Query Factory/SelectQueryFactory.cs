using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
            if (selector == null)
            {
                return default(IQueryable<TResult>);
            }
            else
            {
                return source.SelectMany(selector);
            }
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
            if (selector == null)
            {
                return default(IQueryable<TResult>);
            }
            else
            {
                return source.Select(selector);
            }
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
            if (selector == null)
            {
                return default(IQueryable<TResult>);
            }
            else
            {
                return source.Select(selector).AsQueryable();
            }
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
            if (selector == null)
            {
                return default(IQueryable<TResult>);
            }
            else
            {
                return source.Select(selector).AsQueryable();
            }
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
            if (selector == null)
            {
                return default(IQueryable<TResult>);
            }
            else
            {
                return source.Select(selector).AsQueryable();
            }
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
            if (selector == null)
            {
                return default(TResult);
            }
            else
            {
                return (new List<TSource>() { source }).AsQueryable().Select(selector).FirstOrDefault();
            }
        }
    }
}