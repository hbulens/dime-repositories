using System.Diagnostics.CodeAnalysis;

namespace Dime.Repositories
{
    /// <summary>
    /// Represents a repository configuration object
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RepositoryConfiguration
    {
        /// <summary>
        /// Gets or sets the flag to indicate whether to leverage the UOW pattern and save in batch
        /// </summary>
        public bool SaveInBatch { get; set; }

        /// <summary>
        /// Gets or sets the database save strategy
        /// </summary>
        public ConcurrencyStrategy SaveStrategy { get; set; } = ConcurrencyStrategy.ClientFirst;
    }
}