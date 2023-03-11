# MsSqlSimpleClient

`MsSqlSimpleClient` is a small library intended to help interface with a MSSQL database. Normaly, when calling a procedure from an MSSQL server you have to open a connection, then create a command, modify the command, load all the parameters, then you have to read the data from the procedure.

Although the process is not difficult, it can get very tedious very quickly.

<!-- vscode-markdown-toc -->
* 1. [SQL procedure client](#SQLprocedureclient)
	* 1.1. [Calling a procedure with no parameters and no results](#Callingaprocedurewithnoparametersandnoresults)
	* 1.2. [Calling a procedure with parameters and no results](#Callingaprocedurewithparametersandnoresults)
	* 1.3. [SQL procedure with results](#SQLprocedurewithresults)
* 2. [SQL direct client](#SQLdirectclient)
* 3. [Calling procedures](#Callingprocedures)
* 4. [Calling procedures with output parameters](#Callingprocedureswithoutputparameters)
* 5. [Handling parameter names](#Handlingparameternames)
* 6. [Handling output parameters from a procedure](#Handlingoutputparametersfromaprocedure)
* 7. [Converting a data set](#Convertingadataset)
* 8. [Converting a grouped data set](#Convertingagroupeddataset)
* 9. [Converting a grouped data set (joins)](#Convertingagroupeddatasetjoins)
* 10. [Define sql column name](#Definesqlcolumnname)
* 11. [Ignore property](#Ignoreproperty)

<!-- vscode-markdown-toc-config
	numbering=true
	autoSave=true
	/vscode-markdown-toc-config -->
<!-- /vscode-markdown-toc -->

# Getting started

##  1. <a name='SQLprocedureclient'></a>SQL procedure client

###  1.1. <a name='Callingaprocedurewithnoparametersandnoresults'></a>Calling a procedure with no parameters and no results

```cs
// Creates a new SQL client
ISqlProcedureClient client = new SqlProcedureClient(connectionString);

// Calls the procedure with the given name
client.ExecuteNonQueryAsync("procedureName");

```

This will execute the procedure with the given name on the server speicified through the connection string.

###  1.2. <a name='Callingaprocedurewithparametersandnoresults'></a>Calling a procedure with parameters and no results

In order to call a procedure with paramters you first have to create an object, which contains the properties required by the procedure.

``` SQL
create procedure CreatePerson @FirstName nvarchar(50), @LastName nvarchar(50), @Age int
as
begin
...
end
go
```

Suppose you want to call the procedure `CreatePerson`. You first have to create a props object which will be used to determine which value should be passed to which parameter.

```cs
public class CreatePersonProps
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
}
```

In order to call the procedure you would use the following snippet.

```cs
CreatePersonProps props = new CreatePersonProps
{
  FirstName = "Name",
  LastName = "Surname",
  Age = 10
};

// Creates a new SQL client
ISqlProcedureClient client = new SqlProcedureClient(connectionString);

// Calls the procedure with the given name
client.ExecuteNonQueryAsync("CreatePerson", props);

```

This will execute the SQL procedure with all of the parameters filled in with the values read from the passed in object.

```cs

// Creates a new SQL client
ISqlProcedureClient client = new SqlProcedureClient(connectionString);

// Calls the procedure with the given name
client.ExecuteNonQueryAsync("CreatePerson", new {
  FirstName = "Name",
  LastName = "Surname",
  Age = 10
});

```

In addition you can also pass an annonymuss object isntead of a concrete one.

###  1.3. <a name='SQLprocedurewithresults'></a>SQL procedure with results

When it comes to reading procedures with results all of the modifications specified in the previous parts are also aplicable (What I am trying to imply is that you can pass parameters the same way as mentioned in the section above).

``` cs
ISqlProcedureClient client = new SqlProcedureClient(cs);

// Calls the procedure and returns the results.
DataSet data = await client.ExecuteQueryAsync("GetPeople");
```

The `ExecuteQueryAsync` will call the procedure and any table results the procedure returns will be returned into a `DataSet` object. This is the data set object as provided by C#. 

In order to read the data from the data set, I have provided an extension method `ConvertTo<>` which will read the data from the data set object and parse it into the collection of the specified item.

 ``` cs
// You need model into which the data will be loaded
public class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

// ...

ISqlProcedureClient client = new SqlProcedureClient(cs);

DataSet data = await client.ExecuteQueryAsync("People");

// Converts the data from the data set into an actual object while ignoring grouping
IEnumerable<Person> people = data.ConvertTo<Person>(ignoreGrouping: true);
```

As mentioned above the `ConvertTo<>` method will read the data from the data set. All tables are considered, and all columns are considered.

If you have any extra tables and columns that are not related to the object, they will simply not be parsed into actual values.

`ignoreGrouping` parameter determines if the objects should be grouped together based on the identity. This should (hopefully) be covered in later parts of the document.

##  2. <a name='SQLdirectclient'></a>SQL direct client

Sometimes you don't need the full feature set that the procedures, and you would rather just use SQL commands directly. Although it is not a good practice, it's here.

Maybe one day this feature could be expanded upon, but today is not that day.

``` cs

public class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

ISqlDirectClient client = new SqlDirectClient(cs);

IEnumerable<Person> people = (await client.ExecuteQueryAsync("select * from People")).ConvertTo<Person>();

```

`ExecuteNonQueryAsync` and `ExecuteQueryAsync` will simply pass the provided SQL command to the server specified through the conenction string. `ExecuteNonQueryAsync` will only return the number of affected rows, while `ExecuteQueryAsync` will return a data set.

Same as in the last chapter the data set can be converted using the `ConvertTo<>` mehtod.

# SQL procedure client

##  3. <a name='Callingprocedures'></a>Calling procedures

``` cs

// Calls a procedure with no parameters
sqlProcedureClient.ExecuteNonQuery("CreatePerson");

// Calls a procedure with parameters
sqlProcedureClient.ExecuteNonQuery("CreatePerson", new { FirstName = "", LastName = "" });

```

When calling a procedure you have to provide a procedure name. If your procedure needs parameters, they can provided through the second parameter which is an object. The second parameter can either be an actual object, or you can use an annonymuss object.

##  4. <a name='Callingprocedureswithoutputparameters'></a>Calling procedures with output parameters

``` cs

public class CreatePersonParams
{
  [SqlOutput]
  public int PersonId {get;set;}
  public string FirstName {get;set;}
  public string LastName {get;set;}
}

```

In order to read output parameters you MUST provide an actual object (annonymuss objects will not work). All paramters which should contain the output results should have the `SqlOutput` attribute above them in order to indicate to the library, that the output values from the procedure should be loaded there.

``` cs

var personParams = new CreatePersonParams(...);

sqlProcedureClient.ExecuteNonQuery("CreatePeron", personParams);

// The value will be read from the procedure call
int createdId = personParams.PersonId;

```

As I tried to explain in the above paragraph; after the `ExecuteNonQuery` calls the procedure it will read any specified `SqlOutput` parameters and try to load the values into the provided props object.

##  5. <a name='Handlingparameternames'></a>Handling parameter names

``` cs

public class CreatePersonParams
{
  [SqlOutput]
  public int PersonId {get;set;}

  [SqlParameterName("FirstName")]
  public string UserFirstName {get;set;}
  [SqlParameterName("LastName")]
  public string UserLastName {get;set;}
}

```

Sometimes your variable names will not match with the names of the paramters the procedure has. You can use the `SqlParameterName` attribute to define a different name when mapping properties to a procedure.

##  6. <a name='Handlingoutputparametersfromaprocedure'></a>Handling output parameters from a procedure

``` cs

var personParams = new CreatePersonParams(...);
int createdId = 0;

sqlProcedureClient.ExecuteNonQuery("CreatePeron", personParams, (params) => {
  createdId = params.PersonId;
});

```

There is a third parameter you can pass to the execute methods of the procedure client. After the procedure is called, after the output parameters are loaded, the prams handler will be called (the handler is the third parameter).

When I was defining the methods it sounded usefull, but now... I don't know... It's here tho ;)

# Data set converter

##  7. <a name='Convertingadataset'></a>Converting a data set
``` cs

public class Person
{
  public int PersonId {get;set;}
  public string FirstName {get;set;}
  public string LastName {get;set;}
}

```


``` cs

DataSet data = ...;

IEnumerable<Person> people = data.ConvertTo<Person>(ignoreGrouping: true);

```

Collecs all the tables and rows from the data set and converts them into a `Person` object enumerable collection.

Column names from the data set are mapped to the property names. Columns and properties with the same name are loaded into the object.

`ignoreGrouping` specifies that there should be no fancy groupings done when collecting the objects.

##  8. <a name='Convertingagroupeddataset'></a>Converting a grouped data set

``` cs

public class Person
{
  [SqlIdentity]
  public int PersonId {get;set;}
  public string FirstName {get;set;}
  public string LastName {get;set;}
}

```


``` cs

DataSet data = ...;

IEnumerable<Person> people = data.ConvertTo<Person>();

```

By default when executing the conversion, `ignoreGrouping` is set to `true`. This means that if a duplicate key exists with the data set it should be ignored.

For example, let's say that a data set would return the following data.

| PersonId 	| FirstName 	| LastName 	|
|----------	|-----------	|----------	|
| 1        	| Ivo       	| Ivic     	|
| 2        	| Ana       	| Anic     	|
| 2        	| Jozo       	| Jozic     |

If `ignoreGrouping` is set to false, you would get back 3 items with all of the values loaded from the table.

However, if `ignoreGrouping` is set to true (which is the default value), you would only get back 2 items, while the last item in the table (for this example) would be ignored, because an item with the id = 2 is already loaded.

##  9. <a name='Convertingagroupeddatasetjoins'></a>Converting a grouped data set (joins)

I think grouping is usefull when it comes to SQL joins. Consider the following class hierarchy.


``` cs

public class Family
{
  [SqlIdentity]
  public int FamilyId {get;set;}
  public string FamilyName {get;set;}

  [SqlExtendedCollection]
  public IEnumerable<Person> FamilyMembers {get;set;}
}

public class Person
{
  [SqlIdentity]
  public int PersonId {get;set;}
  public string FirstName {get;set;}
  public string LastName {get;set;}
}

```

Let's also consider the following the table.

| FaimlyId 	| FamilyName 	| PersonId 	| LastName 	| FirstName 	|
|----------	|------------	|----------	|----------	|-----------	|
| 1        	| Familia    	| 1        	| Ana      	| Anic      	|
| 1        	| Familia    	| 2        	| Bob      	| Bob       	|

What I want to create from the given table is a `Family` object which contains a list of all family members.

In order to set this up, you first have to prepare the `Family` class. `SqlIdentity` is used to define on which basis should the duplicates be detected (in 99% of cases the id would be used as duplicate detection).

In order to get a list of all family memebers, the object should provide either an `IEnumerable` or a `GrouppedCollection`. Above the property should be placed the `SqlExtendedCollection` attribute. This will indicate to the parser that the mentioned property should be the next entity to load in from the table.

All entites contained in this "grouped loading" type of conversion must have one property which is defined as the `SqlIdentity`.

``` cs

DataSet data = ...;

IEnumerable<Family> families = data.ConvertTo<Family>();

```

##  10. <a name='Definesqlcolumnname'></a>Define sql column name

``` cs
public class Person
{
  [SqlIdentity]
  public int PersonId {get;set;}

  [SqlColumnName("PersonFirstName")]
  public string FirstName {get;set;}
  [SqlColumnName("PersonLastName")]
  public string LastName {get;set;}
}

```

Sometimes the property name will not match the name of the column the data should be read from. You can use the `SqlColumnName` attribute in order to change from which column the data should be loaded from.

##  11. <a name='Ignoreproperty'></a>Ignore property

``` cs
public class Person
{
  [SqlIdentity]
  public int PersonId {get;set;}

  public string FirstName {get;set;}
  public string LastName {get;set;}

  [SqlIgnore]
  public string ThisShouldBeIgnored {get;set;}
}

```

If you want to ignore a value when reading from the data set, you can use the `SqlIgnore` attribute. When data is being converted, these properties will not be considered when reading data tables.