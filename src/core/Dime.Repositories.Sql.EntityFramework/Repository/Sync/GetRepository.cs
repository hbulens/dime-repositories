using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
        /// <summary>
        /// Checks if there is any record that matches the query
        /// </summary>
        /// <param name="where">The query to execute in the Any method</param>
        /// <returns>True if there is at least one record</returns>
        public bool Exists(Expression<Func<TEntity, bool>> where)
        {
            using (TContext ctx = Context)
                return ctx.Set<TEntity>().AsNoTracking().Any(where);
        }

        /// <summary>
        /// Gets the record by its identifier
        /// </summary>
        /// <param name="id">The identifier of the entity</param>
        /// <returns>The record of type <typeparamref name="TEntity"/> that matches the id</returns>
        public virtual TEntity FindById(long id)
        {
            using (TContext ctx = Context)
                return ctx.Set<TEntity>().Find(id);
        }

        /// <summary>
        /// Gets the record by its identifier
        /// </summary>
        /// <param name="id">The identifier of the entity</param>
        /// <param name="includes">The optional list of related entities that should be eagerly loaded</param>
        /// <returns>The record of type <typeparamref name="TEntity"/> that matches the id</returns>
        public virtual TEntity FindById(long id, params string[] includes)
        {
            using (TContext ctx = Context)
            {
                foreach (string include in includes)
                    ctx.Set<TEntity>().Include(include).AsNoTracking();

                return ctx.Set<TEntity>().Find(id);
            }
        }

        /// <summary>
        /// Gets the first record from the data store that matches the <paramref name="where"/> parameter
        /// </summary>
        /// <param name="where">The expression to execute against the data store</param>
        /// <returns>The first record of type <typeparamref name="TEntity"/> that matches the query</returns>
        public virtual TEntity FindOne(Expression<Func<TEntity, bool>> where)
        {
            using (TContext ctx = Context)
            {
                TEntity query = ctx.Set<TEntity>()
                    .AsNoTracking()
                    .AsExpandable()
                    .With(where)
                    .FirstOrDefault();

                return query;
            }
        }

        /// <summary>
        /// Gets the first record from the data store that matches the <paramref name="where"/> parameter
        /// </summary>
        /// <param name="where">The expression to execute against the data store</param>
        /// <param name="includes">The optional list of related entities that should be eagerly loaded</param>
        /// <returns>The first record of type <typeparamref name="TEntity"/> that matches the query</returns>
        public virtual TEntity FindOne(Expression<Func<TEntity, bool>> where, params string[] includes)
        {
            using (TContext ctx = Context)
            {
                IQueryable<TEntity> query = ctx.Set<TEntity>()
                    .Include(ctx, includes)
                    .AsNoTracking()
                    .AsExpandable()
                    .With(where);

                return query.FirstOrDefault();
            }
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
        public TResult FindOne<TResult>(
           Expression<Func<TEntity, bool>> where = null,
            Expression<Func<TEntity, TResult>> select = null,
            Expression<Func<TEntity, dynamic>> orderBy = null,
            bool? ascending = default(bool?),
            int? page = default(int?),
            int? pageSize = default(int?),
            params string[] includes) where TResult : class
        {
            using (TContext ctx = Context)
            {
                IQueryable<TResult> query = ctx.Set<TEntity>()
                .AsExpandable()
                .AsQueryable()
                .AsNoTracking()
                .With(where)
                .WithOrder(orderBy, ascending ?? true)
                .With(page, pageSize, orderBy)
                .With(pageSize)
                .WithSelect(select);

                return Include(query, includes).FirstOrDefault();
            }
        }

        /// <summary>
        /// Finds entities based on provided criteria.
        /// </summary>
        /// <param name="where">The expression to execute against the data store</param>
        /// <returns>An collection of <typeparamref name="TEntity"/> that matched all filters.</returns>
        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> where)
        {
            using (TContext ctx = Context)
            {
                IQueryable<TEntity> query = ctx.Set<TEntity>()
                .Include(ctx, null)
                .AsExpandable()
                .AsQueryable()
                .AsNoTracking()
                .With(where);

                return query.ToList();
            }
        }

        /// <summary>
        /// Finds entities based on provided criteria.
        /// </summary>
        /// <param name="where">The expression to execute against the data store</param>
        /// <param name="includes">The optional list of related entities that should be eagerly loaded</param>
        /// <returns>An collection of <typeparamref name="TEntity"/> that matched all filters.</returns>
        public virtual IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> where, params string[] includes)
        {
            using (TContext ctx = Context)
            {
                IQueryable<TEntity> query = ctx.Set<TEntity>()
                .Include(ctx, includes)
                .AsExpandable()
                .AsQueryable()
                .AsNoTracking()
                .With(where);

                return query.ToList();
            }
        }

        /// <summary>
        /// Finds entities based on provided criteria.
        /// </summary>
        /// <param name="where">The expression to execute against the data store</param>
        /// <param name="includeAll">The flag to indicate if all navigation properties should be eagerly loaed</param>
        /// <param name="includes">The optional list of related entities that should be eagerly loaded</param>
        /// <returns>An collection of <typeparamref name="TEntity"/> that matched all filters.</returns>
        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> where, bool includeAll, params string[] includes)
        {
            using (TContext ctx = Context)
            {
                IQueryable<TEntity> query = ctx.Set<TEntity>()
                .Include(ctx, includes)
                .AsExpandable()
                .AsQueryable()
                .AsNoTracking()
                .With(where);

                return query.ToList();
            }
        }

        /// <summary>
        /// Retrieves a collection of paged and filtered items in a flat list
        /// </summary>
        /// <param name="where">The expression to execute against the data store</param>
        /// <param name="page">The page number which is multiplied by the pagesize to calculate the amount of items to skip</param>
        /// <param name="pageSize">The size of the batch of items that must be retrieved</param>
        /// <param name="includes">The optional list of related entities that should be eagerly loaded</param>
        /// <returns>An collection of <typeparamref name="TEntity"/> with the mapped data from the records that matched all filters.</returns>
        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> where, int? page, int? pageSize, string[] includes)
        {
            using (TContext ctx = Context)
            {
                IQueryable<TEntity> query = ctx.Set<TEntity>()
                .AsExpandable()
                .AsQueryable()
                .AsNoTracking()
                .With(where)
                .With(page, pageSize, default(IEnumerable<Expression<Func<TEntity, object>>>))
                .With(pageSize);

                return Include(query, includes);
            }
        }

        /// <summary>
        /// Gets the records from the data store that matches the <paramref name="where"/> parameter
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
        public virtual IEnumerable<TResult> FindAll<TResult>(
            Expression<Func<TEntity, bool>> where = null,
            Expression<Func<TEntity, TResult>> select = null,
            Expression<Func<TEntity, dynamic>> orderBy = null,
            bool? ascending = null,
            int? page = null,
            int? pageSize = null,
            params string[] includes)
        {
            using (TContext ctx = Context)
            {
                IQueryable<TResult> query = ctx.Set<TEntity>()
                .Include(ctx, includes)
                .AsNoTracking()
                .AsExpandable()
                .With(where)
                .WithOrder(orderBy, ascending ?? true)
                .With(page, pageSize, orderBy)
                .With(pageSize)
                .WithSelect(select);

                return query.ToList();
            }
        }

        /// <summary>
        /// Retrieves a collection of paged, sorted and filtered items in a flat list
        /// </summary>
        /// <param name="where">The expression to execute against the data store</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="ascending"></param>
        /// <param name="page">The page number which is multiplied by the pagesize to calculate the amount of items to skip</param>
        /// <param name="pageSize">The size of the batch of items that must be retrieved</param>
        /// <param name="includes">The optional list of related entities that should be eagerly loaded</param>
        /// <returns>An collection of <typeparamref name="TEntity"/> with the mapped data from the records that matched all filters.</returns>
        public virtual IEnumerable<TEntity> FindAll(
           Expression<Func<TEntity, bool>> where = null,
           Expression<Func<TEntity, dynamic>> orderBy = null,
           bool? ascending = null,
           int? page = null,
           int? pageSize = null,
           params string[] includes)
        {
            using (TContext ctx = Context)
            {
                IQueryable<TEntity> query = ctx.Set<TEntity>()
                .Include(ctx, includes)
                .AsExpandable()
                .AsNoTracking()
                .With(where)
                .WithOrder(orderBy, ascending ?? true)
                .With(page, pageSize, orderBy)
                .With(pageSize);

                return query.ToList();
            }
        }
    }
}