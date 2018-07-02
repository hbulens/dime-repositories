using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ServiceStack.Redis;

namespace Dime.Repositories.Redis
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>    
    public partial class RedisRepository<T> : IRepository<T> where T : class
    {
        #region Constructor

        /// <summary>
        ///
        /// </summary>
        /// <param name="redisContext"></param>
        ///  <history>
        /// [HB] 06/01/2016 - Create
        /// </history>
        public RedisRepository(IRedisClientsManager redisContext, string contextKey)
        {
            RedisManager = redisContext;
            ContextKey = contextKey;
        }

        #endregion Constructor

        #region Properties

        private IRedisClientsManager RedisManager { get; }
        private string ContextKey { get; }

        /// <summary>
        ///
        /// </summary>
        public IMultiTenantRepositoryConfiguration Configuration
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion Properties

        #region Methods

        #region Get

        public Task<bool> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<T> CreateAsync(T entity, Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<long> CountAsync()
        {
            throw new NotImplementedException();
        }

        long IQueryRepositoryAsync<T>.Count()
        {
            throw new NotImplementedException();
        }

        public long CountAsync(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        Task<long> IQueryRepositoryAsync<T>.CountAsync(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        long IQueryRepositoryAsync<T>.Count(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public System.Collections.Generic.IEnumerable<TResult> FindAll<TResult>(Expression<Func<T, bool>> where = null, Expression<Func<T, TResult>> select = null, Expression<Func<T, object>> orderBy = null, bool? ascending = null, int? page = null, int? pageSize = null, params string[] includes)
        {
            throw new NotImplementedException();
        }

        #endregion Get

        #endregion Methods
    }
}