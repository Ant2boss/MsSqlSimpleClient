using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace MsSqlSimpleClient.SqlClient.Procedures
{
    /// <summary>
    /// SQL client for calling and executing procedures.
    /// </summary>
    public interface ISqlProcedureClient
    {
        /// <summary>
        /// Executes a query with the given name. This method will read any otuput parameters into the given props object.
        /// This method will not return any SQL results.
        /// </summary>
        /// <param name="procedure">Procedure name to execute.</param>
        /// <param name="procedureProps">Properties to pass onto the procedure.</param>
        /// <param name="propsHandler">Handler which is called after the procedure, used to read output parameters.</param>
        /// <returns>Number od records affeceted by the client.</returns>
        /// <exception cref="ArgumentException" />
        /// <exception cref="SqlException" />
        public Task<int> ExecuteNonQueryAsync<PropType>(string procedure, PropType? procedureProps, Action<PropType>? propsHandler);

        /// <summary>
        /// Executes the procedure with given name. This method will read any otuput parameters into the given props object.
        /// After the procedure is finished, the propsHandler will be invoked, from there you can read any output props you need.
        /// </summary>
        /// <typeparam name="PropType"></typeparam>
        /// <param name="procedure">Procedure name to call.</param>
        /// <param name="procedureProps">Props to pass to the procedure.</param>
        /// <param name="propsHandler">Handler to read from the props after the procedure is called.</param>
        /// <returns>Dataset of tables returned by the SQL procedure.</returns>
        /// <exception cref="ArgumentException" />
        /// <exception cref="SqlException" />
        public Task<DataSet> ExecuteQueryAsync<PropType>(string procedure, PropType? procedureProps, Action<PropType>? propsHandler);

        /// <summary>
        /// Executes the procedure with the given name. The paramters are loaded in the same order they are provided in.
        /// </summary>
        /// <param name="procedure">Name of the procedure to invoke.</param>
        /// <param name="procedureParameters">Parameters to pass to the procedure.</param>
        /// <returns>Number of affected rows.</returns>
        public Task<int> ExecuteNonQueryAsyncWith(string procedure, params object[] procedureParameters);

        /// <summary>
        /// Executes the procedure with the given name. The paramters are loaded in the same order they are provided in.
        /// </summary>
        /// <param name="procedure">Name of the procedure to invoke.</param>
        /// <param name="procedureParameters">Parameters to pass to the procedure.</param>
        /// <returns>DataSet containg results of the proceudure.</returns>
        public Task<DataSet> ExecuteQueryAsyncWith(string procedure, params object[] procedureParameters);
    }
}
