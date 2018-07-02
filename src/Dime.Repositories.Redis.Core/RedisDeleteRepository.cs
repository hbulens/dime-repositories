using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

namespace Dime.Repositories.Redis
{
    public partial class RedisRepository<T>
    {
        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(T entity)
        {
            using (IRedisClient redisClient = RedisManager.GetClient())
            {
                IRedisTypedClient<T> redisTypedClient = redisClient.As<T>();
                IRedisList<T> items = redisTypedClient.Lists[ContextKey];
                items.RemoveValue(entity);
            }
        }

        public void Delete(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long id, bool commit)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(T entity, bool commit)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }
    }
}