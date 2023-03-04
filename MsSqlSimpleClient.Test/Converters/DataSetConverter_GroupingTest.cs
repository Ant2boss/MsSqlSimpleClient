using FluentAssertions;
using MsSqlSimpleClient.Converters;
using MsSqlSimpleClient.Exceptions;
using MsSqlSimpleClient.Test.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsSqlSimpleClient.Test.Converters
{
    public sealed class DataSetConverter_GroupingTest
    {
        [Test]
        public void DataSetConverter_ValidDataSet_GeneratesTwoRecords()
        {
            DataSet cityDataSet = new DataSet();

            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Population", typeof(int));
            dt.Columns.Add("IdState", typeof(int));
            dt.Columns.Add("StateName", typeof(string));

            DataRow dr = dt.NewRow();
            dr["Id"] = 1;
            dr["Name"] = "First city";
            dr["Population"] = 1000;
            dr["IdState"] = 1;
            dr["StateName"] = "First state";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 2;
            dr["Name"] = "Second city";
            dr["Population"] = 2000;
            dr["IdState"] = 1;
            dr["StateName"] = "First state";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 3;
            dr["Name"] = "Third city";
            dr["Population"] = 3000;
            dr["IdState"] = 1;
            dr["StateName"] = "First state";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 4;
            dr["Name"] = "Fourht city";
            dr["Population"] = 4000;
            dr["IdState"] = 2;
            dr["StateName"] = "Second state";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 5;
            dr["Name"] = "Fifth city";
            dr["Population"] = 5000;
            dr["IdState"] = 2;
            dr["StateName"] = "Second state";
            dt.Rows.Add(dr);

            dt.AcceptChanges();

            cityDataSet.Tables.Add(dt);
            cityDataSet.AcceptChanges();

            var states = DataSetConverter.ConvertTo<DbState>(cityDataSet).ToArray();

            states.Should().HaveCount(2);

            states[0].IdState.Should().Be(1);
            states[1].IdState.Should().Be(2);

            states[0].StateName.Should().Be("First state");
            states[1].StateName.Should().Be("Second state");

            states[0].Cities.Should().HaveCount(3);
            states[1].Cities.Should().HaveCount(2);
        }

        [Test]
        public void DataSetConverter_ValidCityDataSet_GeneratesThreeRecords()
        {
            DataSet cityDataSet = new DataSet();

            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Population", typeof(int));

            DataRow dr = dt.NewRow();
            dr["Id"] = 1;
            dr["Name"] = "First city";
            dr["Population"] = 1000;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 2;
            dr["Name"] = "Second city";
            dr["Population"] = 2000;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 3;
            dr["Name"] = "Third city";
            dr["Population"] = 3000;
            dt.Rows.Add(dr);

            dt.AcceptChanges();

            cityDataSet.Tables.Add(dt);
            cityDataSet.AcceptChanges();

            var cities = DataSetConverter.ConvertTo<DbCity>(cityDataSet).ToArray();

            cities.Should().HaveCount(3);

            cities[0].Should().BeOfType<DbCity>();
            cities[1].Should().BeOfType<DbCity>();
            cities[2].Should().BeOfType<DbCity>();

            cities[0].Id.Should().Be(1);
            cities[1].Id.Should().Be(2);
            cities[2].Id.Should().Be(3);

            cities[0].Name.Should().Be("First city");
            cities[1].Name.Should().Be("Second city");
            cities[2].Name.Should().Be("Third city");

            cities[0].Papilation.Should().Be(1000);
            cities[1].Papilation.Should().Be(2000);
            cities[2].Papilation.Should().Be(3000);
        }


        [Test]
        public void DataSetConverter_DuplicateDataSet_GeneratesTwoRecords()
        {
            DataSet cityDataSet = new DataSet();

            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Population", typeof(int));
            dt.Columns.Add("IdState", typeof(int));
            dt.Columns.Add("StateName", typeof(string));

            DataRow dr = dt.NewRow();
            dr["Id"] = 1;
            dr["Name"] = "First city";
            dr["Population"] = 1000;
            dr["IdState"] = 1;
            dr["StateName"] = "First state";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 2;
            dr["Name"] = "Second city";
            dr["Population"] = 2000;
            dr["IdState"] = 1;
            dr["StateName"] = "First state";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 3;
            dr["Name"] = "Third city";
            dr["Population"] = 3000;
            dr["IdState"] = 1;
            dr["StateName"] = "First state";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 1;
            dr["Name"] = "First city";
            dr["Population"] = 1000;
            dr["IdState"] = 2;
            dr["StateName"] = "Second state";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 2;
            dr["Name"] = "Second city";
            dr["Population"] = 2000;
            dr["IdState"] = 2;
            dr["StateName"] = "Second state";
            dt.Rows.Add(dr);

            dt.AcceptChanges();

            cityDataSet.Tables.Add(dt);
            cityDataSet.AcceptChanges();

            var states = DataSetConverter.ConvertTo<DbState>(cityDataSet).ToArray();

            states.Should().HaveCount(2);

            states[0].IdState.Should().Be(1);
            states[1].IdState.Should().Be(2);

            states[0].StateName.Should().Be("First state");
            states[1].StateName.Should().Be("Second state");

            states[0].Cities.Should().HaveCount(3);
            states[1].Cities.Should().HaveCount(2);
        }

        [Test]
        public void DataSetConverter_ChildElementContainsNull_GeneratesTwoRecordsWithLessChildren()
        {
            DataSet cityDataSet = new DataSet();

            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Population", typeof(int));
            dt.Columns.Add("IdState", typeof(int));
            dt.Columns.Add("StateName", typeof(string));

            DataRow dr = dt.NewRow();
            dr["Id"] = DBNull.Value;
            dr["Name"] = DBNull.Value;
            dr["Population"] = DBNull.Value;
            dr["IdState"] = 1;
            dr["StateName"] = "First state";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 2;
            dr["Name"] = "Second city";
            dr["Population"] = 2000;
            dr["IdState"] = 2;
            dr["StateName"] = "Second state";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 3;
            dr["Name"] = "Third city";
            dr["Population"] = 3000;
            dr["IdState"] = 2;
            dr["StateName"] = "Second state";
            dt.Rows.Add(dr);

            dt.AcceptChanges();

            cityDataSet.Tables.Add(dt);
            cityDataSet.AcceptChanges();

            var states = DataSetConverter.ConvertTo<DbState>(cityDataSet).ToArray();

            states.Should().HaveCount(2);

            states[0].IdState.Should().Be(1);
            states[1].IdState.Should().Be(2);

            states[0].StateName.Should().Be("First state");
            states[1].StateName.Should().Be("Second state");

            states[0].Cities.Should().BeEmpty();
            states[1].Cities.Should().HaveCount(2);
        }

        [Test]
        public void DataSetConverter_ParentDatasetContainsNull_GeneratesEmpty()
        {
            DataSet cityDataSet = new DataSet();

            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Population", typeof(int));
            dt.Columns.Add("IdState", typeof(int));
            dt.Columns.Add("StateName", typeof(string));

            DataRow dr = dt.NewRow();
            dr["Id"] = DBNull.Value;
            dr["Name"] = DBNull.Value;
            dr["Population"] = DBNull.Value;
            dr["IdState"] = DBNull.Value;
            dr["StateName"] = DBNull.Value;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = DBNull.Value;
            dr["Name"] = DBNull.Value;
            dr["Population"] = DBNull.Value;
            dr["IdState"] = DBNull.Value;
            dr["StateName"] = DBNull.Value;
            dt.Rows.Add(dr);

            dt.AcceptChanges();

            cityDataSet.Tables.Add(dt);
            cityDataSet.AcceptChanges();

            var states = DataSetConverter.ConvertTo<DbState>(cityDataSet).ToArray();

            states.Should().NotBeNull();
            states.Should().BeEmpty();
        }

        [Test]
        public void DataSetConverter_DataSetHasMissingNonRequiredColumns_GeneratesTwoRecords()
        {
            DataSet cityDataSet = new DataSet();

            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("IdState", typeof(int));
            dt.Columns.Add("StateName", typeof(string));

            DataRow dr = dt.NewRow();
            dr["Id"] = 1;
            dr["Name"] = "First city";
            dr["IdState"] = 1;
            dr["StateName"] = "First state";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 2;
            dr["Name"] = "Second city";
            dr["IdState"] = 1;
            dr["StateName"] = "First state";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 3;
            dr["Name"] = "Third city";
            dr["IdState"] = 1;
            dr["StateName"] = "First state";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 4;
            dr["Name"] = "Fourht city";
            dr["IdState"] = 2;
            dr["StateName"] = "Second state";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 5;
            dr["Name"] = "Fifth city";
            dr["IdState"] = 2;
            dr["StateName"] = "Second state";
            dt.Rows.Add(dr);

            dt.AcceptChanges();

            cityDataSet.Tables.Add(dt);
            cityDataSet.AcceptChanges();

            var states = DataSetConverter.ConvertTo<DbState>(cityDataSet).ToArray();

            states.Should().HaveCount(2);

            states[0].IdState.Should().Be(1);
            states[1].IdState.Should().Be(2);

            states[0].StateName.Should().Be("First state");
            states[1].StateName.Should().Be("Second state");

            states[0].Cities.Should().HaveCount(3);
            states[1].Cities.Should().HaveCount(2);

            states[0].Cities.First().Papilation.Should().Be(0);
        }

        [Test]
        public void DataSetConverter_DataSetHasMissingRequiredColumns_ThrowsException()
        {
            DataSet cityDataSet = new DataSet();

            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Population", typeof(int));
            dt.Columns.Add("IdState", typeof(int));
            dt.Columns.Add("StateName", typeof(string));

            DataRow dr = dt.NewRow();
            dr["Id"] = 1;
            dr["Population"] = 1000;
            dr["IdState"] = 1;
            dr["StateName"] = "First state";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 2;
            dr["Population"] = 2000;
            dr["IdState"] = 1;
            dr["StateName"] = "First state";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 3;
            dr["Population"] = 3000;
            dr["IdState"] = 1;
            dr["StateName"] = "First state";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 4;
            dr["Population"] = 4000;
            dr["IdState"] = 2;
            dr["StateName"] = "Second state";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 5;
            dr["Population"] = 5000;
            dr["IdState"] = 2;
            dr["StateName"] = "Second state";
            dt.Rows.Add(dr);

            dt.AcceptChanges();

            cityDataSet.Tables.Add(dt);
            cityDataSet.AcceptChanges();

            Assert.Throws<DataParserRequiredColumnMissingException>(() => DataSetConverter.ConvertTo<DbState>(cityDataSet).ToArray());
        }

        [Test]
        public void DataSetConverter_DataSetHasMissingIdentityColumns_ThrowsException()
        {
            DataSet cityDataSet = new DataSet();

            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Population", typeof(int));
            dt.Columns.Add("StateName", typeof(string));

            DataRow dr = dt.NewRow();
            dr["Id"] = 1;
            dr["Population"] = 1000;
            dr["StateName"] = "First state";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 2;
            dr["Population"] = 2000;
            dr["StateName"] = "First state";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 3;
            dr["Population"] = 3000;
            dr["StateName"] = "First state";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 4;
            dr["Population"] = 4000;
            dr["StateName"] = "Second state";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 5;
            dr["Population"] = 5000;
            dr["StateName"] = "Second state";
            dt.Rows.Add(dr);

            dt.AcceptChanges();

            cityDataSet.Tables.Add(dt);
            cityDataSet.AcceptChanges();

            Assert.Throws<DataParserIdentityMissingException>(() => DataSetConverter.ConvertTo<DbState>(cityDataSet).ToArray());
        }

        [Test]
        public void DataSetConverter_DataSetEmpty_GeneratesTwoRecords()
        {
            DataSet cityDataSet = new DataSet();

            cityDataSet.AcceptChanges();

            var states = DataSetConverter.ConvertTo<DbState>(cityDataSet).ToArray();

            states.Should().NotBeNull();
            states.Should().BeEmpty();

        }

        [Test]
        public void DataSetConverter_PersonDataSet_GeneratesRecord()
        {
            DataSet peopleDataSet = new DataSet();

            DataTable dt = new DataTable();
            dt.Columns.Add("PersonId", typeof(int));
            dt.Columns.Add("first_name", typeof(string));
            dt.Columns.Add("last_name", typeof(string));
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Population", typeof(int));
            dt.Columns.Add("AdditionalColumn", typeof(int));
            dt.Columns.Add("Extra data", typeof(int));

            DataRow dr = dt.NewRow();
            dr["PersonId"] = 1;
            dr["first_name"] = "Robert";
            dr["last_name"] = "MeDiro";
            dr["Id"] = DBNull.Value;
            dr["Name"] = DBNull.Value;
            dr["Population"] = DBNull.Value;
            dr["AdditionalColumn"] = 69;
            dr["Extra data"] = 50;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["PersonId"] = 2;
            dr["first_name"] = "Juro";
            dr["last_name"] = "Tigaguro";
            dr["Id"] = 1;
            dr["Name"] = "Frist city";
            dr["Population"] = 1000;
            dr["AdditionalColumn"] = 69;
            dr["Extra data"] = 50;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["PersonId"] = 3;
            dr["first_name"] = "Maja";
            dr["last_name"] = "Jemala";
            dr["Id"] = 1;
            dr["Name"] = "Frist city";
            dr["Population"] = 1000;
            dr["AdditionalColumn"] = 69;
            dr["Extra data"] = 50;
            dt.Rows.Add(dr);

            dt.AcceptChanges();

            peopleDataSet.Tables.Add(dt);
            peopleDataSet.AcceptChanges();

            var people = DataSetConverter.ConvertTo<DbPerson>(peopleDataSet).ToArray();

            people.Should().NotBeNull();
            people.Should().HaveCount(3);

            people[0].FirstName.Should().Be("Robert");
            people[0].City.Should().BeNull();

            people[1].FirstName.Should().Be("Juro");
            people[1].City.Should().NotBeNull();
            people[1].City!.Id.Should().Be(1);
        }

        [Test]
        public void DataSetConverter_IgnoresValues_GeneratesRecord()
        {
            DataSet peopleDataSet = new DataSet();

            DataTable dt = new DataTable();
            dt.Columns.Add("PersonId", typeof(int));
            dt.Columns.Add("first_name", typeof(string));
            dt.Columns.Add("last_name", typeof(string));
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Population", typeof(int));
            dt.Columns.Add("AdditionalColumn", typeof(int));
            dt.Columns.Add("Extra data", typeof(int));

            DataRow dr = dt.NewRow();
            dr["PersonId"] = 1;
            dr["first_name"] = "Robert";
            dr["last_name"] = "MeDiro";
            dr["Id"] = DBNull.Value;
            dr["Name"] = DBNull.Value;
            dr["Population"] = DBNull.Value;
            dr["AdditionalColumn"] = 69;
            dr["Extra data"] = 50;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["PersonId"] = 2;
            dr["first_name"] = "Juro";
            dr["last_name"] = "Tigaguro";
            dr["Id"] = 1;
            dr["Name"] = "Frist city";
            dr["Population"] = 1000;
            dr["AdditionalColumn"] = 69;
            dr["Extra data"] = 50;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["PersonId"] = 3;
            dr["first_name"] = "Maja";
            dr["last_name"] = "Jemala";
            dr["Id"] = 1;
            dr["Name"] = "Frist city";
            dr["Population"] = 1000;
            dr["AdditionalColumn"] = 69;
            dr["Extra data"] = 50;
            dt.Rows.Add(dr);

            dt.AcceptChanges();

            peopleDataSet.Tables.Add(dt);
            peopleDataSet.AcceptChanges();

            var people = DataSetConverter.ConvertTo<DbPerson>(peopleDataSet).ToArray();

            people.Should().NotBeNull();
            people.Should().HaveCount(3);

            people[0].LastName.Should().Be("");
            people[0].City.Should().BeNull();

            people[1].LastName.Should().Be("");
            people[1].City.Should().NotBeNull();
            people[1].City!.Id.Should().Be(1);
        }
    }
}
