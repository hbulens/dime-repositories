using System;
using System.Collections.Concurrent;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.CodeAnalysis;

namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    [ExcludeFromCodeCoverage]
    public abstract class MultiTenantDbContextFactory<TContext> : IMultiTenantDbContextFactory<TContext> where TContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiTenantDbContextFactory{TContext}"/> class
        /// </summary>
        protected MultiTenantDbContextFactory()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiTenantDbContextFactory{TContext}"/> class
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        protected MultiTenantDbContextFactory(string connectionString) : this()
        {
            Connection = connectionString;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="tenant"></param>
        protected MultiTenantDbContextFactory(string connectionString, string tenant) : this(connectionString)
        {
            Tenant = tenant;
        }

        protected static ConcurrentDictionary<Tuple<string, string>, DbCompiledModel> ModelCache = new ConcurrentDictionary<Tuple<string, string>, DbCompiledModel>();
        protected static ConcurrentDictionary<Tuple<string, string, string>, DbCompiledModel> NamedModelCache = new ConcurrentDictionary<Tuple<string, string, string>, DbCompiledModel>();

        public string Connection { get; set; }
        public string Tenant { get; set; }

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
        /// <param name="nameOrConnectionString">The connection.</param>
        /// <returns></returns>
        public virtual TContext Create(string nameOrConnectionString) => Create("dbo", nameOrConnectionString);

        /// <summary>
        /// Creates the specified tenant.
        /// </summary>
        /// <param name="tenant">The tenant.</param>
        /// <param name="connection">The connection.</param>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual TContext Create(string tenant, string connection, string context)
        {
            if (string.IsNullOrEmpty(context))
                return Create(tenant, connection);

            SqlConnectionFactory connectionFactory = new SqlConnectionFactory();
            DbConnection dbConnection = connectionFactory.CreateConnection(connection);
            Database.SetInitializer<TContext>(null);

            DbCompiledModel compiledModel = NamedModelCache.GetOrAdd(
                Tuple.Create(tenant, dbConnection.ConnectionString, context),
                _ => GetContextModel(dbConnection, tenant));

            return ConstructContext(dbConnection, compiledModel, false);
        }

        /// <summary>
        /// Creates the specified tenant.
        /// </summary>
        /// <param name="tenant">The tenant.</param>
        /// <param name="nameOrConnectionString">The connection.</param>
        /// <returns></returns>
        public virtual TContext Create(string tenant, string nameOrConnectionString)
        {
            SqlConnectionFactory connectionFactory = new SqlConnectionFactory();
            DbConnection dbConnection = connectionFactory.CreateConnection(nameOrConnectionString);
            Database.SetInitializer<TContext>(null);

            DbCompiledModel compiledModel = ModelCache.GetOrAdd(
               Tuple.Create(tenant, dbConnection.ConnectionString),
               _ => GetContextModel(dbConnection, tenant));

            return ConstructContext(dbConnection, compiledModel, false);
        }

        /// <summary>
        /// Constructs the context.
        /// </summary>
        /// <returns></returns>
        protected abstract TContext ConstructContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection);

        /// <summary>
        /// Gets the scheduler context model.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="schema"></param>
        /// <returns></returns>
        protected abstract DbCompiledModel GetContextModel(DbConnection dbConnection, string schema);
    }
}