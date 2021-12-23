using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
        public Task<long> CountAsync()
        {
            using TContext ctx = Context;
            long count = ctx.Count<TEntity>();
            return Task.FromResult(count);
        }

        public Task<long> CountAsync(Expression<Func<TEntity, bool>> where)
        {
            using TContext ctx = Context;
            long count = ctx.Count(where);
            return Task.FromResult(count);
        }
    }
}