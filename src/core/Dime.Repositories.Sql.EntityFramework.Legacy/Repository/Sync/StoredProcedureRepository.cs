using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
        /// <summary>
        /// Executes the stored procedure asynchronous.
        /// </summary>
        /// <param name="command">The name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        /// <history>
        /// [HB] 25/02/2016 - Create
        /// </history>
        public IEnumerable<SqlParameter> GetStoredProcedureSchema(string name, string schema = "dbo")
        {
            using (SqlConnection connection = new SqlConnection(Context.Database.Connection.ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"{schema}.{name}";
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlCommandBuilder.DeriveParameters(cmd);
                    foreach (SqlParameter param in cmd.Parameters)
                    {
                        yield return param;
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
        public int ExecuteStoredProcedure(string name, params DbParameter[] parameters)
        {
            Func<string, DbParameter[], string> execQuery = (x, y) =>
            {
                string parameterString = string.Join(",", parameters.Select(z => $"{z.ParameterName}={z.Value}"));
                return $"EXEC {name} {parameterString}";
            };

            string execQueryString = execQuery(name, parameters);
            using (DbContext context = Context)
            {
                return context.Database.ExecuteSqlCommand(execQueryString, parameters);
            }
        }

        /// <summary>
        /// Executes the stored procedure asynchronous.
        /// </summary>
        /// <param name="command">The name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public int ExecuteStoredProcedure(string name, string schema = "dbo", params DbParameter[] parameters)
        {
            Func<string, DbParameter[], string> execQuery = (x, y) =>
            {
                string parameterString = string.Join(",", parameters.Select(z => $"{z.ParameterName}={z.Value}"));
                return $"EXEC {schema}.{name} {parameterString}";
            };

            string execQueryString = execQuery(name, parameters);
            using (DbContext context = Context)
            {
                return context.Database.ExecuteSqlCommand(execQueryString, parameters);
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
        public IEnumerable<T> ExecuteStoredProcedure<T>(string name, string schema = "dbo", params DbParameter[] parameters)
        {
            using (DbConnection connection = Context.Database.Connection)
            {
                connection.Open();
                using (DbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"{schema}.{name}";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parameters);

                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        return reader.GetRecords<T>();
                    }
                }
            }
        }
    }
}