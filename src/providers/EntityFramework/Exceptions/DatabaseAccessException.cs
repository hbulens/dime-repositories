using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Dime.Repositories
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class DatabaseAccessException : Exception
    {
        public DatabaseAccessException()
        {
        }

        public DatabaseAccessException(string message)
            : base(message)
        {
        }

        public DatabaseAccessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected DatabaseAccessException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}