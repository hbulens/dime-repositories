namespace Dime.Repositories
{
    /// <summary>
    /// Represents a configuration object
    /// </summary>
    public interface IRepositoryConfiguration
    {
        /// <summary>
        /// Gets or sets the batch flag
        /// </summary>
        bool SaveInBatch { get; set; }

        /// <summary>
        /// Gets or sets the concurrency strategy
        /// </summary>
        ConcurrencyStrategy SaveStrategy { get; set; }
    }
}