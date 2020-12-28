namespace Dime.Repositories
{
    /// <summary>
    /// Contract designed to generate a tenant specific repository
    /// </summary>
    public interface IMultiTenantRepositoryFactory : IRepositoryFactory
    {
        /// <summary>
        ///
        /// </summary>
        IMultiTenantRepositoryConfiguration RepositoryConfiguration { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <returns></returns>
        IRepository<T> Create<T>(string connection) where T : class, new();

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tenant"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        IRepository<T> Create<T>(string tenant, string connection) where T : class, new();
    }
}