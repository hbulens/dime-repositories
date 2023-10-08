using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
        public long Count()
        {
            using TContext ctx = Context;
            int count = ctx.Set<TEntity>().AsNoTracking().Count();

            return count;
        }

        public long Count(Expression<Func<TEntity, bool>> where)
        {
            using TContext ctx = Context;
            int count = ctx.Set<TEntity>().AsNoTracking().Count(where);

            return count;
        }
    }
}