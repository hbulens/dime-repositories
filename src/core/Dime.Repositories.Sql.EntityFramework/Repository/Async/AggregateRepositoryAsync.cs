﻿using Microsoft.EntityFrameworkCore;
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
        /// <history>
        /// [HB] 25/08/2015 - Create
        /// [HB] 09/02/2017 - Review and document
        /// </history>
        public async Task<long> CountAsync()
        {
            using (TContext ctx = this.Context)
            {
                return await ctx.Set<TEntity>().CountAsync();
            }
        }

        /// <summary>
        /// Counts the amount of records in the data store for the table that corresponds to the entity type <typeparamref name="TEntity"/>.
        /// </summary>
        /// <returns>A number of the amount of records</returns>
        /// <param name="where">The expression to execute against the data store</param>
        /// <history>
        /// [HB] 25/08/2015 - Create
        /// [HB] 09/02/2017 - Review and document
        /// </history>
        public async Task<long> CountAsync(Expression<Func<TEntity, bool>> where)
        {
            using (TContext ctx = this.Context)
            {
                return await ctx.Set<TEntity>().With(where).CountAsync();
            }
        }
    }
}