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
        #region Projected Pages

        /// <summary>
        /// Finds all hronous.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="where">The where.</param>
        /// <param name="select">The select.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="ascending"></param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="includes">The includes.</param>
        /// <returns></returns>
        public virtual IPage<TResult> FindAllPaged<TResult>(
            Expression<Func<TEntity, bool>> where = null,
            Expression<Func<TEntity, TResult>> select = null,
            Expression<Func<TEntity, dynamic>> orderBy = null,
            bool? ascending = null,
            int? page = null,
            int? pageSize = null,
            params string[] includes)
            where TResult : class
        {
            using (TContext ctx = Context)
            {
                IQueryable<TResult> query =
                    ctx.Set<TEntity>()
                    .AsExpandable()
                    .AsNoTracking()
                    .With(where)
                    .WithOrder(orderBy, ascending ?? true)
                    .With(page, pageSize, orderBy)
                    .With(pageSize)
                    .WithSelect(select);

                IQueryable<TResult> fullGraphQuery = Include(query, includes);
                return new Page<TResult>(fullGraphQuery.ToList(), ctx.Count(where));
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="where"></param>
        /// <param name="groupBy"></param>
        /// <param name="select"></param>
        /// <param name="orderBy"></param>
        /// <param name="ascending"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IPage<TResult> FindAllPaged<TResult>(
            Expression<Func<TEntity, bool>> where = null,
            Func<TEntity, object> groupBy = null,
            Expression<Func<IGrouping<object, TEntity>, IEnumerable<TResult>>> select = null,
            Expression<Func<TEntity, dynamic>> orderBy = null,
            bool? ascending = default(bool?),
            int? page = default(int?),
            int? pageSize = default(int?),
            params string[] includes) where TResult : class, new()
        {
            using (TContext ctx = Context)
            {
                IQueryable<TResult> query =
                ctx.Set<TEntity>()
                .AsExpandable()
                .AsNoTracking()
                .With(where)
                .WithOrder(orderBy, ascending ?? true)
                .With(page, pageSize, orderBy)
                .With(pageSize)
                .WithGroup(groupBy)
                .WithSelect<TEntity, TResult, object>(select);

                IQueryable<TResult> fullGraphQuery = Include(query, includes);
                return new Page<TResult>(fullGraphQuery.ToList(), ctx.Count(where));
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="where"></param>
        /// <param name="select"></param>
        /// <param name="orderBy"></param>
        /// <param name="groupBy"></param>
        /// <param name="ascending"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IPage<TResult> FindAllPaged<TResult>(
           Expression<Func<TEntity, bool>> where = null,
            Expression<Func<TEntity, TResult>> select = null,
            IEnumerable<IOrder<TEntity>> orderBy = null,
            Expression<Func<TEntity, object>> groupBy = null,
            bool? ascending = default(bool?),
            int? page = default(int?),
            int? pageSize = default(int?),
            params string[] includes) where TResult : class
        {
            using (TContext ctx = Context)
            {
                IQueryable<TResult> query =
                ctx.Set<TEntity>()
                .Include(ctx, includes)
                .AsExpandable()
                .AsNoTracking()
                .With(where)
                .WithOrder(orderBy)
                .With(page, pageSize, orderBy)
                .With(pageSize)
                .WithSelect(select);

                return new Page<TResult>(query.ToList(), ctx.Count(where));
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="where"></param>
        /// <param name="count"></param>
        /// <param name="select"></param>
        /// <param name="orderBy"></param>
        /// <param name="groupBy"></param>
        /// <param name="ascending"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IPage<TResult> FindAllPaged<TResult>(
           Expression<Func<TEntity, bool>> where = null,
            Expression<Func<TEntity, bool>> count = null,
            Expression<Func<TEntity, TResult>> select = null,
            IEnumerable<IOrder<TEntity>> orderBy = null,
            Expression<Func<TEntity, object>> groupBy = null,
            bool? ascending = default(bool?),
            int? page = default(int?),
            int? pageSize = default(int?),
            params string[] includes) where TResult : class
        {
            using (TContext ctx = Context)
            {
                IQueryable<TResult> query =
                    ctx.Set<TEntity>()
                    .Include(ctx, includes)
                    .AsNoTracking()
                    .AsExpandable()
                    .AsQueryable()
                    .With(where)
                    .WithOrder(orderBy)
                    .With(page, pageSize, orderBy)
                    .With(pageSize)
                    .WithSelect(select);

                return new Page<TResult>(query.ToList(), ctx.Count(count));
            }
        }

        #endregion Projected Pages

        #region Unprojected Pages

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="ascending"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IPage<TEntity> FindAllPaged(
           Expression<Func<TEntity, bool>> where = null,
           Expression<Func<TEntity, dynamic>> orderBy = null,
           bool? ascending = null,
           int? page = default(int?),
           int? pageSize = default(int?),
           params string[] includes)
        {
            using (TContext ctx = Context)
            {
                IQueryable<TEntity> query =
                ctx.Set<TEntity>()
                .Include(ctx, includes)
                .AsExpandable()
                .AsNoTracking()
                .With(where)
                .WithOrder(orderBy, ascending ?? true)
                .With(page, pageSize, orderBy)
                .With(pageSize)
                .AsQueryable();

                return new Page<TEntity>(query.ToList(), ctx.Count(where));
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="groupBy"></param>
        /// <param name="ascending"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IPage<TEntity> FindAllPaged(
           Expression<Func<TEntity, bool>> where = null,
           Expression<Func<TEntity, dynamic>> orderBy = null,
           Expression<Func<TEntity, object>> groupBy = null,
           bool? ascending = default(bool?),
           int? page = default(int?),
           int? pageSize = default(int?),
           params string[] includes)
        {
            using (TContext ctx = Context)
            {
                IQueryable<TEntity> query =
                ctx.Set<TEntity>()
                .Include(ctx, includes)
                .AsExpandable()
                .AsNoTracking()
                .With(where)
                .WithOrder(orderBy, ascending ?? true)
                .With(page, pageSize, orderBy)
                .With(pageSize);

                return new Page<TEntity>(query.ToList(), ctx.Count(where));
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <param name="count"></param>
        /// <param name="orderBy"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IPage<TEntity> FindAllPaged(
           Expression<Func<TEntity, bool>> where = null,
           Expression<Func<TEntity, bool>> count = null,
           IEnumerable<IOrder<TEntity>> orderBy = null,
           int? page = default(int?),
           int? pageSize = default(int?),
           params string[] includes)
        {
            using (TContext ctx = Context)
            {
                IQueryable<TEntity> query =
                    ctx.Set<TEntity>()
                    .Include(ctx, includes)
                    .AsNoTracking()
                    .AsExpandable()
                    .With(where)
                    .WithOrder(orderBy)
                    .With(page, pageSize, orderBy)
                    .With(pageSize);

                return new Page<TEntity>(query.ToList(), ctx.Count(count));
            }
        }

        /// <summary>
        /// Gets the items hronously.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="where">The where.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="includes">The includes.</param>
        /// <returns></returns>
        public IPage<TEntity> FindAllPaged(
           Expression<Func<TEntity, bool>> where = null,
           IEnumerable<IOrder<TEntity>> orderBy = null,
           int? page = default(int?),
           int? pageSize = default(int?),
           params string[] includes)
        {
            using (TContext ctx = Context)
            {
                IQueryable<TEntity> query =
                 ctx.Set<TEntity>()
                 .Include(ctx, includes)
                 .AsNoTracking()
                 .AsExpandable()
                 .With(where)
                 .WithOrder(orderBy)
                 .With(page, pageSize, orderBy)
                 .With(pageSize);

                return new Page<TEntity>(query.ToList(), ctx.Count(where));
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="ascending"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IPage<TEntity> FindAllPaged(
           Expression<Func<TEntity, bool>> where = null,
           IEnumerable<Expression<Func<TEntity, object>>> orderBy = null,
           bool? ascending = default(bool?),
           int? page = default(int?),
           int? pageSize = default(int?),
           params string[] includes)
        {
            using (TContext ctx = Context)
            {
                IQueryable<TEntity> query =
                 ctx.Set<TEntity>()
                 .Include(ctx, includes)
                 .AsExpandable()
                 .AsNoTracking()
                 .With(where)
                 .WithOrder(orderBy, ascending ?? true)
                 .With(page, pageSize, orderBy)
                 .With(pageSize)
                 .AsQueryable();

                return new Page<TEntity>(query.ToList(), ctx.Count(where));
            }
        }

        #endregion Unprojected Pages
    }
}