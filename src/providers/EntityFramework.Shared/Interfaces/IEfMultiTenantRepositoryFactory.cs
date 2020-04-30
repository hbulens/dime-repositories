#if NET461
using System.Data.Entity;
#else
using Microsoft.EntityFrameworkCore;
#endif

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