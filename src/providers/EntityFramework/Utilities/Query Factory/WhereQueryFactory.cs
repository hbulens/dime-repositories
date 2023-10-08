using System;
using System.Linq;
using System.Linq.Expressions;

namespace Dime.Repositories
{
    internal static partial class QueryFactory
    {
        internal static IQueryable<TSource> With<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
            => predicate == null ? source : source.Where(predicate);

        internal static TSource WithFirst<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
            => predicate == null ? source.FirstOrDefault() : source.FirstOrDefault(predicate);
    }
}