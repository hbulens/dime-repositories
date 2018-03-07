namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IOrder<T>
    {
        /// <summary>
        ///
        /// </summary>
        bool IsAscending { get; set; }

        /// <summary>
        ///
        /// </summary>
        string Property { get; set; }
    }
}