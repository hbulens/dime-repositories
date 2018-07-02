using System;
using System.Linq;
using System.Linq.Expressions;

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
        /// <summary>
        /// Counts the amount of records in the data store for the table that corresponds to the entity type <typeparamref name="TEntity"/>.
        /// </summary>
        /// <returns>A number of the amount of records</returns>
        public long Count()
        {
            using (TContext ctx = Context)
                return ctx.Set<TEntity>().Count();
        }

        /// <summary>
        /// Counts the amount of records in the data store for the table that corresponds to the entity type <typeparamref name="TEntity"/>.
        /// </summary>
        /// <returns>A number of the amount of records</returns>
        /// <param name="where">The expression to execute against the data store</param>
        public long Count(Expression<Func<TEntity, bool>> where)
        {
            using (TContext ctx = Context)
                return ctx.Set<TEntity>().Count(where);
        }
    }
}