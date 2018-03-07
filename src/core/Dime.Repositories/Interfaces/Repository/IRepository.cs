namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public partial interface IRepository<TEntity>
    {
        /// <summary>
        /// The configuration property
        /// </summary>
        IMultiTenantRepositoryConfiguration Configuration { get; set; }
    }
}