using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Dime.Repositories
{
    /// <summary>
    /// Generic repository using Entity Framework
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    public partial class EfRepository<TEntity, TContext>
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
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
                        // Unique constraint error
                        2627 => new ConcurrencyException(sqlException.Message, sqlException),
                        // Constraint check violation
                        // Duplicated key row error
                        547 => new ConstraintViolationException(sqlException.Message,
                            sqlException) // A custom exception of yours for concurrency issues
                        ,
                        2601 => new ConstraintViolationException(sqlException.Message,
                            sqlException) // A custom exception of yours for concurrency issues
                        ,
                        _ => new DatabaseAccessException(sqlException.Message, sqlException)
                    };
                }
            }
            while (saveFailed && retryMax <= 3);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
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
                        // Unique constraint error
                        2627 => new ConcurrencyException(sqlException.Message, sqlException),
                        // Constraint check violation
                        // Duplicated key row error
                        547 => new ConstraintViolationException(sqlException.Message,
                            sqlException) // A custom exception of yours for concurrency issues
                        ,
                        2601 => new ConstraintViolationException(sqlException.Message,
                            sqlException) // A custom exception of yours for concurrency issues
                        ,
                        _ => new DatabaseAccessException(sqlException.Message, sqlException)
                    };
                }
            }
            while (saveFailed && retryMax <= 3);
        }

        private static void Rethrow(DbEntityValidationException ex)
        {
            // Retrieve the error messages as a list of strings.
            IEnumerable<string> errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

            // Join the list to a single string.
            string fullErrorMessage = string.Join("; ", errorMessages);

            // Combine the original exception message with the new one.
            string exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

            // Throw a new DbEntityValidationException with the improved exception message.
            throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
        }
    }
}