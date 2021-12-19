namespace Dime.Repositories
{
    public interface INamedRepositoryFactory : IRepositoryFactory
    {
        IRepository<T> Create<T>(string connection) where T : class, new();
    }
}
