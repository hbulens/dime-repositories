#if NET461
using System.Data.Entity
#else
using Microsoft.EntityFrameworkCore;
#endif

namespace Dime.Repositories
{
    public interface INamedEfRepositoryFactory : INamedRepositoryFactory
    {
        DbContext Context { get; set; }
    }
}