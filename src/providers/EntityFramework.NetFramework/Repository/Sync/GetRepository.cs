using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
        public bool Exists(Expression<Func<TEntity, bool>> where)
        {
            using TContext ctx = Context;
            return ctx.Set<TEntity>().AsNoTracking().Any(where);
        }

        public virtual TEntity FindById(object? id)
        {
            using TContext ctx = Context;
            return ctx.Set<TEntity>().Find(id);
        }

        public virtual TEntity FindById(object? id, params string[] includes)
        {
            using TContext ctx = Context;
            foreach (string include in includes)
                ctx.Set<TEntity>().Include(include).AsNoTracking();

            return ctx.Set<TEntity>().Find(id);
        }

        public virtual TEntity FindOne(Expression<Func<TEntity, bool>> where)
        {
            using TContext ctx = Context;
            TEntity query = ctx.Set<TEntity>()
                .AsNoTracking()
                .AsExpandable()
                .With(where)
                .FirstOrDefault();

            return query;
        }

        public virtual TEntity FindOne(Expression<Func<TEntity, bool>> where, params string[] includes)
        {
            using TContext ctx = Context;
            IQueryable<TEntity> query = ctx.Set<TEntity>()
                .Include(ctx, includes)
                .AsNoTracking()
                .AsExpandable()
                .With(where);

            return query.FirstOrDefault();
        }

        public TResult FindOne<TResult>(
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
                .WithSelect(select);

            return Include(query, includes).FirstOrDefault();
        }

        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> where)
        {
            using TContext ctx = Context;
            IQueryable<TEntity> query = ctx.Set<TEntity>()
                .Include(ctx, null)
                .AsExpandable()
                .AsQueryable()
                .AsNoTracking()
                .With(where);

            return query.ToList();
        }

        public virtual IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> where, params string[] includes)
        {
            using TContext ctx = Context;
            IQueryable<TEntity> query = ctx.Set<TEntity>()
                .Include(ctx, includes)
                .AsExpandable()
                .AsQueryable()
                .AsNoTracking()
                .With(where);

            return query.ToList();
        }

        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> where, bool includeAll, params string[] includes)
        {
            using TContext ctx = Context;
            IQueryable<TEntity> query = ctx.Set<TEntity>()
                .Include(ctx, includes)
                .AsExpandable()
                .AsQueryable()
                .AsNoTracking()
                .With(where);

            return query.ToList();
        }

        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> where, int? page, int? pageSize, string[] includes)
        {
            using TContext ctx = Context;
            IQueryable<TEntity> query = ctx.Set<TEntity>()
                .AsExpandable()
                .AsQueryable()
                .AsNoTracking()
                .With(where)
                .With(page, pageSize, default(IEnumerable<Expression<Func<TEntity, object>>>))
                .With(pageSize);

            return Include(query, includes);
        }

        public virtual IEnumerable<TResult> FindAll<TResult>(
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
                .With(where)
                .WithOrder(orderBy, ascending ?? true)
                .With(page, pageSize, orderBy)
                .With(pageSize)
                .WithSelect(select);

            return query.ToList();
        }

        public virtual IEnumerable<TEntity> FindAll(
           Expression<Func<TEntity, bool>> where = null,
           Expression<Func<TEntity, dynamic>> orderBy = null,
           bool? ascending = null,
           int? page = null,
           int? pageSize = null,
           params string[] includes)
        {
            using TContext ctx = Context;
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