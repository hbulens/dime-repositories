using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Dime.Repositories
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class ConstraintViolationException : Exception
    {
        public ConstraintViolationException()
        {
        }

        public ConstraintViolationException(string message) : base(message)
        {
        }

        public ConstraintViolationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConstraintViolationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}