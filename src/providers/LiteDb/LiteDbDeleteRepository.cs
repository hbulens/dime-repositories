using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LiteDB;

namespace Dime.Repositories
{
    public partial class LiteDbRepository<T>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        public void Delete(Expression<Func<T, bool>> where)
        {
            LiteCollection<T> collection = Db.GetCollection<T>(CollectionName);
            collection.Delete(where);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        public void Delete(long id)
        {
            LiteCollection<T> collection = Db.GetCollection<T>(CollectionName);
            collection.Delete(id);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public Task DeleteAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public Task DeleteAsync(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(long id)
        {
            LiteCollection<T> collection = Db.GetCollection<T>(CollectionName);
            collection.Delete(id);
            await Task.FromResult(0);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commit"></param>
        /// <returns></returns>
        public Task DeleteAsync(T entity, bool commit)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="commit"></param>
        /// <returns></returns>
        public Task DeleteAsync(long id, bool commit)
        {
            throw new NotImplementedException();
        }
    }
}