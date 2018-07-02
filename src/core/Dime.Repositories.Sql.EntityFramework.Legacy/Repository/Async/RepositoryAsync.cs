using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Dime.Repositories
{
    /// <summary>
    /// Generic repository using Entity Framework
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
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
                    if (!Configuration.SaveInBatch)
                        return 0 < await Context.SaveChangesAsync();
                    else
                        return false;
                }
                catch (DbEntityValidationException validationEx)
                {
                    foreach (DbEntityValidationResult entityValidationResult in validationEx.EntityValidationErrors)
                    {
                        string entityType = entityValidationResult.Entry.Entity.GetType().Name;
                        EntityState entityState = entityValidationResult.Entry.State;

                        foreach (DbValidationError validationError in entityValidationResult.ValidationErrors)
                        {
                            Debug.WriteLine("Property: \"{0}\", Error: \"{1}\"", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                    throw;
                }
                catch (DbUpdateConcurrencyException dbUpdateConcurrencyEx)
                {
                    if (Configuration.SaveStrategy == ConcurrencyStrategy.ClientFirst)
                    {
                        foreach (DbEntityEntry failedEnttry in dbUpdateConcurrencyEx.Entries)
                        {
                            var dbValues = failedEnttry.GetDatabaseValues();
                            if (dbValues != null)
                            {
                                failedEnttry.OriginalValues.SetValues(dbValues);
                                return await SaveChangesAsync();
                            }
                        }
                        return true;
                    }
                    else
                    {
                        foreach (DbEntityEntry failedEnttry in dbUpdateConcurrencyEx.Entries)
                        {
                            await failedEnttry.ReloadAsync();
                        }
                        return true;
                    }
                }
                catch (DbUpdateException dbUpdateEx)
                {
                    if (dbUpdateEx != null)
                    {
                        if (dbUpdateEx?.InnerException != null && dbUpdateEx.InnerException.InnerException != null)
                        {
                            if (dbUpdateEx.InnerException.InnerException is SqlException sqlException)
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

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual async Task<bool> SaveChangesAsync(TContext context)
        {
            int retryMax = 0;
            bool saveFailed = false;
            do
            {
                try
                {
                    if (!Configuration.SaveInBatch)
                    {
                        int result = await context.SaveChangesAsync();
                        return 0 < result;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (DbEntityValidationException validationEx)
                {
                    foreach (DbEntityValidationResult entityValidationResult in validationEx.EntityValidationErrors)
                    {
                        string entityType = entityValidationResult.Entry.Entity.GetType().Name;
                        EntityState entityState = entityValidationResult.Entry.State;

                        foreach (DbValidationError validationError in entityValidationResult.ValidationErrors)
                        {
                            Debug.WriteLine("Property: \"{0}\", Error: \"{1}\"", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                    throw;
                }
                catch (DbUpdateConcurrencyException dbUpdateConcurrencyEx)
                {
                    if (Configuration.SaveStrategy == ConcurrencyStrategy.ClientFirst)
                    {
                        bool retried = false;
                        foreach (DbEntityEntry failedEnttry in dbUpdateConcurrencyEx.Entries)
                        {
                            var dbValues = failedEnttry.GetDatabaseValues();
                            if (dbValues != null)
                            {
                                retried = true;
                                failedEnttry.OriginalValues.SetValues(dbValues);
                                return await SaveChangesAsync(context);
                            }
                        }

                        if (!retried)
                            throw dbUpdateConcurrencyEx;

                        return retried;
                    }
                    else
                    {
                        foreach (DbEntityEntry failedEnttry in dbUpdateConcurrencyEx.Entries)
                        {
                            await failedEnttry.ReloadAsync();
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
                            if (dbUpdateEx.InnerException.InnerException is SqlException sqlException)
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
    }
}