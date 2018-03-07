using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;

namespace Dime.Repositories
{
    public interface IStoredProcedureRepository
    {
        /// <summary>
        /// Executes the stored procedure asynchronous.
        /// </summary>
        /// <param name="command">The name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        int ExecuteStoredProcedure(string command, params DbParameter[] parameters);

        /// <summary>
        /// Executes the stored procedure asynchronous.
        /// </summary>
        /// <param name="command">The name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        int ExecuteStoredProcedure(string command, string schema = "dbo", params DbParameter[] parameters);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="schema"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<T> ExecuteStoredProcedure<T>(string name, string schema = "dbo", params DbParameter[] parameters);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        IEnumerable<SqlParameter> GetStoredProcedureSchema(string name, string schema = "dbo");
    }
}