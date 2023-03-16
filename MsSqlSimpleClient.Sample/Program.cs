using MsSqlSimpleClient.Attributes;
using MsSqlSimpleClient.Attributes.Procedures;
using MsSqlSimpleClient.SqlClient.Procedures;
using System.Data;
using System.Data.SqlClient;
using MsSqlSimpleClient.Converters;
using MsSqlSimpleClient.Attributes.SqlTable;
using MsSqlSimpleClient.SqlClient.Direct;

string cs = File.ReadAllLines("connection.secret")[0];


ISqlProcedureClient client = new SqlProcedureClient(cs);

// Manually defining the return parameter
SqlParameter param = new SqlParameter();
param.ParameterName = "@ProcParameter";
param.DbType = DbType.Int32;
param.Direction = ParameterDirection.Output;

// This will invoke the procedure with parameters 1,2,3,4
client.ExecuteNonQueryAsyncWith("procedure", param);

int resultValue = (int)param.Value;
