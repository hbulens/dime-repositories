using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dime.Repositories.Redis
{
    public partial class RedisRepository<T>
    {
        public Task<T> UpdateAsync(T entity, params Func<T, object>[] properties)
        {
            throw new NotImplementedException();
        }

        public Task<T> UpdateAsync(T entity, bool commitChanges = true)
        {
            throw new NotImplementedException();
        }

        public Task<T> UpdateAsync(T entity, params Expression<Func<T, object>>[] properties)
        {
            throw new NotImplementedException();
        }

        public Task<T> UpdateAsync(T entity, params string[] properties)
        {
            throw new NotImplementedException();
        }

        public T Update(T entity, bool commitChanges = true)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(IEnumerable<T> entities, bool commitChanges = true)
        {
            throw new NotImplementedException();
        }

        public T Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}