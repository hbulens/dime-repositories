using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Dime.Repositories
{
    internal static partial class QueryFactory
    {
        internal static IQueryable<TSource> With<TSource>(this IQueryable<TSource> source, int? page, int? pageSize, IEnumerable<IOrder<TSource>> orderBy)
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