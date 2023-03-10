using MsSqlSimpleClient.SqlClient.Handlers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsSqlSimpleClient.SqlClient.Direct
{
    public sealed class SqlDirectClient : ISqlDirectClient
    {
        private readonly string _connectionString;

        public SqlDirectClient(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public Task<int> ExecuteNonQueryAsync(string sqlStatement)
        {
            return Task.Run(() =>
            {
                int result = 0;

                this._PrepareForProcedureCall(sqlStatement, (connection, command) =>
                {
                    result = command.ExecuteNonQuery();
                });

                return result;
            });
        }

        public Task<DataSet> ExecuteQueryAsync(string sqlStatement)
        {
            return Task.Run(() =>
            {
                DataSet ds = new DataSet();

                this._PrepareForProcedureCall(sqlStatement, (connection, command) =>
                {
                    using SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(ds);
                });

                return ds;
            });
        }

        private void _PrepareForProcedureCall(string sqlCommand, Action<SqlConnection, SqlCommand> handler)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sqlCommand;

                    handler.Invoke(connection, command);
                }
            }
        }
    }
}
