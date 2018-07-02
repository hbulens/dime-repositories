using System.Linq;

namespace Dime.Repositories
{
    internal static partial class TakeQueryFactory
    {
        /// <summary>
        /// Wrapper around LINQ TAKE
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="takeCount">The count.</param>
        /// <returns></returns>
        /// <history>
        /// [HB] 17/08/2015 - Create
        /// </history>
        internal static IQueryable<TSource> With<TSource>(this IQueryable<TSource> source, int? takeCount)
        {
            return (takeCount ?? 0) == 0 ? source : source.Take((int)takeCount);
        }
    }
}