using System;
using System.Runtime.Serialization;

namespace Dime.Repositories
{
    /// <summary>
    /// Exception to indicate a general error with the database
    /// </summary>
    [Serializable]
    public class DatabaseAccessException : Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public DatabaseAccessException()
        {
        }

        /// <summary>
        /// Constructor accepting the message
        /// </summary>
        /// <param name="message">The exception message</param>
        public DatabaseAccessException(string message) : base(message)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message">The exception message</param>
        /// <param name="innerException">The exception that was caught</param>
        public DatabaseAccessException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info">SerializationInfo for the exception</param>
        /// <param name="context">The Streaming Context</param>
        protected DatabaseAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}