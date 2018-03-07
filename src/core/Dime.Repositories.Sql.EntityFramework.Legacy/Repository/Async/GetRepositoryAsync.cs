using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> where)
        {
            using (TContext ctx = this.Context)
            {
                return await ctx.Set<TEntity>().AsNoTracking().AnyAsync(where);
            }
        }

        /// <summary>
        /// Gets the record by its identifier
        /// </summary>
        /// <param name="id">The identifier of the entity</param>
        /// <returns>The record of type <typeparamref name="TEntity"/> that matches the id</returns>
        public async virtual Task<TEntity> FindByIdAsync(long id)
        {
            using (TContext ctx = this.Context)
            {
                return await ctx.Set<TEntity>().FindAsync(id);
            }
        }

        /// <summary>
        /// Gets the record by its identifier
        /// </summary>
        /// <param name="id">The identifier of the entity</param>
        /// <param name="includes">The optional list of related entities that should be eagerly loaded</param>
        /// <returns>The record of type <typeparamref name="TEntity"/> that matches the id</returns>
        public async virtual Task<TEntity> FindByIdAsync(long id, params string[] includes)
        {
            using (TContext ctx = this.Context)
            {
                foreach (string include in includes)
                {
                    ctx.Set<TEntity>().Include(include).AsNoTracking();
                }

                return await ctx.Set<TEntity>().FindAsync(id);
            }
        }

        /// <summary>
        /// Gets the first record from the data store that matches the <paramref name="where"/> parameter
        /// </summary>
        /// <param name="where">The expression to execute against the data store</param>
        /// <returns>The first record of type <typeparamref name="TEntity"/> that matches the query</returns>
        public virtual async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> where)
        {
            using (TContext ctx = this.Context)
            {
                TEntity query = ctx.Set<TEntity>()
                    .Include(ctx)
                    .AsNoTracking()
                    .AsExpandable()
                    .With(where)
                    .FirstOrDefault();

                return await Task.Run(() => query);
            }
        }

        /// <summary>
        /// Gets the first record from the data store that matches the <paramref name="where"/> parameter
        /// </summary>
        /// <param name="where">The expression to execute against the data store</param>
        /// <param name="includes">The optional list of related entities that should be eagerly loaded</param>
        /// <returns>The first record of type <typeparamref name="TEntity"/> that matches the query</returns>
        public virtual async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> where, params string[] includes)
        {
            using (TContext ctx = this.Context)
            {
                IQueryable<TEntity> query = ctx.Set<TEntity>()
                    .Include(ctx, includes)
                    .AsNoTracking()
                    .AsExpandable()
                    .With(where);

                IQueryable<TEntity> fullGraphQuery = await Task.Run(() => query);
                return fullGraphQuery.FirstOrDefault();
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
        public async Task<TResult> FindOneAsync<TResult>(
            Expression<Func<TEntity, bool>> where = null,
            Expression<Func<TEntity, TResult>> select = null,
            Expression<Func<TEntity, dynamic>> orderBy = null,
            bool? ascending = default(bool?),
            int? page = default(int?),
            int? pageSize = default(int?),
            params string[] includes) where TResult : class
        {
            using (TContext ctx = this.Context)
            {
                IQueryable<TResult> query = ctx.Set<TEntity>()
                .AsExpandable()
                .AsQueryable()
                .AsNoTracking()
                .With(where)
                .WithOrder(orderBy, ascending ?? true)
                .With(page, pageSize)
                .With(pageSize)
                .WithSelect(select);

                IQueryable<TResult> fullGraphQuery = await Task.Run(() => this.Include<TResult>(query, includes));
                return fullGraphQuery.FirstOrDefault();
            }
        }

        /// <summary>
        /// Finds entities based on provided criteria.
        /// </summary>
        /// <param name="where">The expression to execute against the data store</param>
        /// <param name="includes">The optional list of related entities that should be eagerly loaded</param>
        /// <returns>An collection of <typeparamref name="TEntity"/> that matched all filters.</returns>
        /// <history>
        /// [HB] 17/08/2015 - Update with projection
        /// </history>
        public virtual async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> where, params string[] includes)
        {
            using (TContext ctx = this.Context)
            {
                IQueryable<TEntity> query = ctx.Set<TEntity>()
                .Include(ctx, includes)
                .AsExpandable()
                .AsQueryable()
                .AsNoTracking()
                .With(where);

                return await Task.FromResult(query.ToList().AsQueryable());
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
        public async virtual Task<IEnumerable<TResult>> FindAllAsync<TResult>(
            Expression<Func<TEntity, bool>> where = null,
            Expression<Func<TEntity, TResult>> select = null,
            Expression<Func<TEntity, dynamic>> orderBy = null,
            bool? ascending = null,
            int? page = null,
            int? pageSize = null,
            params string[] includes)
        {
            using (TContext ctx = this.Context)
            {
                IQueryable<TResult> query = ctx.Set<TEntity>()
                .Include(ctx, includes)
                .AsNoTracking()
                .AsExpandable()
                .AsQueryable()
                .With(where)
                .WithOrder(orderBy, ascending ?? true)
                .With(page, pageSize)
                .With(pageSize)
                .WithSelect(select);

                return await Task.FromResult(query.ToList().AsQueryable());
            }
        }

        /// <summary>
        /// Retrieves a collection of paged, sorted and filtered items in a flat list
        /// </summary>
        /// <typeparam name="TResult">The projected class</typeparam>
        /// <param name="where">The expression to execute against the data store</param>
        /// <param name="select">The expression for the projection of type <typeparamref name="TResult"/> that should be executed against the data store</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="page">The page number which is multiplied by the pagesize to calculate the amount of items to skip</param>
        /// <param name="pageSize">The size of the batch of items that must be retrieved</param>
        /// <param name="includes">The optional list of related entities that should be eagerly loaded</param>
        /// <returns>An collection of <typeparamref name="TResult"/> with the mapped data from the records that matched all filters.</returns>
        /// <history>
        /// [HB] 17/08/2015 - Update with projection
        /// </history>
        public async virtual Task<IEnumerable<TEntity>> FindAllAsync(
            Expression<Func<TEntity, bool>> where = null,
            Expression<Func<TEntity, dynamic>> orderBy = null,
            bool? ascending = null,
            int? page = null,
            int? pageSize = null,
            params string[] includes)
        {
            using (TContext ctx = this.Context)
            {
                IQueryable<TEntity> query = ctx.Set<TEntity>()
                .Include(ctx, includes)
                .AsExpandable()
                .AsNoTracking()
                .With(where)
                .WithOrder(orderBy, ascending ?? true)
                .With(page, pageSize)
                .With(pageSize);

                return await Task.FromResult(query.ToList().AsQueryable());
            }
        }

        /// <summary>
        /// Utility to automate including related entities in an eagerly fashion
        /// </summary>
        /// <typeparam name="TResult">The projected entity</typeparam>
        /// <param name="query">The query</param>
        /// <param name="includes">The optional list of related entities that should be eagerly loaded</param>
        /// <returns>The original query enriched by related entities</returns>
        private IQueryable<TResult> Include<TResult>(IQueryable<TResult> query, params string[] includes)
        {
            if (includes == null)
            {
                return query;
            }
            else if (includes.Count() == 0)
            {
                MetadataWorkspace workspace = ((IObjectContextAdapter)this.Context).ObjectContext.MetadataWorkspace;
                ObjectItemCollection itemCollection = (ObjectItemCollection)(workspace.GetItemCollection(DataSpace.OSpace));
                EntityType entityType = itemCollection.OfType<EntityType>().Single(e => itemCollection.GetClrType(e) == typeof(TEntity));

                foreach (NavigationProperty navigationProperty in entityType.NavigationProperties)
                {
                    query = query.Include(navigationProperty.Name);
                }

                return query;
            }
            else
            {
                foreach (var include in includes)
                {
                    if (include != null)
                    {
                        query = query.Include(include);
                    }
                }
                return query;
            }
        }
    }
}