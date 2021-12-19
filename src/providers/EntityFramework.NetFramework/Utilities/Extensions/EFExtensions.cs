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
            _workspace ??= context.ObjectContext.MetadataWorkspace;
            _itemCollection ??= (ObjectItemCollection)_workspace.GetItemCollection(DataSpace.OSpace);

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

            List<string> includeList = new();
            if (includes.Any())
                return includes.Where(x => !string.IsNullOrEmpty(x) && !includeList.Contains(x))
                    .Aggregate(query, (current, include) => current.Include(include));

            ReadOnlyMetadataCollection<NavigationProperty> navigationProperties = GetEntityType<TEntity>(context)?.NavigationProperties;
            if (navigationProperties == null)
                return query;

            foreach (NavigationProperty navigationProperty in
                navigationProperties.Where(navigationProperty => !includeList.Contains(navigationProperty.Name)))
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

            return GetEntityType<TEntity>(context)
                .NavigationProperties
                .Aggregate(query, (current, navigationProperty) => current.Include(navigationProperty.Name));
        }
    }
}