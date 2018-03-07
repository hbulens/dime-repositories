namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    public interface IRepositoryConfiguration
    {
        /// <summary>
        ///
        /// </summary>
        bool SaveInBatch { get; set; }

        /// <summary>
        ///
        /// </summary>
        ConcurrencyStrategy SaveStrategy { get; set; }
    }
}