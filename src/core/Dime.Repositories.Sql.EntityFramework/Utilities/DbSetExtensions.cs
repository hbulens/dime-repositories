using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Dime.Repositories
{
    internal static class DbSetExtensions
    {
        /// <summary>
        /// Conditionally adds the item to the set
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="dbSet">The entity's set</param>
        /// <param name="entity">The record to add</param>
        /// <param name="predicate">The condition</param>
        /// <returns>The created item if the condition was met; otherwise null</returns>
        public static T AddIfNotExists<T>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>> predicate = null) where T : class, new()
                  => !(predicate == null || dbSet.Any(predicate)) ? dbSet.Add(entity).Entity : null;
    }
}