using MsSqlSimpleClient.Attributes;
using MsSqlSimpleClient.Attributes.Conversion;
using MsSqlSimpleClient.Attributes.Procedures;
using MsSqlSimpleClient.SqlClient.Procedures;
using System.Data;
using System.Data.SqlClient;
using MsSqlSimpleClient.Converters;

string cs = File.ReadAllLines("connection.secret")[0];

async Task BasicSqlProcedureCall()
{
    var sqlClient = new SqlProcedureClient(cs);

    var results = (await sqlClient.ExecuteQueryProcedureAsync(
        "test_procedure",
        new
        {
            FirstName = "Robert",
            LastName = "MeDiro",
            Age = 22,
            Id = 0
        })).ConvertTo<Results>(ignoreGrouping: true);

    foreach (var result in results)
    {
        Console.WriteLine(result);
    }
}

await BasicSqlProcedureCall();

class Results
{
    [SqlRequired]
    [SqlColumnName("fn")]
    public string FirstName { get; set; } = "";

    [SqlRequired]
    [SqlColumnName("ln")]
    public string LastName { get; set; } = "";

    [SqlRequired]
    [SqlColumnName("a")]
    public int Age { get; set; }

    public override string ToString() => $"{FirstName} {LastName} [{this.Age}]";
}

