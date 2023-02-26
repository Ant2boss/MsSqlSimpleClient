using MsSqlSimpleClient.Exceptions;
using System.Data;

namespace MsSqlSimpleClient.SqlClient.Direct
{
    /// <summary>
    /// SQL client for executing SQL statements directly.
    /// </summary>
    public interface ISqlDirectClient
    {
        /// <summary>
        /// Executes an SQL statement which does not return any results.
        /// </summary>
        /// <param name="sqlStatement">SQL statement to execute.</param>
        /// <returns>Number of records affeceted by the operation.</returns>
        /// <exception cref="ArgumentException" />
        /// <exception cref="SqlConnectionFailedException" />
        public Task<int> ExecuteNonQueryAsync(string sqlStatement);

        /// <summary>
        /// Executes an SQL statement which returns table results.
        /// </summary>
        /// <param name="sqlStatement">SQL statement to execute.</param>
        /// <returns>Set of tables returned by the SQL statement.</returns>
        /// <exception cref="ArgumentException" />
        /// <exception cref="SqlConnectionFailedException" />
        public Task<DataSet> ExecuteQueryAsync(string sqlStatement);
    }
}
