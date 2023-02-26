using MsSqlSimpleClient.Attributes;
using MsSqlSimpleClient.SqlClient.Procedures;
using System.Data;
using System.Data.SqlClient;

string cs = File.ReadAllLines("connection.secret")[0];

var sqlClient = new SqlProcedureClient(cs);

int id = 0;

DataSet data = await sqlClient.ExecuteQueryProcedureAsync(
    "test_procedure",
    new ProcedureParameters
    {
        FirstName = "Robert",
        LastName = "MeDiro",
        Age = 22
    }, (props) =>
    {
        id = props.Identity;
    });

Console.WriteLine("ID: " + id);

Console.WriteLine("Table count: " + data.Tables.Count);
Console.WriteLine("Table[0] rows count: " + data.Tables[0].Rows.Count);

Console.WriteLine("Table[0].Rows[0][fn]: " + data.Tables[0].Rows[0]["fn"]);
Console.WriteLine("Table[0].Rows[0][ln]: " + data.Tables[0].Rows[0]["ln"]);
Console.WriteLine("Table[0].Rows[0][a]: " + data.Tables[0].Rows[0]["a"]);

class ProcedureParameters
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public int Age { get; set; } = 0;

    [SqlOutput]
    [SqlParameterName("Id")]
    public int Identity { get; set; }
}

