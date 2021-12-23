using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
        /// <summary>
        /// Counts the amount of records in the data store for the table that corresponds to the entity type <typeparamref name="TEntity"/>.
        /// </summary>
        /// <returns>A number of the amount of records</returns>
        public Task<long> CountAsync()
        {
            long count = Context.Count<TEntity>();
            return Task.FromResult(count);
        }

        /// <summary>
        /// Counts the amount of records in the data store for the table that corresponds to the entity type <typeparamref name="TEntity"/>.
        /// </summary>
        /// <returns>A number of the amount of records</returns>
        /// <param name="where">The expression to execute against the data store</param>
        public Task<long> CountAsync(Expression<Func<TEntity, bool>> where)
        {
            long count = Context.Count(where);
            return Task.FromResult(count);
        }
    }
}