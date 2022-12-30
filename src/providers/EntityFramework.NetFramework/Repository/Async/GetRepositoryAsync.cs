using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LinqKit;

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> where)
        {
            using TContext ctx = Context;
            return await ctx.Set<TEntity>().AsNoTracking().AnyAsync(where);
        }

        public virtual async Task<TEntity> FindByIdAsync(object? id)
        {
            using TContext ctx = Context;
            return await ctx.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task<TEntity> FindByIdAsync(object? id, params string[] includes)
        {
            using TContext ctx = Context;
            foreach (string include in includes)
                ctx.Set<TEntity>().Include(include).AsNoTracking();

            return await ctx.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> where)
        {
            using TContext ctx = Context;
            TEntity query = ctx.Set<TEntity>()
                .AsNoTracking()
                .AsExpandable()
                .With(where)
                .FirstOrDefault();

            return await Task.Run(() => query);
        }

        public virtual async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> where, params string[] includes)
        {
            using TContext ctx = Context;
            IQueryable<TEntity> query = ctx.Set<TEntity>()
                .Include(ctx, includes)
                .AsNoTracking()
                .AsExpandable()
                .With(where);

            IQueryable<TEntity> fullGraphQuery = await Task.Run(() => query);
            return fullGraphQuery.FirstOrDefault();
        }

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
                .WithSelect(select);

            IQueryable<TResult> fullGraphQuery = Include(query, includes);
            return Task.FromResult(fullGraphQuery.FirstOrDefault());
        }

        public virtual async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> where, params string[] includes)
        {
            using TContext ctx = Context;
            IQueryable<TEntity> query = ctx.Set<TEntity>()
                .Include(ctx, includes)
                .AsExpandable()
                .AsQueryable()
                .AsNoTracking()
                .With(where);

            return await Task.FromResult(query.ToList());
        }

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

        public virtual async Task<IEnumerable<TEntity>> FindAllAsync(
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

            return await Task.FromResult(query.ToList());
        }

        private IQueryable<TResult> Include<TResult>(IQueryable<TResult> query, params string[] includes)
        {
            if (includes == null)
                return query;

            if (includes.Any())
                return includes
                    .Where(include => include != null)
                    .Aggregate(query, (current, include) => current.Include(include));

            MetadataWorkspace workspace = ((IObjectContextAdapter)Context).ObjectContext.MetadataWorkspace;
            ObjectItemCollection itemCollection = (ObjectItemCollection)workspace.GetItemCollection(DataSpace.OSpace);
            EntityType entityType = itemCollection.OfType<EntityType>().Single(e => itemCollection.GetClrType(e) == typeof(TEntity));

            return entityType.NavigationProperties.Aggregate(query, (current, navigationProperty) => current.Include(navigationProperty.Name));
        }
    }
}