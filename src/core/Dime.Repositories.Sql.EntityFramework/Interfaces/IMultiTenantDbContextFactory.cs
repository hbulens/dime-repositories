using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IMultiTenantDbContextFactory<TContext> : IDesignTimeDbContextFactory<TContext> where TContext : DbContext
    {
        /// <summary>
        ///
        /// </summary>
        string Tenant { get; set; }

        /// <summary>
        ///
        /// </summary>
        string Connection { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        TContext Create(string name);

        /// <summary>
        ///
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        TContext Create(DbContextFactoryOptions options);
    }
}