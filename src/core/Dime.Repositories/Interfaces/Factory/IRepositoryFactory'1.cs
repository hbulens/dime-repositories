namespace Dime.Repositories
{
    public interface IRepositoryFactory<in TOpts> : IRepositoryFactory
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns>Repository instance for the collection type</returns>
        IRepository<T> Create<T>(TOpts opts) where T : class, new();
    }
}