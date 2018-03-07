using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dime.Repositories.Redis
{
    partial class RedisRepository<T>
    {
        public bool Exists(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        /// <history>
        /// [HB] 06/01/2016 - Create
        /// </history>
        public IEnumerable<T> FindAll(Expression<Func<T, bool>> where)
        {
            try
            {
                using (IRedisClient redisClient = this.RedisManager.GetClient())
                {
                    IRedisTypedClient<T> redisTypedClient = redisClient.As<T>();
                    var items = redisTypedClient.Lists[this.ContextKey];
                    return where != null ? items.AsQueryable().Where(where) : items.AsQueryable();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public T FindOne(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public T FindOne()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T FindById(long id)
        {
            try
            {
                using (IRedisClient redisClient = this.RedisManager.GetClient())
                {
                    IRedisTypedClient<T> redisTypedClient = redisClient.As<T>();
                    IRedisList<T> items = redisTypedClient.Lists[this.ContextKey];
                    return items.FirstOrDefault();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> where, int? page, int? pageSize, string[] includes)
        {
            throw new NotImplementedException();
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }

        public Task<T> FindOneAsync(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindOneAsync(Expression<Func<T, bool>> where, params string[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> FindOneAsync<TResult>(Expression<Func<T, bool>> where = null, Expression<Func<T, TResult>> select = null, Expression<Func<T, object>> orderBy = null, bool? ascending = default(bool?), int? page = default(int?), int? pageSize = default(int?), params string[] includes) where TResult : class
        {
            throw new NotImplementedException();
        }

        public Task<T> FindOneAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T> FindByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindByIdAsync(long id, params string[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TResult>> FindAllAsync<TResult>(Expression<Func<T, bool>> where = null, Expression<Func<T, TResult>> select = null, Expression<Func<T, object>> orderBy = null, bool? ascending = default(bool?), int? page = default(int?), int? pageSize = default(int?), params string[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> where = null, Expression<Func<T, object>> orderBy = null, bool? ascending = default(bool?), int? page = default(int?), int? pageSize = default(int?), params string[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> where, params string[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> FindOneAsync<TResult>(Expression<Func<T, bool>> where = null, Expression<Func<T, TResult>> select = null, Func<T, object> orderBy = null, bool? ascending = default(bool?), int? page = default(int?), int? pageSize = default(int?), params string[] includes) where TResult : class
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TResult>> FindAllAsync<TResult>(Expression<Func<T, bool>> where = null, Expression<Func<T, TResult>> select = null, Func<T, object> orderBy = null, bool? ascending = default(bool?), int? page = default(int?), int? pageSize = default(int?), params string[] includes) where TResult : class
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> where = null, Func<T, object> orderBy = null, bool? ascending = default(bool?), int? page = default(int?), int? pageSize = default(int?), params string[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> FindOneAsync<TResult>(Expression<Func<T, bool>> where = null, Func<T, TResult> select = null, Func<T, object> orderBy = null, bool? ascending = default(bool?), int? page = default(int?), int? pageSize = default(int?), params string[] includes) where TResult : class
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TResult>> FindAllAsync<TResult>(Expression<Func<T, bool>> where = null, Func<T, TResult> select = null, Func<T, object> orderBy = null, bool? ascending = default(bool?), int? page = default(int?), int? pageSize = default(int?), params string[] includes) where TResult : class
        {
            throw new NotImplementedException();
        }

        public Task<TResult> FindOneAsync<TResult>(Expression<Func<T, bool>> where = null, Func<T, TResult> select = null, Expression<Func<T, object>> orderBy = null, bool? ascending = default(bool?), int? page = default(int?), int? pageSize = default(int?), params string[] includes) where TResult : class
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TResult>> FindAllAsync<TResult>(Expression<Func<T, bool>> where = null, Func<T, TResult> select = null, Expression<Func<T, object>> orderBy = null, bool? ascending = default(bool?), int? page = default(int?), int? pageSize = default(int?), params string[] includes) where TResult : class
        {
            throw new NotImplementedException();
        }

        Task<IPage<T>> IPagedQueryRepositoryAsync<T>.FindAllPagedAsync(Expression<Func<T, bool>> where, IEnumerable<IOrder<T>> orderBy, int? page, int? pageSize, params string[] includes)
        {
            throw new NotImplementedException();
        }

        Task<IPage<T>> IPagedQueryRepositoryAsync<T>.FindAllPagedAsync(Expression<Func<T, bool>> where, Expression<Func<T, bool>> count, IEnumerable<IOrder<T>> orderBy, int? page, int? pageSize, params string[] includes)
        {
            throw new NotImplementedException();
        }

        Task<IPage<T>> IPagedQueryRepositoryAsync<T>.FindAllPagedAsync(Expression<Func<T, bool>> where, IEnumerable<Expression<Func<T, object>>> orderBy, bool? ascending, int? page, int? pageSize, params string[] includes)
        {
            throw new NotImplementedException();
        }

        Task<IPage<T>> IPagedQueryRepositoryAsync<T>.FindAllPagedAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, bool? ascending, int? page, int? pageSize, params string[] includes)
        {
            throw new NotImplementedException();
        }

        Task<IPage<T>> IPagedQueryRepositoryAsync<T>.FindAllPagedAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, Expression<Func<T, object>> groupBy, bool? ascending, int? page, int? pageSize, params string[] includes)
        {
            throw new NotImplementedException();
        }

        Task<IPage<TResult>> IPagedQueryRepositoryAsync<T>.FindAllPagedAsync<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> select, IEnumerable<IOrder<T>> orderBy, Expression<Func<T, object>> groupBy, bool? ascending, int? page, int? pageSize, params string[] includes)
        {
            throw new NotImplementedException();
        }

        Task<IPage<TResult>> IPagedQueryRepositoryAsync<T>.FindAllPagedAsync<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, bool>> count, Expression<Func<T, TResult>> select, IEnumerable<IOrder<T>> orderBy, Expression<Func<T, object>> groupBy, bool? ascending, int? page, int? pageSize, params string[] includes)
        {
            throw new NotImplementedException();
        }

        Task<IPage<TResult>> IPagedQueryRepositoryAsync<T>.FindAllPagedAsync<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> select, Expression<Func<T, object>> orderBy, bool? ascending, int? page, int? pageSize, params string[] includes)
        {
            throw new NotImplementedException();
        }

        Task<IPage<TResult>> IPagedQueryRepositoryAsync<T>.FindAllPagedAsync<TResult>(Expression<Func<T, bool>> where, Func<T, object> groupBy, Expression<Func<IGrouping<object, T>, IEnumerable<TResult>>> select, Expression<Func<T, object>> orderBy, bool? ascending, int? page, int? pageSize, params string[] includes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> where, bool includeAll, params string[] includes)
        {
            throw new NotImplementedException();
        }

        public T FindOne(Expression<Func<T, bool>> where, params string[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<long> Count(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<long> Count()
        {
            throw new NotImplementedException();
        }
    }
}