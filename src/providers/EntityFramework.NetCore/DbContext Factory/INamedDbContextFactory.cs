using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Dime.Repositories
{
    /// <summary>
    /// Entity Framework context factory
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface INamedDbContextFactory<out TContext> : IDesignTimeDbContextFactory<TContext> where TContext : DbContext
    {
        /// <summary>
        /// Creates the context by name
        /// </summary>
        /// <param name="nameOrConnectionString">The name or connection string</param>
        /// <returns>The instantiated context</returns>
        TContext Create(string nameOrConnectionString);
    }
}