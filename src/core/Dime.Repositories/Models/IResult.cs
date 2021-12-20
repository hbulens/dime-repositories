using System;
using System.Collections.Generic;

namespace Dime.Repositories
{
    [Obsolete("Will be removed in version 2.0.0")]
    public interface IResult<T>
    {
        IEnumerable<T> Data { get; set; }
        string Message { get; set; }
    }
}