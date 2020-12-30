using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
            int retryMax;
            bool saveFailed;
            do
            {
                try
                {
                    return !Configuration.SaveInBatch && 0 < await Context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (DbUpdateConcurrencyException dbUpdateConcurrencyEx)
                {
                    if (Configuration.SaveStrategy == ConcurrencyStrategy.ClientFirst)
                    {
                        foreach (EntityEntry failedEntry in dbUpdateConcurrencyEx.Entries)
                        {
                            PropertyValues dbValues = await failedEntry.GetDatabaseValuesAsync();
                            if (dbValues == null)
                                continue;

                            failedEntry.OriginalValues.SetValues(dbValues);
                            return await SaveChangesAsync(Context);
                        }
                        return true;
                    }

                    foreach (EntityEntry failedEntry in dbUpdateConcurrencyEx.Entries) await failedEntry.ReloadAsync();
                    return true;
                }
                catch (DbUpdateException dbUpdateEx)
                {
                    if (dbUpdateEx.InnerException?.InnerException == null)
                        throw;

                    if (!(dbUpdateEx.InnerException.InnerException is SqlException sqlException))
                        throw new DatabaseAccessException(dbUpdateEx.Message, dbUpdateEx.InnerException);

                    throw sqlException.Number switch
                    {
                        // Unique constraint error
                        2627 => (Exception)new ConcurrencyException(sqlException.Message, sqlException),
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
            int retryMax = 0;
            bool saveFailed = false;
            do
            {
                try
                {
                    if (!((!Configuration?.SaveInBatch) ?? true)) 
                        return false;

                    int result = await context.SaveChangesAsync().ConfigureAwait(false);
                    return 0 < result;
                }
                catch (DbUpdateConcurrencyException dbUpdateConcurrencyEx)
                {
                    if (Configuration.SaveStrategy == ConcurrencyStrategy.ClientFirst)
                    {
                        foreach (EntityEntry failedEntry in dbUpdateConcurrencyEx.Entries)
                        {
                            PropertyValues dbValues = await failedEntry.GetDatabaseValuesAsync();
                            if (dbValues == null)
                                continue;

                            failedEntry.OriginalValues.SetValues(dbValues);
                            return await SaveChangesAsync(Context);
                        }
                        return true;
                    }

                    foreach (EntityEntry failedEntry in dbUpdateConcurrencyEx.Entries) await failedEntry.ReloadAsync();
                    return true;
                }
                catch (DbUpdateException dbUpdateEx)
                {
                    if (dbUpdateEx.InnerException?.InnerException == null)
                        throw;

                    if (!(dbUpdateEx.InnerException.InnerException is SqlException sqlException))
                        throw new DatabaseAccessException(dbUpdateEx.Message, dbUpdateEx.InnerException);

                    throw sqlException.Number switch
                    {
                        // Unique constraint error
                        2627 => (Exception)new ConcurrencyException(sqlException.Message, sqlException),
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
    }
}