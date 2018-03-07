using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dime.Repositories.Redis
{
    partial class RedisRepository<T>
    {
        public T Create(T entity, bool commit)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Create(T entity)
        {
            using (IRedisClient redisClient = this.RedisManager.GetClient())
            {
                IRedisTypedClient<T> redisTypedClient = redisClient.As<T>();
                IRedisList<T> items = redisTypedClient.Lists[this.ContextKey];

                items.Add(entity);
            }

            return entity;
        }

        public Task<T> CreateAsync(T entity, bool commit)
        {
            throw new NotImplementedException();
        }

        public T CreateAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T> CreateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<T>> CreateAsync(IQueryable<T> entity)
        {
            throw new NotImplementedException();
        }
    }
}