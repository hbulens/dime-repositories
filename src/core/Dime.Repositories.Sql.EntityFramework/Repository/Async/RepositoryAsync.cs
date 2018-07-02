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
            int retryMax = 0;
            bool saveFailed = false;
            do
            {
                try
                {
                    return !Configuration.SaveInBatch && 0 < await Context.SaveChangesAsync();
                }
                //catch (DbEntityValidationException validationEx)
                //{
                //    foreach (DbEntityValidationResult entityValidationResult in validationEx.EntityValidationErrors)
                //        foreach (DbValidationError validationError in entityValidationResult.ValidationErrors)
                //            Trace.WriteLine($"Property: \"{validationError.PropertyName}\", Error: \"{validationError.ErrorMessage}\"");

                //    throw;
                //}
                catch (DbUpdateConcurrencyException dbUpdateConcurrencyEx)
                {
                    if (Configuration.SaveStrategy == ConcurrencyStrategy.ClientFirst)
                    {
                        foreach (EntityEntry failedEntry in dbUpdateConcurrencyEx.Entries)
                        {
                            PropertyValues dbValues = failedEntry.GetDatabaseValues();
                            if (dbValues == null)
                                continue;

                            failedEntry.OriginalValues.SetValues(dbValues);
                            return await SaveChangesAsync();
                        }
                        return true;
                    }
                    else
                    {
                        foreach (EntityEntry failedEnttry in dbUpdateConcurrencyEx.Entries)
                            await failedEnttry.ReloadAsync();

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
                    if (!Configuration?.SaveInBatch ?? true)
                    {
                        int result = await context.SaveChangesAsync();
                        return 0 < result;
                    }
                    else
                        return false;
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
                        bool retried = false;
                        foreach (EntityEntry failedEntry in dbUpdateConcurrencyEx.Entries)
                        {
                            PropertyValues dbValues = failedEntry.GetDatabaseValues();
                            if (dbValues == null)
                                continue;

                            retried = true;
                            failedEntry.OriginalValues.SetValues(dbValues);
                            return await SaveChangesAsync(context);
                        }

                        if (!retried)
                            throw;

                        return retried;
                    }
                    else
                    {
                        foreach (EntityEntry failedEnttry in dbUpdateConcurrencyEx.Entries)
                            await failedEnttry.ReloadAsync();

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
    }
}