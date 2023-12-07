using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
        public IEnumerable<SqlParameter> GetStoredProcedureSchema(string name, string schema = "dbo")
        {
            DbConnection dbConnection = Context.Database.GetDbConnection();
            string connectionString = dbConnection.ConnectionString;

            using SqlConnection connection = new(connectionString);
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

        public int ExecuteStoredProcedure(string name, params DbParameter[] parameters)
        {
            string ExecQuery(string x, DbParameter[] y)
            {
                string parameterString = string.Join(",", parameters.Select(z => $"{z.ParameterName}={z.Value}"));
                return $"EXEC {name} {parameterString}";
            }

            string execQueryString = ExecQuery(name, parameters);
            using DbContext context = Context;
            return context.Database.ExecuteSqlRaw(execQueryString, parameters);
        }

        public int ExecuteStoredProcedure(string name, string schema = "dbo", params DbParameter[] parameters)
        {
            string ExecQuery(string x, DbParameter[] y)
            {
                string parameterString = string.Join(",", parameters.Select(z => $"{z.ParameterName}={z.Value}"));
                return $"EXEC {schema}.{name} {parameterString}";
            }

            string execQueryString = ExecQuery(name, parameters);
            using DbContext context = Context;
            return context.Database.ExecuteSqlRaw(execQueryString, parameters);
        }

        public IEnumerable<T> ExecuteStoredProcedure<T>(string name, string schema = "dbo", params DbParameter[] parameters)
        {
            DbConnection dbConnection = Context.Database.GetDbConnection();
            string connectionString = dbConnection.ConnectionString;

            using SqlConnection connection = new(connectionString);
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