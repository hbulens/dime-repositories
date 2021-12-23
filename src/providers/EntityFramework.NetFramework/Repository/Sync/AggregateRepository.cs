using System;
using System.Linq;
using System.Linq.Expressions;

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
        public long Count()
        {
            using TContext ctx = Context;
            return ctx.Set<TEntity>().AsNoTracking().Count();
        }

        public long Count(Expression<Func<TEntity, bool>> where)
        {
            using TContext ctx = Context;
            return ctx.Set<TEntity>().AsNoTracking().Count(where);
        }
    }
}