using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Dime.Repositories
{
    [ExcludeFromCodeCoverage]
    internal static class TakeQueryFactory
    {
        internal static IQueryable<TSource> With<TSource>(this IQueryable<TSource> source, int? takeCount)
        {
            int itemsToTake = takeCount.GetValueOrDefault();
            return itemsToTake == 0 ? source : source.Take(() => itemsToTake);
        }
    }

    [ExcludeFromCodeCoverage]
    internal static class PageFactory
    {
        internal static Page<TSource> ToPage<TSource>(this IQueryable<TSource> source, int count)
            => new Page<TSource>(source.ToList(), count);
    }
}