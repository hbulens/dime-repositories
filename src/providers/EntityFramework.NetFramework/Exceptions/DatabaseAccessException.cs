using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Dime.Repositories
{
    /// <summary>
    /// Exception to indicate a general error with the database
    /// </summary>
    [Serializable]
    [Obsolete("Use shared project")]
    [ExcludeFromCodeCoverage]
    public class DatabaseAccessException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the System.Exception class.
        /// </summary>
        public DatabaseAccessException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified error message
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public DatabaseAccessException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified error
        /// message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public DatabaseAccessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified error
        /// message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="info">SerializationInfo for the exception</param>
        /// <param name="context">The Streaming Context</param>
        protected DatabaseAccessException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}