using System;

namespace Dime.Repositories
{
    /// <summary>
    /// Exception to indicate an error with concurrency
    /// </summary>
    [Serializable]
    public class ConcurrencyException : Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ConcurrencyException()
        {
        }

        /// <summary>
        /// Constructor accepting the message
        /// </summary>
        /// <param name="message">The exception message</param>
        public ConcurrencyException(string message) : base(message)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message">The exception message</param>
        /// <param name="innerException">The exception that was caught</param>
        public ConcurrencyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}