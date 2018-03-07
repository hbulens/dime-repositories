using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
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
            using (TContext ctx = this.Context)
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
                string parameterString = string.Join(",", parameters.Select(z => string.Format("{0}={1}", z.ParameterName, z.Value)));
                return string.Format("EXEC {0} {1}", name, parameterString);
            };

            string execQueryString = execQuery(name, parameters);
            using (TContext ctx = this.Context)
            {
                return await ctx.Database.ExecuteSqlCommandAsync(execQueryString, default(CancellationToken), parameters);
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
                string parameterString = string.Join(",", parameters.Select(z => string.Format("{0}={1}", z.ParameterName, z.Value)));
                return string.Format("EXEC {0}.{1} {2}", schema, name, parameterString);
            };

            string execQueryString = execQuery(name, parameters);
            using (TContext ctx = this.Context)
            {
                return await ctx.Database.ExecuteSqlCommandAsync(execQueryString, default(CancellationToken), parameters);
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
            using (IDbConnection connection = this.Context.Database.GetDbConnection())
            {
                connection.Open();
                using (IDbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = name;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(parameters);

                    using (IDataReader reader = cmd.ExecuteReader())
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
                string parameterString = string.Join(",", parameters.Select(z => string.Format("{0}={1}", z.ParameterName, z.Value)));
                return string.Format("EXEC {0}.{1} {2}", schema, nameof(name), parameterString);
            };

            string execQueryString = execQuery(nameof(name), parameters);
            using (TContext ctx = this.Context)
            {
                return await ctx.Database.ExecuteSqlCommandAsync(execQueryString, default(CancellationToken), parameters);
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
                string parameterString = string.Join(",", parameters.Select(z => string.Format("@{0}={1}", z.ParameterName, z.Value)));
                return string.Format("EXEC {0} {1}", command, parameterString);
            };

            return await Task.Run(() =>
            {
                using (TContext ctx = this.Context)
                {
                    return Task.FromResult(default(IEnumerable<T>));
                    //return ctx.Database.ExecuteSqlCommand>(execQuery(command, parameters));
                }
            });
        }
    }
}