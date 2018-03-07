using System.Linq;

namespace Dime.Repositories
{
    internal static partial class QueryFactory
    {
        /// <summary>
        /// Wrapper around LINQ SKIP
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        /// <history>
        /// [HB] 17/08/2015 - Create
        /// </history>
        internal static IQueryable<TSource> With<TSource>(this IQueryable<TSource> source, int? page, int? pageSize)
        {
            if ((page ?? 0) == 0 || (pageSize ?? 0) == 0)
            {
                return source;
            }
            else
            {
                return source.Skip(((int)page - 1) * (int)pageSize);
            }
        }
    }
}