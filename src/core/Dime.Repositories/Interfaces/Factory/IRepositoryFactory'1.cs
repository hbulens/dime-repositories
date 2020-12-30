namespace Dime.Repositories
{
    public interface IRepositoryFactory<in TOpts>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Repository instance for the collection type</returns>
        IRepository<T> Create<T>(TOpts opts) where T : class, new();
    }
}