using MsSqlSimpleClient.Attributes;
using MsSqlSimpleClient.Attributes.Procedures;
using MsSqlSimpleClient.SqlClient.Procedures;
using System.Data;
using System.Data.SqlClient;
using MsSqlSimpleClient.Converters;
using MsSqlSimpleClient.Attributes.SqlTable;
using MsSqlSimpleClient.SqlClient.Direct;

string cs = File.ReadAllLines("connection.secret")[0];


ISqlDirectClient client = new SqlDirectClient(cs);

IEnumerable<string> people = (await client.ExecuteQueryAsync("select * from People")).ConvertTo<string>();

