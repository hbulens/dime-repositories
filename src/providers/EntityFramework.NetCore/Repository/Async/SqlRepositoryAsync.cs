using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
        /// <summary>
        /// Executes the SQL asynchronous.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <returns></returns>
        public async Task ExecuteSqlAsync(string sql)
        {
            using TContext ctx = Context;
            await ctx.Database.ExecuteSqlRawAsync(sql).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes the stored procedure asynchronous.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public async Task<int> ExecuteStoredProcedureAsync(string name, params DbParameter[] parameters)
        {
            string ExecQuery(string x, DbParameter[] y)
            {
                string parameterString = string.Join(",", parameters.Select(z => $"{z.ParameterName}={z.Value}"));
                return $"EXEC {name} {parameterString}";
            }

            string execQueryString = ExecQuery(name, parameters);
            using TContext ctx = Context;
            return await ctx.Database.ExecuteSqlCommandAsync(execQueryString, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes the stored procedure asynchronous.
        /// </summary>
        /// <param name="name">The name of the stored procedure.</param>
        /// <param name="schema"></param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public async Task<int> ExecuteStoredProcedureAsync(string name, string schema = "dbo", params DbParameter[] parameters)
        {
            string ExecQuery(string x, DbParameter[] y)
            {
                string parameterString = string.Join(",", parameters.Select(z => $"{z.ParameterName}={z.Value}"));
                return $"EXEC {schema}.{name} {parameterString}";
            }

            string execQueryString = ExecQuery(name, parameters);
            using TContext ctx = Context;
            return await ctx.Database.ExecuteSqlCommandAsync(execQueryString, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes the stored procedure asynchronous.
        /// </summary>
        /// <param name="name">The name of the stored procedure.</param>
        /// <param name="schema"></param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(string name, string schema = "dbo", params DbParameter[] parameters)
        {
            string connectionString = Context.Database.GetDbConnection()?.ConnectionString;
            using DbConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using DbCommand cmd = connection.CreateCommand();
            cmd.CommandText = name;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parameters);

            using IDataReader reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            return reader.GetRecords<T>();
        }

        /// <summary>
        /// Executes the stored procedure asynchronous.
        /// </summary>
        /// <param name="name">The name of the stored procedure.</param>
        /// <param name="schema"></param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public async Task<int> ExecuteStoredProcedureAsync<T>(T name, string schema = "dbo", params DbParameter[] parameters) where T : IStoredProcedure
        {
            string ExecQuery(string x, DbParameter[] y)
            {
                string parameterString = string.Join(",", parameters.Select(z => $"{z.ParameterName}={z.Value}"));
                return $"EXEC {schema}.{nameof(name)} {parameterString}";
            }

            string execQueryString = ExecQuery(nameof(name), parameters);
            using TContext ctx = Context;
            //return await ctx.Database.ExecuteSqlCommandAsync(execQueryString, parameters).ConfigureAwait(false);
            return 1;
        }

        /// <summary>
        /// Executes the stored procedure asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">The name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(string command, params DbParameter[] parameters)
        {
            string ExecQuery(string x, DbParameter[] y)
            {
                string parameterString = string.Join(",", parameters.Select(z => $"@{z.ParameterName}={z.Value}"));
                return $"EXEC {command} {parameterString}";
            }

            return await Task.Run(() =>
            {
                using TContext ctx = Context;
                //return ctx.Database.SqlQuery<T>(ExecQuery(command, parameters));
                return Task.FromResult(new List<T>());
            }).ConfigureAwait(false);
        }
    }
}