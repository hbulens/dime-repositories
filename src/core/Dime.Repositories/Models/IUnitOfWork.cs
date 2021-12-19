namespace Dime.Repositories
{    
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : class, new();
        IRepository<T> GetRepository<T>(string connection) where T : class, new();
        IRepository<T> GetRepository<T>(string tenant, string connection) where T : class, new();
        bool SaveChanges();
        System.Threading.Tasks.Task<bool> SaveChangesAsync();
    }
}