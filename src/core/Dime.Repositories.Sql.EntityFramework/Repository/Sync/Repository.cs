using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public partial class EfRepository<TEntity, TContext> : ISqlRepository<TEntity>, IDisposable
        where TEntity : class, new()
        where TContext : DbContext
    {
        #region Constructor

        /// <summary>
        ///
        /// </summary>
        /// <param name="dbContext"></param>
        public EfRepository(TContext dbContext)
        {
            this.Context = dbContext;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="configuration"></param>
        public EfRepository(TContext dbContext, IMultiTenantRepositoryConfiguration configuration)
        {
            this.Context = dbContext;
            this.Configuration = configuration;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dbContextFactory"></param>
        public EfRepository(IMultiTenantDbContextFactory<TContext> dbContextFactory)
        {
            this.Factory = dbContextFactory;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dbContextFactory"></param>
        /// <param name="configuration"></param>
        public EfRepository(IMultiTenantDbContextFactory<TContext> dbContextFactory, IMultiTenantRepositoryConfiguration configuration)
        {
            this.Factory = dbContextFactory;
            this.Configuration = configuration;
        }

        #endregion Constructor

        #region Properties

        private TContext _context;

        protected TContext Context
        {
            get
            {
                return _context == null ? this.Factory.Create(new DbContextFactoryOptions()) : _context;
            }
            set
            {
                _context = value;
            }
        }

        private IMultiTenantDbContextFactory<TContext> Factory { get; set; }
        public IMultiTenantRepositoryConfiguration Configuration { get; set; }

        #endregion Properties

        #region Methods

        #region Save Changes Region

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual bool SaveChanges(TContext context)
        {
            int retryMax = 0;
            bool saveFailed = false;
            do
            {
                try
                {
                    if (!this.Configuration.SaveInBatch)
                    {
                        int result = context.SaveChanges();
                        return 0 < result;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (DbUpdateConcurrencyException dbUpdateConcurrencyEx)
                {
                    if (this.Configuration.SaveStrategy == ConcurrencyStrategy.ClientFirst)
                    {
                        foreach (EntityEntry failedEnttry in dbUpdateConcurrencyEx.Entries)
                        {
                            var dbValues = failedEnttry.GetDatabaseValues();
                            if (dbValues != null)
                            {
                                failedEnttry.OriginalValues.SetValues(dbValues);
                                return this.SaveChanges(context);
                            }
                        }
                        return true;
                    }
                    else
                    {
                        foreach (EntityEntry failedEnttry in dbUpdateConcurrencyEx.Entries)
                        {
                            failedEnttry.Reload();
                        }
                        return true;
                    }
                }
                catch (DbUpdateException dbUpdateEx)
                {
                    if (dbUpdateEx != null)
                    {
                        if (dbUpdateEx != null && dbUpdateEx.InnerException != null && dbUpdateEx.InnerException.InnerException != null)
                        {
                            SqlException sqlException = dbUpdateEx.InnerException.InnerException as SqlException;
                            if (sqlException != null)
                            {
                                switch (sqlException.Number)
                                {
                                    // Unique constraint error
                                    case 2627:
                                        throw new ConcurrencyException(sqlException.Message, sqlException);

                                    // Constraint check violation
                                    // Duplicated key row error
                                    case 547:
                                    case 2601:
                                        throw new ConstraintViolationException(sqlException.Message, sqlException);   // A custom exception of yours for concurrency issues

                                    default:
                                        // A custom exception of yours for other DB issues
                                        throw new DatabaseAccessException(sqlException.Message, sqlException);
                                }
                            }

                            throw new DatabaseAccessException(dbUpdateEx.Message, dbUpdateEx.InnerException);
                        }
                    }
                    throw;
                }
                catch (RetryLimitExceededException)
                {
                    throw;
                }
            }
            while (saveFailed && retryMax <= 3);
        }

        #endregion Save Changes Region

        #region Dispose region

        /// <summary>
        /// Releases all resources used by the Entities
        /// </summary>
        public void Dispose()
        {
            if (this.Context != null)
            {
                if (this.Context.Database != null && this.Context.Database.GetDbConnection() != null)
                    this.Context.Database.CloseConnection();

                this.Context.Dispose();
                this.Context = null;
            }
        }

        public System.Collections.Generic.IEnumerable<TResult> FindAll<TResult>(Expression<Func<TEntity, bool>> where = null, Expression<Func<TEntity, TResult>> select = null, Expression<Func<TEntity, object>> orderBy = null, bool? ascending = null, int? page = null, int? pageSize = null, params string[] includes)
        {
            throw new NotImplementedException();
        }

        #endregion Dispose region

        /// <summary>
        ///
        /// </summary>
        /// <param name="repository"></param>
        public static explicit operator TContext(EfRepository<TEntity, TContext> repository)
        {
            return repository.Context;
        }

        #endregion Methods
    }
}