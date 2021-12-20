using System;

namespace Dime.Repositories
{
    [Obsolete("Will be removed in 2.0.0")]
    public interface IOrder<T>
    {        
        bool IsAscending { get; set; }
        
        string Property { get; set; }

        void Deconstruct(out string property, out bool isAscending);
    }
}