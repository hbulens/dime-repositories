using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Dime.Repositories
{
    [ExcludeFromCodeCoverage]
    internal static class EFExtensions
    {
        private static MetadataWorkspace _workspace;
        private static ObjectItemCollection _itemCollection;

        private static EntityType GetEntityType<T>(IObjectContextAdapter context)
        {
            if (_workspace == default(MetadataWorkspace))
                _workspace = context.ObjectContext.MetadataWorkspace;

            if (_itemCollection == default(ObjectItemCollection))
                _itemCollection = (ObjectItemCollection)_workspace.GetItemCollection(DataSpace.OSpace);

            EntityType entityType = _itemCollection.OfType<EntityType>().FirstOrDefault(e => _itemCollection.GetClrType(e) == typeof(T));
            return entityType;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="includes"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        internal static IQueryable<TEntity> Include<TEntity>(this IQueryable<TEntity> query, DbContext context, params string[] includes)
            where TEntity : class
        {
            // Don't eagerly load navigation properties when the includes parameter has been explicitly set to null
            if (includes == null)
                return query;

            List<string> includeList = new List<string>();
            if (includes.Any())
                return includes.Where(x => !string.IsNullOrEmpty(x) && !includeList.Contains(x))
                    .Aggregate(query, (current, include) => current.Include(include));

            ReadOnlyMetadataCollection<NavigationProperty> navigationProperties = GetEntityType<TEntity>(context)?.NavigationProperties;
            if (navigationProperties == null)
                return query;

            foreach (NavigationProperty navigationProperty in navigationProperties.Where(navigationProperty => !includeList.Contains(navigationProperty.Name)))
            {
                includeList.Add(navigationProperty.Name);
                query = query.Include(navigationProperty.Name);
            }

            return query;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="includes"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        internal static IQueryable<TResult> IncludeView<TEntity, TResult>(this IQueryable<TResult> query, DbContext context, params string[] includes) where TEntity : class
        {
            if (includes != null && includes.Any())
                return includes.Where(include => include != null)
                    .Aggregate(query, (current, include) => current.Include(include));

            foreach (NavigationProperty navigationProperty in GetEntityType<TEntity>(context).NavigationProperties)
                query = query.Include(navigationProperty.Name);

            return query;
        }

        /// <summary>
        /// Utility to automate including related entities in an eagerly fashion
        /// </summary>
        /// <typeparam name="TResult">The projected entity</typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query">The query</param>
        /// <param name="context"></param>
        /// <param name="includes">The optional list of related entities that should be eagerly loaded</param>
        /// <returns>The original query enriched by related entities</returns>
        internal static IQueryable<TResult> Include<TEntity, TResult>(IQueryable<TResult> query, DbContext context, params string[] includes)
        {
            if (includes == null)
                return query;

            if (includes.Any())
                return includes
                    .Where(include => include != null)
                    .Aggregate(query, (current, include) => current.Include(include));

            MetadataWorkspace workspace = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;
            ObjectItemCollection itemCollection =
                (ObjectItemCollection)workspace.GetItemCollection(DataSpace.OSpace);
            EntityType entityType = itemCollection.OfType<EntityType>()
                .Single(e => itemCollection.GetClrType(e) == typeof(TEntity));

            return entityType.NavigationProperties.Aggregate(query, (current, navigationProperty) => current.Include(navigationProperty.Name));
        }
    }
}