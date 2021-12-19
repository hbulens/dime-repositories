using System;

namespace Dime.Repositories
{
    [Obsolete("Will be removed in 2.0.0")]
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