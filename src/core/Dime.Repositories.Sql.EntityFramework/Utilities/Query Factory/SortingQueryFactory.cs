using Dime.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Dime.Repositories
{
    internal static partial class QueryFactory
    {
        /// <summary>
        /// Wrapper around LINQ ORDER BY
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="orderByExpression">The order by expression.</param>
        /// <returns></returns>
        /// <history>
        /// [HB] 17/08/2015 - Create
        /// </history>
        internal static IQueryable<TSource> WithOrder<TSource>(this IQueryable<TSource> source, IEnumerable<IOrder<TSource>> orderByExpression)
        {
            if (orderByExpression != null && orderByExpression.Count() > 1)
            {
                IEnumerable<TSource> orderBy = null;
                for (int i = 0; i < orderByExpression.Count(); i++)
                {
                    IOrder<TSource> element = orderByExpression.ElementAt(i);
                    if (i == 0)
                    {
                        orderBy = element.IsAscending ?
                            source.Order(element.Property) :
                            source.OrderDescending(element.Property);
                    }
                    else
                    {
                        orderBy = element.IsAscending ?
                            orderBy.ThenBy(element.Property) :
                            orderBy.ThenByDescending(element.Property);
                    }
                }

                return orderBy.AsQueryable();
            }
            else if (orderByExpression != null && orderByExpression.Count() == 1)
            {
                return orderByExpression.ElementAt(0).IsAscending ?
                    source.Order(orderByExpression.ElementAt(0).Property).AsQueryable() :
                    source.OrderDescending(orderByExpression.ElementAt(0).Property).AsQueryable();
            }
            else
            {
                bool isComparer = typeof(IComparer<TSource>).IsAssignableFrom(typeof(TSource));
                bool isComparable = typeof(IComparable).IsAssignableFrom(typeof(TSource)) || typeof(IComparable<TSource>).IsAssignableFrom(typeof(TSource));

                return source.OrderBy(x => true);
            }
        }

        /// <summary>
        /// Wrapper around LINQ ORDER BY
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="orderByExpression">The order by expression.</param>
        /// <returns></returns>
        /// <history>
        /// [HB] 17/08/2015 - Create
        /// </history>
        internal static IQueryable<TSource> WithOrder<TSource>(this IQueryable<TSource> source, IEnumerable<Expression<Func<TSource, object>>> orderByExpression, bool ascending)
        {
            if (orderByExpression == null)
            {
                bool isComparer = typeof(IComparer<TSource>).IsAssignableFrom(typeof(TSource));
                bool isComparable = typeof(IComparable).IsAssignableFrom(typeof(TSource)) || typeof(IComparable<TSource>).IsAssignableFrom(typeof(TSource));

                return ascending ? source.OrderBy(x => true) : source.OrderByDescending(x => true);
            }
            else
            {
                if (orderByExpression.Count() > 1)
                {
                    Func<TSource, dynamic> orderBy = orderByExpression.ElementAt(0).Compile();
                    Func<TSource, dynamic> orderByThen = orderByExpression.ElementAt(1).Compile();

                    return ascending ?
                    source.OrderBy(orderBy).ThenBy(orderByThen).AsQueryable() :
                    source.OrderBy(orderBy).ThenByDescending(orderByThen).AsQueryable();
                }
                else
                {
                    Func<TSource, dynamic> orderBy = orderByExpression.ElementAt(0).Compile();

                    return ascending ?
                    source.OrderBy(orderBy).AsQueryable() :
                    source.OrderByDescending(orderBy).AsQueryable();
                }
            }
        }

        /// <summary>
        /// Wrapper around LINQ ORDER BY
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="orderByExpression">The order by expression.</param>
        /// <returns></returns>
        /// <history>
        /// [HB] 17/08/2015 - Create
        /// </history>
        internal static IQueryable<TSource> WithOrder<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, object>> orderByExpression, bool ascending)
        {
            if (orderByExpression == null)
            {
                bool isComparer = typeof(IComparer<TSource>).IsAssignableFrom(typeof(TSource));
                bool isComparable = typeof(IComparable).IsAssignableFrom(typeof(TSource)) || typeof(IComparable<TSource>).IsAssignableFrom(typeof(TSource));

                return ascending ? source.OrderBy(x => true) : source.OrderByDescending(x => true);
            }
            else
            {
                Func<TSource, dynamic> compiledExpression = orderByExpression.Compile();
                return ascending ?
                    source.OrderBy(compiledExpression).AsQueryable() :
                    source.OrderByDescending(compiledExpression).AsQueryable();
            }
        }

        /// <summary>
        /// Wrapper around LINQ ORDER BY
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="orderByExpression">The order by expression.</param>
        /// <returns></returns>
        /// <history>
        /// [HB] 17/08/2015 - Create
        /// </history>
        internal static IQueryable<TSource> WithOrder<TSource>(this IQueryable<TSource> source, Func<TSource, object> orderByExpression, bool ascending)
        {
            if (orderByExpression == null)
            {
                bool isComparer = typeof(IComparer<TSource>).IsAssignableFrom(typeof(TSource));
                bool isComparable = typeof(IComparable).IsAssignableFrom(typeof(TSource)) || typeof(IComparable<TSource>).IsAssignableFrom(typeof(TSource));

                Func<TSource, object> defaultSorting = x => true;
                return ascending ? source.OrderBy(defaultSorting).AsQueryable() : source.OrderByDescending(defaultSorting).AsQueryable();
            }
            else
            {
                return ascending ? source.OrderBy(orderByExpression).AsQueryable() : source.OrderByDescending(orderByExpression).AsQueryable();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="property"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }
    }
}