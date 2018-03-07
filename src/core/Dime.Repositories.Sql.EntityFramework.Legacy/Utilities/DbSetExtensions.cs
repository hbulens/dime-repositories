using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Dime.Repositories
{
    internal static class DbSetExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="entity"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static T AddIfNotExists<T>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>> predicate = null) where T : class, new()
        {
            bool exists = predicate != null ? dbSet.Any(predicate) : dbSet.Any();
            return !exists ? dbSet.Add(entity) : null;
        }
    }
}