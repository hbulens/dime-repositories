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
        TContext Create(string nameOrConnectionString);
    }
}