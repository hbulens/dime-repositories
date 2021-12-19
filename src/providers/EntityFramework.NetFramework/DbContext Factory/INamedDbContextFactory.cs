using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Dime.Repositories
{
    /// <summary>
    /// Entity Framework context factory
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface INamedDbContextFactory<out TContext> : IDbContextFactory<TContext> where TContext : DbContext
    {
        /// <summary>
        /// Creates the context by name
        /// </summary>
        /// <param name="nameOrConnectionString">The name</param>
        /// <returns>The instantiated context</returns>
        TContext Create(string nameOrConnectionString);
    }
}