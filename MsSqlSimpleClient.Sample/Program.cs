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

DataSet data = await client.ExecuteQueryAsyncWith("GetFamilyMember", 1);

Console.WriteLine(data);
