using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [KnownType(typeof(Page<>))]
    public class Result<T> : IResult<T>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public Result()
        {
            this.Data = new HashSet<T>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        public Result(IEnumerable<T> data) : this()
        {
            this.Data = data;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        public Result(IEnumerable<T> data, string message) : this(data)
        {
            this.Message = message;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<T> Data { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Message { get; set; }

        #endregion Properties
    }
}