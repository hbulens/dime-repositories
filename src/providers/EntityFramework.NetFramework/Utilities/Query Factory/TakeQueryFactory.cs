using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Dime.Repositories
{
    [ExcludeFromCodeCoverage]
    internal static class TakeQueryFactory
    {
        /// <summary>
        /// Returns a specified number of contiguous elements from the start of a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The sequence to return elements from.</param>
        /// <param name="takeCount">The number of elements to return.</param>
        /// <returns>An System.Linq.IQueryable`1 that contains the specified number of elements from the start of source.</returns>
        internal static IQueryable<TSource> With<TSource>(this IQueryable<TSource> source, int? takeCount)
        {
            int itemsToTake = takeCount.GetValueOrDefault();
            return itemsToTake == 0 ? source : source.Take(() => itemsToTake);
        }
    }
}