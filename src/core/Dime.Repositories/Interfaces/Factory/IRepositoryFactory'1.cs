namespace Dime.Repositories
{
    public interface IRepositoryFactory<in TOpts> : IRepositoryFactory
    {
        IRepository<T> Create<T>(TOpts opts) where T : class, new();
    }
}