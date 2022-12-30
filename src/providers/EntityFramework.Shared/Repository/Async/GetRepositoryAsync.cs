using System;
using System.Collections.Generic;

#if NET461

using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;

#else

using Microsoft.EntityFrameworkCore;

#endif

using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LinqKit;

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
        /// <summary>
        /// Checks if there is any record that matches the query
        /// </summary>
        /// <param name="where">The query to execute in the Any method</param>
        /// <returns>True if there is at least one record</returns>
        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> where)
        {
            await using TContext ctx = Context;
            return await ctx.Set<TEntity>().AsNoTracking().AnyAsync(where);
        }

        /// <summary>
        /// Gets the record by its identifier
        /// </summary>
        /// <param name="id">The identifier of the entity</param>
        /// <returns>The record of type <typeparamref name="TEntity"/> that matches the id</returns>
        public virtual async Task<TEntity> FindByIdAsync(object? id)
        {
            await using TContext ctx = Context;
            return await ctx.Set<TEntity>().FindAsync(id);
        }

        /// <summary>
        /// Gets the record by its identifier
        /// </summary>
        /// <param name="id">The identifier of the entity</param>
        /// <param name="includes">The optional list of related entities that should be eagerly loaded</param>
        /// <returns>The record of type <typeparamref name="TEntity"/> that matches the id</returns>
        public virtual async Task<TEntity> FindByIdAsync(object? id, params string[] includes)
        {
            await using TContext ctx = Context;
            foreach (string include in includes)
                ctx.Set<TEntity>().Include(include).AsNoTracking();

            return await ctx.Set<TEntity>().FindAsync(id);
        }

        /// <summary>
        /// Gets the first record from the data store that matches the <paramref name="where"/> parameter
        /// </summary>
        /// <param name="where">The expression to execute against the data store</param>
        /// <returns>The first record of type <typeparamref name="TEntity"/> that matches the query</returns>
        public virtual async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> where)
        {
            await using TContext ctx = Context;
            TEntity query = ctx.Set<TEntity>()
                .AsNoTracking()
                .AsExpandable()
                .With(where)
                .FirstOrDefault();

            return await Task.Run(() => query);
        }

        /// <summary>
        /// Gets the first record from the data store that matches the <paramref name="where"/> parameter
        /// </summary>
        /// <param name="where">The expression to execute against the data store</param>
        /// <param name="includes">The optional list of related entities that should be eagerly loaded</param>
        /// <returns>The first record of type <typeparamref name="TEntity"/> that matches the query</returns>
        public virtual async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> where, params string[] includes)
        {
            await using TContext ctx = Context;
            IQueryable<TEntity> query = ctx.Set<TEntity>()
                .Include(ctx, includes)
                .AsNoTracking()
                .AsExpandable()
                .With(where);

            IQueryable<TEntity> fullGraphQuery = await Task.Run(() => query);
            return fullGraphQuery.FirstOrDefault();
        }

        /// <summary>
        /// Gets the first record from the data store that matches the <paramref name="where"/> parameter
        /// </summary>
        /// <typeparam name="TResult">The projected class</typeparam>
        /// <param name="where">The expression to execute against the data store</param>
        /// <param name="select">The expression for the projection of type <typeparamref name="TResult"/> that should be executed against the data store</param>
        /// <param name="orderBy">The sorting expression to execute against the data store</param>
        /// <param name="ascending">Indicates whether the sorting is ascending (true) or descending (false)</param>
        /// <param name="page">The page number which is multiplied by the pagesize to calculate the amount of items to skip</param>
        /// <param name="pageSize">The size of the batch of items that must be retrieved</param>
        /// <param name="includes">The optional list of related entities that should be eagerly loaded</param>
        /// <returns>An instance of <typeparamref name="TResult"/> with the mapped data from the record that matched all filters.</returns>
        public Task<TResult> FindOneAsync<TResult>(
            Expression<Func<TEntity, bool>> where = null,
            Expression<Func<TEntity, TResult>> select = null,
            Expression<Func<TEntity, dynamic>> orderBy = null,
            bool? ascending = default,
            int? page = default,
            int? pageSize = default,
            params string[] includes) where TResult : class
        {
            using TContext ctx = Context;
            IQueryable<TResult> query = ctx.Set<TEntity>()
                .AsExpandable()
                .AsQueryable()
                .AsNoTracking()
                .With(where)
                .WithOrder(orderBy, ascending ?? true)
                .With(page, pageSize, orderBy)
                .With(pageSize)
                .WithSelect(select)
                .Include(Context, includes);

            return Task.FromResult(query.FirstOrDefault());
        }

        /// <summary>
        /// Finds entities based on provided criteria.
        /// </summary>
        /// <param name="where">The expression to execute against the data store</param>
        /// <param name="includes">The optional list of related entities that should be eagerly loaded</param>
        /// <returns>An collection of <typeparamref name="TEntity"/> that matched all filters.</returns>
        public virtual async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> where, params string[] includes)
        {
            await using TContext ctx = Context;
            IQueryable<TEntity> query = ctx.Set<TEntity>()
                .Include(ctx, includes)
                .AsExpandable()
                .AsQueryable()
                .AsNoTracking()
                .With(where);

            return await Task.FromResult(query.ToList());
        }

        /// <summary>
        /// Gets the first record from the data store that matches the <paramref name="where"/> parameter
        /// </summary>
        /// <typeparam name="TResult">The projected class</typeparam>
        /// <param name="where">The expression to execute against the data store</param>
        /// <param name="select">The expression for the projection of type <typeparamref name="TResult"/> that should be executed against the data store</param>
        /// <param name="orderBy">The sorting expression to execute against the data store</param>
        /// <param name="ascending">Indicates whether the sorting is ascending (true) or descending (false)</param>
        /// <param name="page">The page number which is multiplied by the pagesize to calculate the amount of items to skip</param>
        /// <param name="pageSize">The size of the batch of items that must be retrieved</param>
        /// <param name="includes">The optional list of related entities that should be eagerly loaded</param>
        /// <returns>An instance of <typeparamref name="TResult"/> with the mapped data from the record that matched all filters.</returns>
        public virtual Task<IEnumerable<TResult>> FindAllAsync<TResult>(
            Expression<Func<TEntity, bool>> where = null,
            Expression<Func<TEntity, TResult>> select = null,
            Expression<Func<TEntity, dynamic>> orderBy = null,
            bool? ascending = null,
            int? page = null,
            int? pageSize = null,
            params string[] includes)
        {
            using TContext ctx = Context;
            IQueryable<TResult> query = ctx.Set<TEntity>()
                .Include(ctx, includes)
                .AsNoTracking()
                .AsExpandable()
                .AsQueryable()
                .With(where)
                .WithOrder(orderBy, ascending ?? true)
                .With(page, pageSize, orderBy)
                .With(pageSize)
                .WithSelect(select);

            return Task.FromResult(query.ToList() as IEnumerable<TResult>);
        }

        /// <summary>
        /// Retrieves a collection of paged, sorted and filtered items in a flat list
        /// </summary>
        /// <param name="where">The expression to execute against the data store</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="ascending"></param>
        /// <param name="page">The page number which is multiplied by the page size to calculate the amount of items to skip</param>
        /// <param name="pageSize">The size of the batch of items that must be retrieved</param>
        /// <param name="includes">The optional list of related entities that should be eagerly loaded</param>
        /// <returns>An collection of <typeparamref name="TEntity"/> with the mapped data from the records that matched all filters.</returns>
        public virtual async Task<IEnumerable<TEntity>> FindAllAsync(
            Expression<Func<TEntity, bool>> where = null,
            Expression<Func<TEntity, dynamic>> orderBy = null,
            bool? ascending = null,
            int? page = null,
            int? pageSize = null,
            params string[] includes)
        {
            await using TContext ctx = Context;
            IQueryable<TEntity> query = ctx.Set<TEntity>()
                .Include(ctx, includes)
                .AsExpandable()
                .AsNoTracking()
                .With(where)
                .WithOrder(orderBy, ascending ?? true)
                .With(page, pageSize, orderBy)
                .With(pageSize);

            return await Task.FromResult(query.ToList());
        }
    }
}