using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Dime.Repositories
{
    internal static partial class QueryFactory
    {
        /// <summary>
        /// Wrapper around LINQ ORDER BY
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="orderByExpression">The order by expression.</param>
        /// <returns></returns>
        internal static IQueryable<TSource> WithOrder<TSource>(this IQueryable<TSource> source, IEnumerable<IOrder<TSource>> orderByExpression)
        {
            if (orderByExpression != null && orderByExpression.Count() > 1)
            {
                IEnumerable<TSource> orderBy = null;
                for (int i = 0; i < orderByExpression.Count(); i++)
                {
                    IOrder<TSource> element = orderByExpression.ElementAt(i);
                    if (i == 0)
                        orderBy = element.IsAscending ?
                            source.Order(element.Property) :
                            source.OrderDescending(element.Property);
                    else
                        orderBy = element.IsAscending ?
                            orderBy.ThenBy(element.Property) :
                            orderBy.ThenByDescending(element.Property);
                }

                return orderBy.AsQueryable();
            }
            else if (orderByExpression != null && orderByExpression.Count() == 1)
                return orderByExpression.ElementAt(0).IsAscending ?
                    source.Order(orderByExpression.ElementAt(0).Property).AsQueryable() :
                    source.OrderDescending(orderByExpression.ElementAt(0).Property).AsQueryable();
            else
                return source.OrderBy(x => true);
        }

        /// <summary>
        /// Wrapper around LINQ ORDER BY
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="orderByExpression">The order by expression.</param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        internal static IQueryable<TSource> WithOrder<TSource>(this IQueryable<TSource> source, IEnumerable<Expression<Func<TSource, object>>> orderByExpression, bool ascending)
        {
            if (orderByExpression == null)
                return source;
            else
            {
                if (orderByExpression.Count() > 1)
                {
                    Func<TSource, dynamic> orderBy = orderByExpression.ElementAt(0).Compile();
                    Func<TSource, dynamic> orderByThen = orderByExpression.ElementAt(1).Compile();

                    return ascending
                        ? source.OrderBy(orderBy).ThenBy(orderByThen).AsQueryable()
                        : source.OrderBy(orderBy).ThenByDescending(orderByThen).AsQueryable();
                }
                else
                {
                    Func<TSource, dynamic> orderBy = orderByExpression.ElementAt(0).Compile();

                    return ascending
                        ? source.OrderBy(orderBy).AsQueryable()
                        : source.OrderByDescending(orderBy).AsQueryable();
                }
            }
        }

        /// <summary>
        /// Wrapper around LINQ ORDER BY
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="orderByExpression">The order by expression.</param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        internal static IQueryable<TSource> WithOrder<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, object>> orderByExpression, bool ascending)
        {
            if (orderByExpression == null)
                return source;
            else
            {
                Func<TSource, dynamic> compiledExpression = orderByExpression.Compile();
                return ascending
                    ? source.OrderBy(compiledExpression).AsQueryable()
                    : source.OrderByDescending(compiledExpression).AsQueryable();
            }
        }

        /// <summary>
        /// Wrapper around LINQ ORDER BY
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="orderByExpression">The order by expression.</param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        internal static IQueryable<TSource> WithOrder<TSource>(this IQueryable<TSource> source, Func<TSource, object> orderByExpression, bool ascending)
        {
            if (orderByExpression == null)
            {
                Func<TSource, object> defaultSorting = x => true;
                return ascending ? source.OrderBy(defaultSorting).AsQueryable() :
                    source.OrderByDescending(defaultSorting).AsQueryable();
            }
            else
                return ascending ? source.OrderBy(orderByExpression).AsQueryable() :
                    source.OrderByDescending(orderByExpression).AsQueryable();
        }
    }
}