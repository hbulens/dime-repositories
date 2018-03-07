using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dime.Repositories
{
    public partial class LiteDbRepository<T>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <history>
        /// [HB] 05/01/2017 - Review & Refactor
        /// </history>
        public T Update(T entity)
        {
            LiteCollection<T> collection = this.Db.GetCollection<T>(this.CollectionName);
            collection.Update(entity);

            return entity;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commitChanges"></param>
        /// <returns></returns>
        /// <history>
        /// [HB] 05/01/2017 - Review & Refactor
        /// </history>
        public T Update(T entity, bool commitChanges = true)
        {
            LiteCollection<T> collection = this.Db.GetCollection<T>(this.CollectionName);
            collection.Update(entity);

            return entity;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commitChanges"></param>
        /// <returns></returns>
        /// <history>
        /// [HB] 05/01/2017 - Review & Refactor
        /// </history>
        public Task<T> UpdateAsync(T entity, bool commitChanges = true)
        {
            LiteCollection<T> collection = this.Db.GetCollection<T>(this.CollectionName);
            collection.Update(entity);

            return Task.FromResult(entity);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="commitChanges"></param>
        /// <returns></returns>
        /// <history>
        /// [HB] 05/01/2017 - Review & Refactor
        /// </history>
        public async Task UpdateAsync(IEnumerable<T> entities, bool commitChanges = true)
        {
            LiteCollection<T> collection = this.Db.GetCollection<T>(this.CollectionName);
            collection.Update(entities);

            await Task.FromResult(0);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        /// <history>
        /// [HB] 05/01/2017 - Review & Refactor
        /// </history>
        public Task<T> UpdateAsync(T entity, params Expression<Func<T, object>>[] properties)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        /// <history>
        /// [HB] 05/01/2017 - Review & Refactor
        /// </history>
        public Task<T> UpdateAsync(T entity, params string[] properties)
        {
            throw new NotImplementedException();
        }
    }
}