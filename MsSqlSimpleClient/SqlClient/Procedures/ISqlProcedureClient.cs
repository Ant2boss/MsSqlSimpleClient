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
        public Task<int> ExecuteNonQueryProcedureAsync<PropType>(string procedure, PropType? procedureProps, Action<PropType>? propsHandler);

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
        public Task<DataSet> ExecuteQueryProcedureAsync<PropType>(string procedure, PropType? procedureProps, Action<PropType>? propsHandler);
    }
}
