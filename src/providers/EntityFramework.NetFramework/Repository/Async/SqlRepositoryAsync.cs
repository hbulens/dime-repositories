using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

#if NET461
using System.Data.SqlClient;
#else

using Microsoft.Data.SqlClient;

#endif

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
        public async Task ExecuteSqlAsync(string sql)
        {
            using TContext ctx = Context;
            await ctx.Database.ExecuteSqlCommandAsync(sql).ConfigureAwait(false);
        }

        public async Task<int> ExecuteStoredProcedureAsync(string name, params DbParameter[] parameters)
        {
            using TContext ctx = Context;

            string ExecQuery(string x, DbParameter[] y)
            {
                string parameterString = string.Join(",", y.Select(z => $"{z.ParameterName}={z.Value}"));
                return $"EXEC {x} {parameterString}";
            }

            string execQueryString = ExecQuery(name, parameters);

            return await ctx.Database.ExecuteSqlCommandAsync(execQueryString, parameters).ConfigureAwait(false);
        }

        public async Task<int> ExecuteStoredProcedureAsync(string name, string schema = "dbo", params DbParameter[] parameters)
        {
            string ExecQuery(string x, DbParameter[] y)
            {
                string parameterString = string.Join(",", y.Select(z => $"{z.ParameterName}={z.Value}"));
                return $"EXEC {schema}.{x} {parameterString}";
            }

            string execQueryString = ExecQuery(name, parameters);

            using TContext ctx = Context;
            return await ctx.Database.ExecuteSqlCommandAsync(execQueryString, parameters).ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(string name, string schema = "dbo", params DbParameter[] parameters)
        {
            using TContext ctx = Context;
            using DbConnection connection = ctx.Database.Connection;
            connection.Open();
            using DbCommand cmd = connection.CreateCommand();
            cmd.CommandText = name;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parameters);

            using IDataReader reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            return reader.GetRecords<T>();
        }

        public async Task<int> ExecuteStoredProcedureAsync<T>(T name, string schema = "dbo", params DbParameter[] parameters)
        {
            string ExecQuery(string x, DbParameter[] y)
            {
                string parameterString = string.Join(",", y.Select(z => $"{z.ParameterName}={z.Value}"));
                return $"EXEC {schema}.{nameof(x)} {parameterString}";
            }

            string execQueryString = ExecQuery(nameof(name), parameters);

            using TContext ctx = Context;
            return await ctx.Database.ExecuteSqlCommandAsync(execQueryString, parameters).ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(string command, params DbParameter[] parameters)
        {
            string ExecQuery(string x, DbParameter[] y)
            {
                string parameterString = string.Join(",", y.Select(z => $"@{z.ParameterName}={z.Value}"));
                return $"EXEC {x} {parameterString}";
            }

            return await Task.Run(() =>
            {
                using TContext ctx = Context;
                return ctx.Database.SqlQuery<T>(ExecQuery(command, parameters));
            }).ConfigureAwait(false);
        }
    }
}