using System;

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
        /// Constructor accepting the message
        /// </summary>
        /// <param name="message">The exception message</param>
        /// <param name="innerException"></param>
        public DatabaseAccessException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}