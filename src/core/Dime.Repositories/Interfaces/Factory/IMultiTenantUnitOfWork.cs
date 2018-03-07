namespace Dime.Repositories
{
    /// <summary>
    /// Unit of Work interface that obligates the retrieval and storage of repositories
    /// </summary>
    public interface IMultiTenantUnitOfWork
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tenant"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        IRepository<T> GetRepository<T>(string tenant, string connection) where T : class, new();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        bool Save();
    }
}