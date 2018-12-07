namespace Dime.Repositories
{
    /// <summary>
    /// Contract designed to generate a repository for the type
    /// </summary>
    public interface IMultiTenantRepositoryConfiguration : IRepositoryConfiguration
    {
        /// <summary>
        /// Gets or sets the connection
        /// </summary>
        string Connection { get; set; }

        /// <summary>
        /// Gets or sets the tenant
        /// </summary>
        string Tenant { get; set; }
    }
}