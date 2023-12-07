using System;
using System.Diagnostics.CodeAnalysis;

namespace Dime.Repositories
{
    [Serializable]
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
    }
}