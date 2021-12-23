using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Dime.Repositories
{
    [Serializable]
    [Obsolete("Use shared project")]
    [ExcludeFromCodeCoverage]
    public class ConcurrencyException : Exception
    {
        public ConcurrencyException()
        {
        }

        public ConcurrencyException(string message) : base(message)
        {
        }

        public ConcurrencyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConcurrencyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}