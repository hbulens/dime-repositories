using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

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
            int count;
            using (TContext ctx = Context)
                count = ctx.Set<TEntity>().AsNoTracking().Count();

            return count;
        }

        /// <summary>
        /// Counts the amount of records in the data store for the table that corresponds to the entity type <typeparamref name="TEntity"/>.
        /// </summary>
        /// <returns>A number of the amount of records</returns>
        /// <param name="where">The expression to execute against the data store</param>
        public long Count(Expression<Func<TEntity, bool>> where)
        {
            int count;
            using (TContext ctx = Context)
                count = ctx.Set<TEntity>().AsNoTracking().Count(where);

            return count;
        }
    }
}