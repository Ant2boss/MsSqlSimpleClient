using MsSqlSimpleClient.Converters;
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
        public static Task<DataSet> ExecuteQueryAsync(this ISqlProcedureClient client, string procedure)
            => client.ExecuteQueryAsync<object?>(procedure, null, null);
        public static Task<DataSet> ExecuteQueryAsync<PropType>(this ISqlProcedureClient client, string procedure, PropType procedureProps)
            => client.ExecuteQueryAsync(procedure, procedureProps, null);

        public static Task<int> ExecuteNonQueryAsync(this ISqlProcedureClient client, string procedure)
            => client.ExecuteNonQueryAsync<object?>(procedure, null, null);
        public static Task<int> ExecuteNonQueryAsync<PropType>(this ISqlProcedureClient client, string procedure, PropType procedureProps)
            => client.ExecuteNonQueryAsync(procedure, procedureProps, null);
    }
}
