using System;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;

namespace Dime.Repositories
{
    public partial class LiteDbRepository<T>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Create(T entity)
        {
            LiteCollection<T> collection = Db.GetCollection<T>(CollectionName);
            collection.Insert(entity);

            return entity;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commit"></param>
        /// <returns></returns>
        public T Create(T entity, bool commit)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<IQueryable<T>> CreateAsync(IQueryable<T> entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<T> CreateAsync(T entity)
        {
            LiteCollection<T> collection = Db.GetCollection<T>(CollectionName);
            collection.Insert(entity);

            return Task.FromResult(entity);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commit"></param>
        /// <returns></returns>
        public Task<T> CreateAsync(T entity, bool commit)
        {
            throw new NotImplementedException();
        }
    }
}