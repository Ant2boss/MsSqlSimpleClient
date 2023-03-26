using MsSqlSimpleClient.SqlClient.Handlers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsSqlSimpleClient.SqlClient.Procedures
{
    public sealed class SqlProcedureClient : ISqlProcedureClient
    {
        private readonly string _connectionString;

        public SqlProcedureClient(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public Task<int> ExecuteNonQueryAsync<PropType>(string procedure, PropType? procedureProps, Action<PropType>? propsHandler)
        {
            return Task.Run(() =>
            {
                int result = 0;

                this._PrepareForProcedureCall(procedure, procedureProps, (connection, command) =>
                {
                    result = command.ExecuteNonQuery();
                });

                if (procedureProps is not null)
                {
                    propsHandler?.Invoke(procedureProps);
                }

                return result;
            });
        }

        public Task<int> ExecuteNonQueryAsyncWith(string procedure, params object[] procedureParameters)
        {
            return Task.Run(() =>
            {
                int result = 0;

                this._PrepareForProcedureCallWith(procedure, procedureParameters, (connection, command) =>
                {
                    result = command.ExecuteNonQuery();
                });

                return result;
            });
        }

        public Task<DataSet> ExecuteQueryAsync<PropType>(string procedure, PropType? procedureProps, Action<PropType>? propsHandler)
        {
            return Task.Run(() =>
            {
                DataSet ds = new DataSet();

                this._PrepareForProcedureCall(procedure, procedureProps, (connection, command) =>
                {
                    using SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(ds);
                });

                if (procedureProps is not null)
                {
                    propsHandler?.Invoke(procedureProps);
                }

                return ds;
            });
        }

        public Task<DataSet> ExecuteQueryAsyncWith(string procedure, params object[] procedureParameters)
        {
            return Task.Run(() =>
            {
                DataSet ds = new DataSet();

                this._PrepareForProcedureCallWith(procedure, procedureParameters, (connection, command) =>
                {
                    using SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(ds);
                });

                return ds;
            });
        }

        private void _PrepareForProcedureCall(string procedure, object? props, Action<SqlConnection, SqlCommand> handler)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = procedure;

                    if (props is not null)
                    {
                        SqlCommandBuilder.DeriveParameters(command);
                        SqlPropsHandler.SetupParameters(command.Parameters, props);
                    }

                    handler.Invoke(connection, command);

                    if (props is not null)
                    {
                        SqlPropsHandler.ReadOutputParameters(command.Parameters, props);
                    }
                }
            }
        }

        private void _PrepareForProcedureCallWith(string procedure, object[] procedureParameters, Action<SqlConnection, SqlCommand> handler)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = procedure;

                    SqlCommandBuilder.DeriveParameters(command);

                    if (command.Parameters.Count != procedureParameters.Length + 1)
                    {
                        throw new ArgumentException($"Procedure ({procedure}) expected ({command.Parameters.Count}) parameters, but found ({procedureParameters.Length})");
                    }

                    for (int i = 0; i < procedureParameters.Length; ++i)
                    {
                        if (procedureParameters[i] is SqlParameter param)
                        {
                            command.Parameters[i + 1] = param;
                            continue;
                        }

                        command.Parameters[i + 1].Value = procedureParameters[i];
                    }

                    handler.Invoke(connection, command);
                }
            }
        }
    }
}
