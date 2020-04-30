using System;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace Dime.Repositories
{
    [ExcludeFromCodeCoverage]
    internal static class DbSetExtensions
    {
        /// <summary>
        /// Conditionally adds the item to the set
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="dbSet">The entity's set</param>
        /// <param name="entity">The record to add</param>
        /// <param name="condition">The condition</param>
        /// <returns>The created item if the condition was met; otherwise null</returns>
        public static T AddIfNotExists<T>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>> condition = null) where T : class, new()
            => !(condition == null || dbSet.Any(condition)) ? dbSet.Add(entity) : null;
    }
}