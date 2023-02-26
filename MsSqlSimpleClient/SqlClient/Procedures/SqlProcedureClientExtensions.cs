using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsSqlSimpleClient.SqlClient.Procedures
{
    public static class SqlProcedureClientExtensions
    {
        public static Task<DataSet> ExecuteQueryProcedureAsync(this ISqlProcedureClient client, string procedure)
            => client.ExecuteQueryProcedureAsync<object?>(procedure, null, null);
        public static Task<DataSet> ExecuteQueryProcedureAsync<PropType>(this ISqlProcedureClient client, string procedure, PropType procedureProps)
            => client.ExecuteQueryProcedureAsync(procedure, procedureProps, null);
    }
}
