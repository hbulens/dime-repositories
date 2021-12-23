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
            
            return await Context.Set<TEntity>().AsNoTracking().AnyAsync(where).ConfigureAwait(false);
        }

        public virtual async Task<TEntity> FindByIdAsync(long id) 
            => await Context.Set<TEntity>().FindAsync(id).ConfigureAwait(false);

        public virtual async Task<TEntity> FindByIdAsync(long id, params string[] includes)
        {
            foreach (string include in includes)
                Context.Set<TEntity>().Include(include).AsNoTracking();

            return await Context.Set<TEntity>().FindAsync(id).ConfigureAwait(false);
        }

        public virtual async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> where)
        {
            TEntity query = Context.Set<TEntity>()
                .AsNoTracking()
                .AsExpandable()
                .With(where)
                .FirstOrDefault();

            return await Task.Run(() => query).ConfigureAwait(false);
        }

        public virtual async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> where, params string[] includes)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>()
                .Include(Context, includes)
                .AsNoTracking()
                .AsExpandable()
                .With(where);

            IQueryable<TEntity> fullGraphQuery = await Task.Run(() => query).ConfigureAwait(false);
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
            IQueryable<TResult> query = Context.Set<TEntity>()
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
            IQueryable<TEntity> query = Context.Set<TEntity>()
                .Include(Context, includes)
                .AsExpandable()
                .AsQueryable()
                .AsNoTracking()
                .With(where);

            return await Task.FromResult(query.ToList()).ConfigureAwait(false);
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
            IQueryable<TResult> query = Context.Set<TEntity>()
                .Include(Context, includes)
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
            IQueryable<TEntity> query = Context.Set<TEntity>()
                .Include(Context, includes)
                .AsExpandable()
                .AsNoTracking()
                .With(where)
                .WithOrder(orderBy, ascending ?? true)
                .With(page, pageSize, orderBy)
                .With(pageSize);

            return await Task.FromResult(query.ToList()).ConfigureAwait(false);
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