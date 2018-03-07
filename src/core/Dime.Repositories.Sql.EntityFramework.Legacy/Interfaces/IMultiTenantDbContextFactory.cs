using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IMultiTenantDbContextFactory<TContext> : IDbContextFactory<TContext> where TContext : DbContext
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
    }
}