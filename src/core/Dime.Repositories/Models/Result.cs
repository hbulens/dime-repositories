using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Dime.Repositories
{

    [Obsolete("Will be removed in version 2.0.0")]
    [KnownType(typeof(Page<>))]
    public class Result<T> : IResult<T>
    {
        public IEnumerable<T> Data { get; set; }

        public string Message { get; set; }

        public Result()
        {
            Data = new HashSet<T>();
        }

        public Result(IEnumerable<T> data) : this()
        {
            Data = data;
        }

        public Result(IEnumerable<T> data, string message)
            : this(data)
        {
            Message = message;
        }
    }
}