using Microsoft.EntityFrameworkCore;

namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    public interface IMultiTenantEfRepositoryFactory : IMultiTenantRepositoryFactory
    {
        /// <summary>
        ///
        /// </summary>
        DbContext Context { get; }
    }
}