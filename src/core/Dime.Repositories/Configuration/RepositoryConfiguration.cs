namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    public class RepositoryConfiguration : IMultiTenantRepositoryConfiguration
    {
        /// <summary>
        /// The identifier of the tenant
        /// </summary>
        public string Tenant { get; set; }

        /// <summary>
        /// The database connection
        /// </summary>
        public string Connection { get; set; }

        /// <summary>
        /// Flag to indicate whether to leverage the UOW pattern and save in batch
        /// </summary>
        public bool SaveInBatch { get; set; }

        /// <summary>
        /// The database save strategy
        /// </summary>
        public ConcurrencyStrategy SaveStrategy { get; set; }

        /// <summary>
        /// Flag to indicate whether to leverage the caching mechanism
        /// </summary>
        public bool Cached { get; set; }
    }
}