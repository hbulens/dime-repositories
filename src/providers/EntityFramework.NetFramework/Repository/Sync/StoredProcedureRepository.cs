using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace Dime.Repositories
{
    public partial class EfRepository<TEntity, TContext>
    {
        public IEnumerable<SqlParameter> GetStoredProcedureSchema(string name, string schema = "dbo")
        {
            using TContext ctx = Context;
            using SqlConnection connection = new(ctx.Database.Connection.ConnectionString);
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
            using TContext ctx = Context;
            string ExecQuery(string x, DbParameter[] y)
            {
                string parameterString = string.Join(",", parameters.Select(z => $"{z.ParameterName}={z.Value}"));
                return $"EXEC {name} {parameterString}";
            }

            string execQueryString = ExecQuery(name, parameters);
            return ctx.Database.ExecuteSqlCommand(execQueryString, parameters);
        }

        public int ExecuteStoredProcedure(string name, string schema = "dbo", params DbParameter[] parameters)
        {
            using TContext ctx = Context;
            string ExecQuery(string x, DbParameter[] y)
            {
                string parameterString = string.Join(",", parameters.Select(z => $"{z.ParameterName}={z.Value}"));
                return $"EXEC {schema}.{name} {parameterString}";
            }

            string execQueryString = ExecQuery(name, parameters);
            return ctx.Database.ExecuteSqlCommand(execQueryString, parameters);
        }

        public IEnumerable<T> ExecuteStoredProcedure<T>(string name, string schema = "dbo", params DbParameter[] parameters)
        {
            using TContext ctx = Context;
            using DbConnection connection = ctx.Database.Connection;
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