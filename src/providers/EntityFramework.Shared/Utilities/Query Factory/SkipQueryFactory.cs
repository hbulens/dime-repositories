using System;
using System.Collections.Generic;

#if NET461

using System.Data.Entity;

#else

using Microsoft.EntityFrameworkCore;

#endif

using System.Linq;
using System.Linq.Expressions;

namespace Dime.Repositories
{
    internal static partial class QueryFactory
    {
        /// <summary>
        /// Bypasses a specified number of elements in a sequence and then returns the remaining elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">An System.Linq.IQueryable`1 to return elements from.</param>
        /// <param name="page">The number of elements to skip before returning the remaining elements.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="orderBy"></param>
        /// <returns>An System.Linq.IQueryable`1 that contains elements that occur after the specified index in the input sequence.</returns>
        internal static IQueryable<TSource> With<TSource>(this IQueryable<TSource> source, int? page, int? pageSize, IEnumerable<Order<TSource>> orderBy)
        {
            int pageToApply = page.GetValueOrDefault();
            int pageSizeToApply = pageSize.GetValueOrDefault();

            if (pageToApply == 0 || pageSizeToApply == 0)
                return source;

            int itemsToSkip = (pageToApply - 1) * pageSizeToApply;

            return orderBy == null
                ? source.OrderBy(x => true).Skip(itemsToSkip)
                : source.Skip(itemsToSkip);
        }

        /// <summary>
        /// Bypasses a specified number of elements in a sequence and then returns the remaining elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">An System.Linq.IQueryable`1 to return elements from.</param>
        /// <param name="page">The number of elements to skip before returning the remaining elements.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="orderBy"></param>
        /// <returns>An System.Linq.IQueryable`1 that contains elements that occur after the specified index in the input sequence.</returns>
        internal static IQueryable<TSource> With<TSource>(this IQueryable<TSource> source, int? page, int? pageSize, Expression<Func<TSource, dynamic>> orderBy)
        {
            int pageToApply = page.GetValueOrDefault();
            int pageSizeToApply = pageSize.GetValueOrDefault();

            if (pageToApply == 0 || pageSizeToApply == 0)
                return source;

            int itemsToSkip = (pageToApply - 1) * pageSizeToApply;

            return orderBy == null ?
                source.OrderBy(x => true).Skip(itemsToSkip) :
                source.Skip(itemsToSkip);
        }

        /// <summary>
        /// Bypasses a specified number of elements in a sequence and then returns the remaining elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">An System.Linq.IQueryable`1 to return elements from.</param>
        /// <param name="page">The number of elements to skip before returning the remaining elements.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="orderBy"></param>
        /// <returns>An System.Linq.IQueryable`1 that contains elements that occur after the specified index in the input sequence.</returns>
        internal static IQueryable<TSource> With<TSource>(this IQueryable<TSource> source, int? page, int? pageSize, IEnumerable<Expression<Func<TSource, object>>> orderBy)
        {
            int pageToApply = page.GetValueOrDefault();
            int pageSizeToApply = pageSize.GetValueOrDefault();

            if (pageToApply == 0 || pageSizeToApply == 0)
                return source;

            int itemsToSkip = (pageToApply - 1) * pageSizeToApply;

            return orderBy == null ?
                source.OrderBy(x => true).Skip(itemsToSkip) :
                source.Skip(itemsToSkip);
        }
    }
}