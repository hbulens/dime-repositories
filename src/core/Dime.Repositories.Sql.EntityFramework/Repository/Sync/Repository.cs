using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Dime.Repositories
{
    /// <summary>
    /// Represents a repository with Entity Framework as the backbone for connecting to SQL databases
    /// </summary>
    /// <typeparam name="TEntity">The domain model that is registered in the underlying DbContext</typeparam>
    /// <typeparam name="TContext"></typeparam>
    public partial class EfRepository<TEntity, TContext> : ISqlRepository<TEntity>
        where TEntity : class, new()
        where TContext : DbContext
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EfRepository{TEntity,TContext}"/> class
        /// </summary>
        /// <param name="dbContext">The DbContext instance</param>
        public EfRepository(TContext dbContext)
        {
            Context = dbContext;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EfRepository{TEntity,TContext}"/> class
        /// </summary>
        /// <param name="dbContext">The DbContext instance</param>
        /// <param name="configuration">Repository behavior configuration</param>
        public EfRepository(TContext dbContext, IMultiTenantRepositoryConfiguration configuration)
        {
            Context = dbContext;
            Configuration = configuration;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EfRepository{TEntity,TContext}"/> class
        /// </summary>
        /// <param name="dbContextFactory">Context factory</param>
        public EfRepository(IMultiTenantDbContextFactory<TContext> dbContextFactory)
        {
            Factory = dbContextFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EfRepository{TEntity,TContext}"/> class
        /// </summary>
        /// <param name="dbContextFactory">Context factory</param>
        /// <param name="configuration">Repository behavior configuration</param>
        public EfRepository(IMultiTenantDbContextFactory<TContext> dbContextFactory, IMultiTenantRepositoryConfiguration configuration)
        : this(dbContextFactory)
        {
            Configuration = configuration;
        }

        #endregion Constructor

        #region Properties

        private TContext _context;

        protected TContext Context
        {
            get => _context ?? Factory.Create(Factory.Connection);
            set => _context = value;
        }

        private IMultiTenantDbContextFactory<TContext> Factory { get; }
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
                    if (!Configuration?.SaveInBatch ?? true)
                    {
                        int result = context.SaveChanges();
                        return 0 < result;
                    }
                    else
                    {
                        return false;
                    }
                }
                //catch (DbEntityValidationException validationEx)
                //{
                //    foreach (DbEntityValidationResult entityValidationResult in validationEx.EntityValidationErrors)
                //        foreach (DbValidationError validationError in entityValidationResult.ValidationErrors)
                //            Debug.WriteLine("Property: \"{0}\", Error: \"{1}\"", validationError.PropertyName, validationError.ErrorMessage);

                //    throw;
                //}
                catch (DbUpdateConcurrencyException dbUpdateConcurrencyEx)
                {
                    if (Configuration.SaveStrategy == ConcurrencyStrategy.ClientFirst)
                    {
                        foreach (EntityEntry failedEnttry in dbUpdateConcurrencyEx.Entries)
                        {
                            PropertyValues dbValues = failedEnttry.GetDatabaseValues();
                            if (dbValues == null)
                                continue;

                            failedEnttry.OriginalValues.SetValues(dbValues);
                            return SaveChanges(context);
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
                    if (dbUpdateEx.InnerException?.InnerException == null)
                        throw;

                    if (!(dbUpdateEx.InnerException.InnerException is SqlException sqlException))
                        throw new DatabaseAccessException(dbUpdateEx.Message, dbUpdateEx.InnerException);

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

                    throw new DatabaseAccessException(dbUpdateEx.Message, dbUpdateEx.InnerException);
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
            if (Context == null)
                return;

            //Context.Database?.GetDbConnection()?.Dispose();
            Context.Dispose();
            Context = null;
        }

        #endregion Dispose region

        /// <summary>
        ///
        /// </summary>
        /// <param name="repository"></param>
        public static explicit operator TContext(EfRepository<TEntity, TContext> repository)
            => repository.Context;

        #endregion Methods
    }
}