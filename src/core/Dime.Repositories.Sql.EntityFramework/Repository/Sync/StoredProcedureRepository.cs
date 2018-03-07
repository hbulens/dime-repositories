using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
            using (IDbConnection connection = this.Context.Database.GetDbConnection())
            {
                connection.Open();
                using (IDbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = string.Format("{0}.{1}", schema, name);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    //SqlCommandBuilder.DeriveParameters(cmd);
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
                string parameterString = string.Join(",", parameters.Select(z => string.Format("{0}={1}", z.ParameterName, z.Value)));
                return string.Format("EXEC {0} {1}", name, parameterString);
            };

            string execQueryString = execQuery(name, parameters);
            using (DbContext context = this.Context)
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
                string parameterString = string.Join(",", parameters.Select(z => string.Format("{0}={1}", z.ParameterName, z.Value)));
                return string.Format("EXEC {0}.{1} {2}", schema, name, parameterString);
            };

            string execQueryString = execQuery(name, parameters);
            using (DbContext context = this.Context)
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
            using (IDbConnection connection = this.Context.Database.GetDbConnection())
            {
                connection.Open();
                using (IDbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = string.Format("{0}.{1}", schema, name);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(parameters);

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return reader.GetRecords<T>();
                    }
                }
            }
        }
    }
}