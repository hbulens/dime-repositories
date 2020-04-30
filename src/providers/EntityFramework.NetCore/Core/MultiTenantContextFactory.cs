using System;
using System.Collections.Concurrent;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    [ExcludeFromCodeCoverage]
    public abstract class MultiTenantContextFactory<TContext> : IMultiTenantDbContextFactory<TContext> where TContext : DbContext
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiTenantContextFactory{TContext}"/> class
        /// </summary>
        protected MultiTenantContextFactory()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiTenantContextFactory{TContext}"/> class
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        protected MultiTenantContextFactory(string connectionString) : this()
        {
            Connection = connectionString;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="tenant"></param>
        protected MultiTenantContextFactory(string connectionString, string tenant) : this(connectionString)
        {
            Tenant = tenant;
        }

        #endregion Constructor

        #region Properties

        public string Connection { get; set; }
        public string Tenant { get; set; }

        #endregion Properties

        /// <summary>
        /// Creates the instance of <typeparamref name="TContext"/> with the default settings
        /// </summary>
        /// <returns></returns>
        public virtual TContext Create()
            => !string.IsNullOrEmpty(Tenant) && !string.IsNullOrEmpty(Connection) ?
                Create(Tenant, Connection) :
                Create("dbo", Connection);

        /// <summary>
        /// Creates the specified connection.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <returns></returns>
        public virtual TContext Create(string connection) => Create("dbo", connection);

        /// <summary>
        /// Creates the specified tenant.
        /// </summary>
        /// <param name="tenant">The tenant.</param>
        /// <param name="connection">The connection.</param>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual TContext Create(string tenant, string connection, string context)
        {
            //if (string.IsNullOrEmpty(context))
            //    return Create(tenant, connection);

            //SqlConnectionFactory connectionFactory = new SqlConnectionFactory();
            //DbConnection dbConnection = connectionFactory.CreateConnection(connection);
            //Database.SetInitializer<TContext>(null);

            //DbCompiledModel compiledModel = NamedModelCache.GetOrAdd(
            //    Tuple.Create(tenant, dbConnection.ConnectionString, context),
            //    t => GetContextModel(dbConnection, tenant));

            //return ConstructContext(dbConnection, compiledModel, false);

            return default;
        }

        /// <summary>
        /// Creates the specified tenant.
        /// </summary>
        /// <param name="tenant">The tenant.</param>
        /// <param name="connection">The connection.</param>
        /// <returns></returns>
        public virtual TContext Create(string tenant, string connection)
        {
            //SqlConnectionFactory connectionFactory = new SqlConnectionFactory();
            //DbConnection dbConnection = connectionFactory.CreateConnection(connection);
            //Database.SetInitializer<TContext>(null);

            //DbCompiledModel compiledModel = ModelCache.GetOrAdd(
            //   Tuple.Create(tenant, dbConnection.ConnectionString),
            //   t => GetContextModel(dbConnection, tenant));

            //return ConstructContext(dbConnection, compiledModel, false);

            return default;
        }


        /// <summary>
        /// Constructs the context.
        /// </summary>
        /// <returns></returns>
        protected abstract TContext ConstructContext();

        public TContext CreateDbContext(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}