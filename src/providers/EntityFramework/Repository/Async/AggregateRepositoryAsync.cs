using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
        public Task<long> CountAsync()
            => CountAsync(null);

        public Task<long> CountAsync(Expression<Func<TEntity, bool>> where)
        {
            long count;
            using (TContext ctx = Context)
                count = ctx.Count(where);

            return Task.FromResult((long)count);
        }
    }
}