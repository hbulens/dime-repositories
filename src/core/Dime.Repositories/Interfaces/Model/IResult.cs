using System.Collections.Generic;

namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IResult<T>
    {
        /// <summary>
        ///
        /// </summary>
        IEnumerable<T> Data { get; set; }

        /// <summary>
        ///
        /// </summary>
        string Message { get; set; }
    }
}