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
        #region Constructor

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
            this.Data = data;
            this.Summary = new List<dynamic>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        public Page(IEnumerable<T> data, int total) : this(data)
        {
            this.Total = total;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <param name="summary"></param>
        public Page(IEnumerable<T> data, int total, string message) : this(data, total)
        {
            this.Message = message;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <param name="summary"></param>
        /// <param name="total"></param>
        public Page(IEnumerable<T> data, int total, string message, IEnumerable<dynamic> summary) : this(data, total, message)
        {
            this.Summary = summary != null ? summary.ToList() : new List<dynamic>();
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
        public int Total { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///
        /// </summary>
        public List<dynamic> Summary { get; set; }

        #endregion Properties
    }
}