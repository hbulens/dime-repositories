using System;
using System.Runtime.Serialization;

namespace Dime.Repositories
{
    /// <summary>
    /// Exception to indicate an error with constraints such as PK and FK
    /// </summary>
    [Serializable]
    [Obsolete("Use shared project")]
    public class ConstraintViolationException : Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ConstraintViolationException()
        {
        }

        /// <summary>
        /// Constructor accepting the message
        /// </summary>
        /// <param name="message">The exception message</param>
        public ConstraintViolationException(string message) : base(message)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message">The exception message</param>
        /// <param name="innerException">The exception that was caught</param>
        public ConstraintViolationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info">SerializationInfo for the exception</param>
        /// <param name="context">The Streaming Context</param>
        protected ConstraintViolationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}