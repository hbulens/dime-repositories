namespace Dime.Repositories
{
    /// <summary>
    /// Represents a repository configuration object
    /// </summary>
    public class RepositoryConfiguration : IMultiTenantRepositoryConfiguration
    {
        /// <summary>
        /// Gets or sets the identifier of the tenant
        /// </summary>
        public string Tenant { get; set; }

        /// <summary>
        /// Gets or sets the database connection
        /// </summary>
        public string Connection { get; set; }

        /// <summary>
        /// Gets or sets the flag to indicate whether to leverage the UOW pattern and save in batch
        /// </summary>
        public bool SaveInBatch { get; set; }

        /// <summary>
        /// Gets or sets the database save strategy
        /// </summary>
        public ConcurrencyStrategy SaveStrategy { get; set; }

        /// <summary>
        /// Gets or sets the flag to indicate whether to leverage the caching mechanism
        /// </summary>
        public bool Cached { get; set; }
    }
}