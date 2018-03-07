namespace Dime.Repositories
{
    /// <summary>
    /// Contract designed to generate a repository for the type
    /// </summary>
    public interface IMultiTenantRepositoryConfiguration : IRepositoryConfiguration
    {
        /// <summary>
        ///
        /// </summary>
        string Connection { get; set; }

        /// <summary>
        ///
        /// </summary>
        string Tenant { get; set; }
    }
}