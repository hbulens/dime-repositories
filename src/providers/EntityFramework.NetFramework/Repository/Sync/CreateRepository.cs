using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
        public virtual TEntity Create(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Added;
            TEntity createdItem = Context.Set<TEntity>().Add(entity);
            SaveChanges(Context);

            return createdItem;
        }

        public virtual TEntity Create(TEntity entity, Expression<Func<TEntity, bool>> condition)
        {
            Context.Entry(entity).State = EntityState.Added;
            TEntity createdItem = Context.Set<TEntity>().AddIfNotExists(entity, condition);
            SaveChanges(Context);

            return createdItem;
        }

        public virtual TEntity Create(TEntity entity, Func<TEntity, TContext, Task> beforeSaveAction)
        {
            beforeSaveAction(entity, Context);

            Context.Entry(entity).State = EntityState.Added;
            TEntity createdItem = Context.Set<TEntity>().Add(entity);
            SaveChanges(Context);

            return createdItem;
        }

        public virtual TEntity Create(TEntity entity, bool commit)
        {
            Context.Entry(entity).State = EntityState.Added;
            TEntity createdItem = Context.Set<TEntity>().Add(entity);

            if (commit)
                SaveChanges(Context);

            return createdItem;
        }

        public virtual IQueryable<TEntity> Create(IQueryable<TEntity> entities)
        {
            if (!entities.Any())
                return entities;

            List<TEntity> newEntities = new();
            List<TEntity> entitiesToCreate = entities.ToList();
            
            foreach (TEntity entity in entitiesToCreate)
            {
                Context.Entry(entity).State = EntityState.Added;
                TEntity newEntity = Context.Set<TEntity>().Add(entity);
                newEntities.Add(newEntity);
            }

            SaveChanges(Context);

            return newEntities.AsQueryable();
        }
    }
}