using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
        public virtual async Task<bool> SaveChangesAsync()
        {
            int retryMax = 0;
            bool saveFailed = false;
            do
            {
                try
                {
                    return !Configuration.SaveInBatch && 0 < await Context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (DbEntityValidationException ex)
                {
                    Rethrow(ex);
                    return false;
                }
                catch (DbUpdateConcurrencyException dbUpdateConcurrencyEx)
                {
                    if (Configuration.SaveStrategy == ConcurrencyStrategy.ClientFirst)
                    {
                        foreach (DbEntityEntry failedEntry in dbUpdateConcurrencyEx.Entries)
                        {
                            if (failedEntry.State == EntityState.Deleted)
                            {
                                failedEntry.State = EntityState.Detached;
                                continue;
                            }

                            DbPropertyValues dbValues = failedEntry.GetDatabaseValues();
                            if (dbValues == null)
                                continue;

                            failedEntry.OriginalValues.SetValues(dbValues);
                            return await SaveChangesAsync().ConfigureAwait(false);
                        }
                        return true;
                    }
                    else
                    {
                        foreach (DbEntityEntry failedEnttry in dbUpdateConcurrencyEx.Entries)
                            await failedEnttry.ReloadAsync().ConfigureAwait(false);

                        return true;
                    }
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

        public virtual async Task<bool> SaveChangesAsync(TContext context)
        {
            int retryMax;
            bool saveFailed;
            do
            {
                try
                {
                    if (!Configuration.SaveInBatch)
                    {
                        int result = context.SaveChanges();
                        return 0 < result;
                    }
                    else
                        return false;
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
                        bool retried = false;
                        foreach (DbEntityEntry failedEntry in dbUpdateConcurrencyEx.Entries)
                        {
                            if (failedEntry.State == EntityState.Deleted)
                            {
                                failedEntry.State = EntityState.Detached;
                                retried = true;
                                continue;
                            }

                            DbPropertyValues dbValues = failedEntry.GetDatabaseValues();
                            if (dbValues == null)
                                continue;

                            retried = true;
                            failedEntry.OriginalValues.SetValues(dbValues);
                            return await SaveChangesAsync(context).ConfigureAwait(false);
                        }

                        if (!retried)
                            throw;

                        return retried;
                    }
                    else
                    {
                        foreach (DbEntityEntry failedEnttry in dbUpdateConcurrencyEx.Entries)
                            await failedEnttry.ReloadAsync().ConfigureAwait(false);

                        return true;
                    }
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

        private static void Rethrow(DbEntityValidationException ex)
        {
            IEnumerable<string> errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

            string fullErrorMessage = string.Join("; ", errorMessages);

            string exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

            throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
        }
    }
}