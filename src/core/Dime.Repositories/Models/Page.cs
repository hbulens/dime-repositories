using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [KnownType(typeof(Page<>))]
    public class Page<T> : IPage<T>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public Page()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        public Page(IEnumerable<T> data)
        {
            Data = data;
            Summary = new List<dynamic>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <param name="total"></param>
        public Page(IEnumerable<T> data, int total)
            : this(data)
        {
            Total = total;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <param name="total"></param>
        /// <param name="message"></param>
        public Page(IEnumerable<T> data, int total, string message)
            : this(data, total)
        {
            Message = message;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <param name="summary"></param>
        /// <param name="total"></param>
        public Page(IEnumerable<T> data, int total, string message, IEnumerable<dynamic> summary)
            : this(data, total, message)
        {
            Summary = summary != null ? summary.ToList() : new List<dynamic>();
        }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<T> Data { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int Total { get; set;  }

        /// <summary>
        ///
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///
        /// </summary>
        public List<dynamic> Summary { get; }
    }
}