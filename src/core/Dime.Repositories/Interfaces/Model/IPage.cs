namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPage<T> : IResult<T>
    {
        /// <summary>
        ///
        /// </summary>
        int Total { get; set; }
    }
}