using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

#if NET461

using System.Data.Entity;

#else

using Microsoft.EntityFrameworkCore;

#endif

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
        /// <summary>
        /// Executes the stored procedure asynchronous.
        /// </summary>
        /// <param name="name">The name of the stored procedure.</param>
        /// <param name="schema">The parameters.</param>
        /// <returns></returns>
        public IEnumerable<SqlParameter> GetStoredProcedureSchema(string name, string schema = "dbo")
        {
            DbConnection dbConnection = Context.Database.GetDbConnection();
            string connectionString = dbConnection.ConnectionString;

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = $"{schema}.{name}";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            SqlCommandBuilder.DeriveParameters(cmd);
            foreach (SqlParameter param in cmd.Parameters)
            {
                yield return param;
            }
        }

        /// <summary>
        /// Executes the stored procedure asynchronous.
        /// </summary>
        /// <param name="name">The name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public int ExecuteStoredProcedure(string name, params DbParameter[] parameters)
        {
            string ExecQuery(string x, DbParameter[] y)
            {
                string parameterString = string.Join(",", parameters.Select(z => $"{z.ParameterName}={z.Value}"));
                return $"EXEC {name} {parameterString}";
            }

            string execQueryString = ExecQuery(name, parameters);
            using DbContext context = Context;
            return context.Database.ExecuteSqlCommand(execQueryString, parameters);
        }

        /// <summary>
        /// Executes the stored procedure asynchronous.
        /// </summary>
        /// <param name="name">The name of the stored procedure.</param>
        /// <param name="schema"></param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public int ExecuteStoredProcedure(string name, string schema = "dbo", params DbParameter[] parameters)
        {
            string ExecQuery(string x, DbParameter[] y)
            {
                string parameterString = string.Join(",", parameters.Select(z => $"{z.ParameterName}={z.Value}"));
                return $"EXEC {schema}.{name} {parameterString}";
            }

            string execQueryString = ExecQuery(name, parameters);
            using DbContext context = Context;
            return context.Database.ExecuteSqlCommand(execQueryString, parameters);
        }

        /// <summary>
        /// Executes the stored procedure asynchronous.
        /// </summary>
        /// <param name="name">The name of the stored procedure.</param>
        /// <param name="schema"></param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public IEnumerable<T> ExecuteStoredProcedure<T>(string name, string schema = "dbo", params DbParameter[] parameters)
        {
            DbConnection dbConnection = Context.Database.GetDbConnection();
            string connectionString = dbConnection.ConnectionString;

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using DbCommand cmd = connection.CreateCommand();
            cmd.CommandText = $"{schema}.{name}";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parameters);

            using DbDataReader reader = cmd.ExecuteReader();
            return reader.GetRecords<T>();
        }
    }
}