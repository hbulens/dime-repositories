using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace Dime.Repositories
{
    public interface IStoredProcedureRepository
    {
        /// <summary>
        /// Executes the stored procedure.
        /// </summary>
        /// <param name="command">The name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        int ExecuteStoredProcedure(string command, params DbParameter[] parameters);

        /// <summary>
        /// Executes the stored procedure.
        /// </summary>
        /// <param name="command">The name of the stored procedure.</param>
        /// <param name="schema">The schema</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        int ExecuteStoredProcedure(string command, string schema = "dbo", params DbParameter[] parameters);

        /// <summary>
        /// Executes the stored procedure asynchronously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">The name of the stored procedure.</param>
        /// <param name="schema">The schema</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        IEnumerable<T> ExecuteStoredProcedure<T>(string name, string schema = "dbo", params DbParameter[] parameters);

        /// <summary>
        /// Executes the stored procedure asynchronously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">The name of the stored procedure.</param>
        /// <param name="schema">The schema</param>
        /// <returns></returns>
        IEnumerable<SqlParameter> GetStoredProcedureSchema(string name, string schema = "dbo");
    }
}