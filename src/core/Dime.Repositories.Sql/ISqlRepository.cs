using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISqlRepository<T> : IRepository<T>, IStoredProcedureRepository where T : class
    {
        /// <summary>
        /// Executes the SQL asynchronous.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <returns></returns>
        Task ExecuteSqlAsync(string sql);

        /// <summary>
        /// Executes the stored procedure asynchronous.
        /// </summary>
        /// <param name="command">The name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        Task<int> ExecuteStoredProcedureAsync(string command, params DbParameter[] parameters);

        /// <summary>
        /// Executes the stored procedure asynchronous.
        /// </summary>
        /// <param name="command">The name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        Task<int> ExecuteStoredProcedureAsync(string command, string schema = "dbo", params DbParameter[] parameters);

        /// <summary>
        /// Executes the stored procedure asynchronous.
        /// </summary>
        /// <param name="command">The name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        Task<int> ExecuteStoredProcedureAsync<TResult>(TResult command, string schema = "dbo", params DbParameter[] parameters) where TResult : IStoredProcedure;

        /// <summary>
        /// Executes the stored procedure asynchronous.
        /// </summary>
        /// <param name="command">The name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        Task<IEnumerable<TResult>> ExecuteStoredProcedureAsync<TResult>(string command, params DbParameter[] parameters);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="schema"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<IEnumerable<TResult>> ExecuteStoredProcedureAsync<TResult>(string name, string schema = "dbo", params DbParameter[] parameters);
    }
}