using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Dime.Repositories
{
    /// <summary>
    /// Entity Framework context factory
    /// </summary>
    /// <typeparam name="TContext">The DbContext type</typeparam>
    public interface IMultiTenantDbContextFactory<out TContext> : IDesignTimeDbContextFactory<TContext> where TContext : DbContext
    {
        /// <summary>
        /// Gets or sets the tenant
        /// </summary>
        string Tenant { get; set; }

        /// <summary>
        /// Gets or sets the connection
        /// </summary>
        string Connection { get; set; }

        /// <summary>
        /// Creates the context by name
        /// </summary>
        /// <param name="name">The name</param>
        /// <returns>The instantiated context</returns>
        TContext Create(string name);

        /// <summary>
        /// Creates the context with the provided options
        /// </summary>
        /// <param name="options">The factory options</param>
        /// <returns>The instantiated context</returns>
        TContext Create(DbContextFactoryOptions options);
    }
}