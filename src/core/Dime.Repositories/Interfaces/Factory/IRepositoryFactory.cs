namespace Dime.Repositories
{
    /// <summary>
    /// Definition for the factory that initiates repository instances
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns>Repository instance for the collection type</returns>
        IRepository<T> Create<T>() where T : class, new();
    }
}