using LiteDB;
using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class LiteDbRepository<T> : IRepository<T> where T : class, new()
    {
        #region Constructor

        /// <summary>
        ///
        /// </summary>
        /// <param name="connection">Connection path to the database</param>
        public LiteDbRepository(string connection)
        {
            this.Db = new LiteDatabase(connection);

            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            this.CollectionName = rgx.Replace(typeof(T).Name.EndsWith("s") ? typeof(T).Name : string.Format("{0}s", typeof(T).Name), "");
        }

        #endregion Constructor

        #region Properties

        private readonly string CollectionName;
        private static readonly object _padlock = new object();
        private readonly LiteDatabase Db;

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

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            this.Db.Dispose();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Task<bool> SaveChangesAsync()
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="repository"></param>
        public static explicit operator LiteDatabase(LiteDbRepository<T> repository)
        {
            return repository.Db;
        }

        #endregion Methods
    }
}