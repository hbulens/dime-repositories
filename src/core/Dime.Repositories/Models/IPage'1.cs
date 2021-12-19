using System;

namespace Dime.Repositories
{
    [Obsolete("Will be removed in 2.0.0")]
    public interface IPage<T> : IResult<T>
    {
        int Total
        {
            get;
            set;
        }
    }
}