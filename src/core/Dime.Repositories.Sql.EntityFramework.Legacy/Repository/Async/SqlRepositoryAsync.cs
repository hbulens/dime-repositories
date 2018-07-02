using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

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
            using (TContext ctx = Context)
            {
                await ctx.Database.ExecuteSqlCommandAsync(sql);
            }
        }

        /// <summary>
        /// Executes the stored procedure asynchronous.
        /// </summary>
        /// <param name="command">The name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public async Task<int> ExecuteStoredProcedureAsync(string name, params DbParameter[] parameters)
        {
            Func<string, DbParameter[], string> execQuery = (x, y) =>
            {
                string parameterString = string.Join(",", parameters.Select(z => $"{z.ParameterName}={z.Value}"));
                return $"EXEC {name} {parameterString}";
            };

            string execQueryString = execQuery(name, parameters);
            using (TContext ctx = Context)
            {
                return await ctx.Database.ExecuteSqlCommandAsync(execQueryString, parameters);
            }
        }

        /// <summary>
        /// Executes the stored procedure asynchronous.
        /// </summary>
        /// <param name="command">The name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public async Task<int> ExecuteStoredProcedureAsync(string name, string schema = "dbo", params DbParameter[] parameters)
        {
            Func<string, DbParameter[], string> execQuery = (x, y) =>
            {
                string parameterString = string.Join(",", parameters.Select(z => $"{z.ParameterName}={z.Value}"));
                return $"EXEC {schema}.{name} {parameterString}";
            };

            string execQueryString = execQuery(name, parameters);
            using (TContext ctx = Context)
            {
                return await ctx.Database.ExecuteSqlCommandAsync(execQueryString, parameters);
            }
        }

        /// <summary>
        /// Executes the stored procedure asynchronous.
        /// </summary>
        /// <param name="command">The name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        /// <history>
        /// [HB] 25/02/2016 - Create
        /// </history>
        public async Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(string name, string schema = "dbo", params DbParameter[] parameters)
        {
            using (DbConnection connection = Context.Database.Connection)
            {
                connection.Open();
                using (DbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = name;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parameters);

                    using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        return reader.GetRecords<T>();
                    }
                }
            }
        }

        /// <summary>
        /// Executes the stored procedure asynchronous.
        /// </summary>
        /// <param name="command">The name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public async Task<int> ExecuteStoredProcedureAsync<T>(T name, string schema = "dbo", params DbParameter[] parameters) where T : IStoredProcedure
        {
            Func<string, DbParameter[], string> execQuery = (x, y) =>
            {
                string parameterString = string.Join(",", parameters.Select(z => $"{z.ParameterName}={z.Value}"));
                return $"EXEC {schema}.{nameof(name)} {parameterString}";
            };

            string execQueryString = execQuery(nameof(name), parameters);
            using (TContext ctx = Context)
            {
                return await ctx.Database.ExecuteSqlCommandAsync(execQueryString, parameters);
            }
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
            Func<string, DbParameter[], string> execQuery = (x, y) =>
            {
                string parameterString = string.Join(",", parameters.Select(z => $"@{z.ParameterName}={z.Value}"));
                return $"EXEC {command} {parameterString}";
            };

            return await Task.Run(() =>
            {
                using (TContext ctx = Context)
                {
                    return ctx.Database.SqlQuery<T>(execQuery(command, parameters));
                }
            });
        }
    }
}