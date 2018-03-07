using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dime.Repositories
{
    public partial class LiteDbRepository<T>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        /// <history>
        /// [HB] 05/01/2017 - Review & Refactor
        /// </history>
        public T FindOne(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        /// <history>
        /// [HB] 05/01/2017 - Review & Refactor
        /// </history>
        public T FindOne(Expression<Func<T, bool>> where, params string[] includes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        /// <history>
        /// [HB] 05/01/2017 - Review & Refactor
        /// </history>
        public Task<T> FindOneAsync(Expression<Func<T, bool>> where, params string[] includes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        /// <history>
        /// [HB] 05/01/2017 - Review & Refactor
        /// </history>
        public Task<T> FindOneAsync(Expression<Func<T, bool>> where)
        {
            LiteCollection<T> collection = this.Db.GetCollection<T>(this.CollectionName);
            return Task.FromResult(collection.FindOne(where));
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="where"></param>
        /// <param name="select"></param>
        /// <param name="orderBy"></param>
        /// <param name="ascending"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        /// <history>
        /// [HB] 05/01/2017 - Review & Refactor
        /// </history>
        public Task<TResult> FindOneAsync<TResult>(Expression<Func<T, bool>> where = null, Expression<Func<T, TResult>> select = null, Expression<Func<T, object>> orderBy = null, bool? ascending = default(bool?), int? page = default(int?), int? pageSize = default(int?), params string[] includes) where TResult : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <history>
        /// [HB] 05/01/2017 - Review & Refactor
        /// </history>
        public T FindById(long id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        /// <history>
        /// [HB] 05/01/2017 - Review & Refactor
        /// </history>
        public IEnumerable<T> FindAll(Expression<Func<T, bool>> where)
        {
            LiteCollection<T> collection = this.Db.GetCollection<T>(this.CollectionName);
            return collection.FindAll().ToList();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        /// <history>
        /// [HB] 05/01/2017 - Review & Refactor
        /// </history>
        public IEnumerable<T> FindAll(Expression<Func<T, bool>> where, int? page, int? pageSize, string[] includes)
        {
            LiteCollection<T> collection = this.Db.GetCollection<T>(this.CollectionName);
            return collection.FindAll().ToList();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <param name="includeAll"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        /// <history>
        /// [HB] 05/01/2017 - Review & Refactor
        /// </history>
        public IEnumerable<T> FindAll(Expression<Func<T, bool>> where, bool includeAll, params string[] includes)
        {
            LiteCollection<T> collection = this.Db.GetCollection<T>(this.CollectionName);
            return collection.FindAll().ToList();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <history>
        /// [HB] 05/01/2017 - Review & Refactor
        /// </history>
        public Task<T> FindByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        /// <history>
        /// [HB] 05/01/2017 - Review & Refactor
        /// </history>
        public Task<T> FindByIdAsync(long id, params string[] includes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="where"></param>
        /// <param name="select"></param>
        /// <param name="orderBy"></param>
        /// <param name="ascending"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        /// <history>
        /// [HB] 05/01/2017 - Review & Refactor
        /// </history>
        public Task<IEnumerable<TResult>> FindAllAsync<TResult>(Expression<Func<T, bool>> where = null, Expression<Func<T, TResult>> select = null, Expression<Func<T, object>> orderBy = null, bool? ascending = default(bool?), int? page = default(int?), int? pageSize = default(int?), params string[] includes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="ascending"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        /// <history>
        /// [HB] 05/01/2017 - Review & Refactor
        /// </history>
        public Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> where = null, Expression<Func<T, object>> orderBy = null, bool? ascending = default(bool?), int? page = default(int?), int? pageSize = default(int?), params string[] includes)
        {
            LiteCollection<T> collection = this.Db.GetCollection<T>(this.CollectionName);
            return Task.FromResult(collection.FindAll().ToList() as IEnumerable<T>);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        /// <history>
        /// [HB] 05/01/2017 - Review & Refactor
        /// </history>
        public Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> where, params string[] includes)
        {
            lock (_padlock)
            {
                LiteCollection<T> collection = this.Db.GetCollection<T>(this.CollectionName);
                IEnumerable<T> items = collection.Find(where);
                return Task.FromResult(items);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        /// <history>
        /// [HB] 05/01/2017 - Review & Refactor
        /// </history>
        public Task<IPage<T>> FindAllPagedAsync(Expression<Func<T, bool>> where = null, IEnumerable<IOrder<T>> orderBy = null, int? page = default(int?), int? pageSize = default(int?), params string[] includes)
        {
            LiteCollection<T> collection = this.Db.GetCollection<T>(this.CollectionName);
            IEnumerable<T> items = collection.Find(where, page.GetValueOrDefault(), pageSize == null ? int.MaxValue : pageSize.GetValueOrDefault());
            int collectionCount = collection.Count(where);

            return Task.FromResult(new Page<T>(items, collectionCount) as IPage<T>);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="ascending"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public Task<IPage<T>> FindAllPagedAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, bool? ascending, int? page, int? pageSize, params string[] includes)
        {
            LiteCollection<T> collection = this.Db.GetCollection<T>(this.CollectionName);
            IEnumerable<T> items = collection.Find(where, page.GetValueOrDefault(), pageSize.GetValueOrDefault());
            int collectionCount = collection.Count(where);
            return Task.FromResult(new Page<T>(items, collectionCount)) as Task<IPage<T>>;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="ascending"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public Task<IPage<T>> FindAllPagedAsync(Expression<Func<T, bool>> where, IEnumerable<Expression<Func<T, object>>> orderBy, bool? ascending, int? page, int? pageSize, params string[] includes)
        {
            LiteCollection<T> collection = this.Db.GetCollection<T>(this.CollectionName);
            IEnumerable<T> items = collection.Find(where, page.GetValueOrDefault(), pageSize.GetValueOrDefault());
            int collectionCount = collection.Count(where);
            return Task.FromResult(new Page<T>(items, collectionCount) as IPage<T>) as Task<IPage<T>>;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <param name="count"></param>
        /// <param name="orderBy"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public Task<IPage<T>> FindAllPagedAsync(Expression<Func<T, bool>> where, Expression<Func<T, bool>> count, IEnumerable<IOrder<T>> orderBy, int? page, int? pageSize, params string[] includes)
        {
            LiteCollection<T> collection = this.Db.GetCollection<T>(this.CollectionName);
            IEnumerable<T> items = collection.Find(where, page.GetValueOrDefault(), pageSize.GetValueOrDefault());
            int collectionCount = collection.Count(count);
            return Task.FromResult(new Page<T>(items, collectionCount) as IPage<T>);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="groupBy"></param>
        /// <param name="ascending"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public Task<IPage<T>> FindAllPagedAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, Expression<Func<T, object>> groupBy, bool? ascending, int? page, int? pageSize, params string[] includes)
        {
            LiteCollection<T> collection = this.Db.GetCollection<T>(this.CollectionName);
            IEnumerable<T> items = collection.Find(where, page.GetValueOrDefault(), pageSize.GetValueOrDefault());
            int collectionCount = collection.Count(where);
            return Task.FromResult(new Page<T>(items, collectionCount) as IPage<T>);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="where"></param>
        /// <param name="select"></param>
        /// <param name="orderBy"></param>
        /// <param name="ascending"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public Task<IPage<TResult>> FindAllPagedAsync<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> select, Expression<Func<T, object>> orderBy, bool? ascending, int? page, int? pageSize, params string[] includes) where TResult : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="where"></param>
        /// <param name="groupBy"></param>
        /// <param name="select"></param>
        /// <param name="orderBy"></param>
        /// <param name="ascending"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public Task<IPage<TResult>> FindAllPagedAsync<TResult>(Expression<Func<T, bool>> where, Func<T, object> groupBy, Expression<Func<IGrouping<object, T>, IEnumerable<TResult>>> select, Expression<Func<T, object>> orderBy, bool? ascending, int? page, int? pageSize, params string[] includes) where TResult : class, new()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="where"></param>
        /// <param name="select"></param>
        /// <param name="orderBy"></param>
        /// <param name="groupBy"></param>
        /// <param name="ascending"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public Task<IPage<TResult>> FindAllPagedAsync<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> select, IEnumerable<IOrder<T>> orderBy, Expression<Func<T, object>> groupBy, bool? ascending, int? page, int? pageSize, params string[] includes) where TResult : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="where"></param>
        /// <param name="count"></param>
        /// <param name="select"></param>
        /// <param name="orderBy"></param>
        /// <param name="groupBy"></param>
        /// <param name="ascending"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public Task<IPage<TResult>> FindAllPagedAsync<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, bool>> count, Expression<Func<T, TResult>> select, IEnumerable<IOrder<T>> orderBy, Expression<Func<T, object>> groupBy, bool? ascending, int? page, int? pageSize, params string[] includes) where TResult : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Task<long> Count()
        {
            LiteCollection<T> collection = this.Db.GetCollection<T>(this.CollectionName);
            return Task.FromResult((long)collection.Count());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public Task<long> Count(Expression<Func<T, bool>> where)
        {
            LiteCollection<T> collection = this.Db.GetCollection<T>(this.CollectionName);
            return Task.FromResult((long)collection.Count(where));
        }

        public bool Exists(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}