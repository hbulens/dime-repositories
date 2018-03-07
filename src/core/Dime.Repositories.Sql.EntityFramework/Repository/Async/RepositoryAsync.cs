using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.SqlClient;
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
                    if (!this.Configuration.SaveInBatch)
                        return 0 < await this.Context.SaveChangesAsync();
                    else
                        return false;
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
                                return await this.SaveChangesAsync();
                            }
                        }
                        return true;
                    }
                    else
                    {
                        foreach (EntityEntry failedEnttry in dbUpdateConcurrencyEx.Entries)
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
                    if (!this.Configuration.SaveInBatch)
                    {
                        int result = await context.SaveChangesAsync();
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
                        bool retried = false;
                        foreach (EntityEntry failedEnttry in dbUpdateConcurrencyEx.Entries)
                        {
                            var dbValues = failedEnttry.GetDatabaseValues();
                            if (dbValues != null)
                            {
                                retried = true;
                                failedEnttry.OriginalValues.SetValues(dbValues);
                                return await this.SaveChangesAsync(context);
                            }
                        }

                        if (!retried)
                            throw dbUpdateConcurrencyEx;

                        return retried;
                    }
                    else
                    {
                        foreach (EntityEntry failedEnttry in dbUpdateConcurrencyEx.Entries)
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
    }
}