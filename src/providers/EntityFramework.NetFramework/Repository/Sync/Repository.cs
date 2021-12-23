using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

namespace Dime.Repositories
{
    [ExcludeFromCodeCoverage]
    public partial class EfRepository<TEntity, TContext> : ISqlRepository<TEntity>
        where TEntity : class, new()
        where TContext : DbContext
    {
        public EfRepository(TContext dbContext)
            : this(dbContext, new RepositoryConfiguration())
        {
        }

        public EfRepository(TContext dbContext, RepositoryConfiguration configuration)
        {
            Context = dbContext;
            Configuration = configuration;
        }

        public EfRepository(RepositoryConfiguration configuration, INamedDbContextFactory<TContext> factory)
        {
            Configuration = configuration;
            Factory = factory;
        }

        private TContext _context;

        protected TContext Context
        {
            get => Factory?.Create(Configuration.Connection);
            set => _context = value;
        }

        private RepositoryConfiguration Configuration { get; }

        private INamedDbContextFactory<TContext> Factory { get; }

        public virtual bool SaveChanges(TContext context)
        {
            int retryMax;
            bool saveFailed;
            do
            {
                try
                {
                    if (Configuration.SaveInBatch)
                        return false;

                    int result = context.SaveChanges();
                    return 0 < result;
                }
                catch (DbEntityValidationException validationEx)
                {
                    Rethrow(validationEx);
                    return false;
                }
                catch (DbUpdateConcurrencyException dbUpdateConcurrencyEx)
                {
                    if (Configuration.SaveStrategy == ConcurrencyStrategy.ClientFirst)
                    {
                        foreach (DbEntityEntry failedEntry in dbUpdateConcurrencyEx.Entries)
                        {
                            DbPropertyValues dbValues = failedEntry.GetDatabaseValues();
                            if (dbValues == null)
                                continue;

                            failedEntry.OriginalValues.SetValues(dbValues);
                            return SaveChanges(context);
                        }
                        return true;
                    }

                    foreach (DbEntityEntry failedEntry in dbUpdateConcurrencyEx.Entries)
                        failedEntry.Reload();

                    return true;
                }
                catch (DbUpdateException dbUpdateEx)
                {
                    if (dbUpdateEx.InnerException?.InnerException == null)
                        throw;

                    if (dbUpdateEx.InnerException.InnerException is not SqlException sqlException)
                        throw new DatabaseAccessException(dbUpdateEx.Message, dbUpdateEx.InnerException);

                    throw sqlException.Number switch
                    {
                        2627 => new ConcurrencyException(sqlException.Message, sqlException),
                        547 => new ConstraintViolationException(sqlException.Message,
                            sqlException)
                        ,
                        2601 => new ConstraintViolationException(sqlException.Message,
                            sqlException)
                        ,
                        _ => new DatabaseAccessException(sqlException.Message, sqlException)
                    };
                }
            }
            while (saveFailed && retryMax <= 3);
        }

        public void Dispose()
        {
            if (Context == null)
                return;

            Context.Dispose();
            Context = null;
        }

        public static explicit operator TContext(EfRepository<TEntity, TContext> repository)
            => repository.Context;
    }
}