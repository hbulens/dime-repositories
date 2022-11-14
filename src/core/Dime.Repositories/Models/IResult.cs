using System.Collections.Generic;

namespace Dime.Repositories
{
    public interface IResult<T>
    {
        IEnumerable<T> Data { get; set; }
        string Message { get; set; }
    }
}