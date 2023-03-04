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
    public sealed class DataSetConverter_NoGroupingTest
    {
        [Test]
        public void DataSetConverter_ValidDataSet_GeneratesThreeRecords()
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

            var cities = DataSetConverter.ConvertTo<DbCity>(cityDataSet, ignoreGrouping: true).ToArray();

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
        public void DataSetConverter_EmptyDataSetTable_GeneratesEmptyArray()
        {
            DataSet cityDataSet = new DataSet();

            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Population", typeof(int));

            dt.AcceptChanges();

            cityDataSet.Tables.Add(dt);
            cityDataSet.AcceptChanges();

            var cities = DataSetConverter.ConvertTo<DbCity>(cityDataSet, ignoreGrouping: true).ToArray();

            cities.Should().HaveCount(0);
        }

        [Test]
        public void DataSetConverter_EmptyDataSet_GeneratesEmptyArray()
        {
            DataSet cityDataSet = new DataSet();
            cityDataSet.AcceptChanges();

            var cities = DataSetConverter.ConvertTo<DbCity>(cityDataSet, ignoreGrouping: true).ToArray();

            cities.Should().HaveCount(0);
        }

        [Test]
        public void DataSetConverter_JankyButValidDataSet_GeneratesFourRecords()
        {
            DataSet cityDataSet = new DataSet();

            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Population", typeof(int));

            DataRow dr = dt.NewRow();
            dr["Id"] = 1;
            dr["Name"] = DBNull.Value;
            dr["Population"] = 1000;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 2;
            dr["Name"] = "";
            dr["Population"] = 0;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 3;
            dr["Name"] = "Third city";
            dr["Population"] = DBNull.Value;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = DBNull.Value;
            dr["Name"] = DBNull.Value;
            dr["Population"] = DBNull.Value;
            dt.Rows.Add(dr);

            dt.AcceptChanges();

            cityDataSet.Tables.Add(dt);
            cityDataSet.AcceptChanges();

            var cities = DataSetConverter.ConvertTo<DbCity>(cityDataSet, true).ToArray();

            cities.Should().HaveCount(4);

            cities[0].Should().BeOfType<DbCity>();
            cities[1].Should().BeOfType<DbCity>();
            cities[2].Should().BeOfType<DbCity>();
            cities[3].Should().BeOfType<DbCity>();

            cities[0].Id.Should().Be(1);
            cities[1].Id.Should().Be(2);
            cities[2].Id.Should().Be(3);
            cities[3].Id.Should().Be(0);

            cities[0].Name.Should().Be("");
            cities[1].Name.Should().Be("");
            cities[2].Name.Should().Be("Third city");
            cities[3].Name.Should().Be("");

            cities[0].Papilation.Should().Be(1000);
            cities[1].Papilation.Should().Be(0);
            cities[2].Papilation.Should().Be(0);
            cities[3].Papilation.Should().Be(0);
        }

        [Test]
        public void DataSetConverter_DataSetMissingNonRequiredColumns_GeneratesFourRecords()
        {
            DataSet cityDataSet = new DataSet();

            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));

            DataRow dr = dt.NewRow();
            dr["Id"] = 1;
            dr["Name"] = "First city";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 2;
            dr["Name"] = "Second city";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 3;
            dr["Name"] = "Third city";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 4;
            dr["Name"] = "Fourth city";
            dt.Rows.Add(dr);

            dt.AcceptChanges();

            cityDataSet.Tables.Add(dt);
            cityDataSet.AcceptChanges();

            var cities = DataSetConverter.ConvertTo<DbCity>(cityDataSet, ignoreGrouping: true).ToArray();

            cities.Should().HaveCount(4);

            cities[0].Should().BeOfType<DbCity>();
            cities[1].Should().BeOfType<DbCity>();
            cities[2].Should().BeOfType<DbCity>();
            cities[3].Should().BeOfType<DbCity>();

            cities[0].Id.Should().Be(1);
            cities[1].Id.Should().Be(2);
            cities[2].Id.Should().Be(3);
            cities[3].Id.Should().Be(4);

            cities[0].Name.Should().Be("First city");
            cities[1].Name.Should().Be("Second city");
            cities[2].Name.Should().Be("Third city");
            cities[3].Name.Should().Be("Fourth city");

            cities[0].Papilation.Should().Be(0);
            cities[1].Papilation.Should().Be(0);
            cities[2].Papilation.Should().Be(0);
            cities[3].Papilation.Should().Be(0);
        }

        [Test]
        public void DataSetConverter_DataSetMissingRequiredColumns_ThrowsException()
        {
            DataSet cityDataSet = new DataSet();

            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Population", typeof(string));

            DataRow dr = dt.NewRow();
            dr["Id"] = 1;
            dr["Population"] = 1000;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 2;
            dr["Population"] = 2000;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 3;
            dr["Population"] = 3000;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 4;
            dr["Population"] = 4000;
            dt.Rows.Add(dr);

            dt.AcceptChanges();

            cityDataSet.Tables.Add(dt);
            cityDataSet.AcceptChanges();

            Assert.Throws<DataParserRequiredColumnMissingException>(() => DataSetConverter.ConvertTo<DbCity>(cityDataSet, ignoreGrouping: true).ToArray());
        }

        [Test]
        public void DataSetConverter_ObjectMissingIdentityColumn_GeneratesThreeRecords()
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

            var citites = DataSetConverter.ConvertTo<BrokenDbCity>(cityDataSet, ignoreGrouping: true).ToArray();

            citites.Should().NotBeNull();
            citites.Should().HaveCount(3);

            citites[0].Should().NotBeNull();
            citites[1].Should().NotBeNull();
            citites[2].Should().NotBeNull();

            citites[0].Name.Should().Be("First city");
            citites[1].Name.Should().Be("Second city");
            citites[2].Name.Should().Be("Third city");
        }

        [Test]
        public void DataSetConverter_DataSetWithInvalidColumns_GeneratesThreeRecords()
        {
            DataSet cityDataSet = new DataSet();

            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Papilation", typeof(int));

            DataRow dr = dt.NewRow();
            dr["Id"] = 1;
            dr["Name"] = "First city";
            dr["Papilation"] = 1000;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 2;
            dr["Name"] = "Second city";
            dr["Papilation"] = 2000;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 3;
            dr["Name"] = "Third city";
            dr["Papilation"] = 3000;
            dt.Rows.Add(dr);

            dt.AcceptChanges();

            cityDataSet.Tables.Add(dt);
            cityDataSet.AcceptChanges();

            var cities = DataSetConverter.ConvertTo<DbCity>(cityDataSet, ignoreGrouping: true).ToArray();

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

            cities[0].Papilation.Should().Be(0);
            cities[1].Papilation.Should().Be(0);
            cities[2].Papilation.Should().Be(0);
        }

        [Test]
        public void DataSetConverter_DataSetWithInvalidDataType_ThrowsException()
        {
            DataSet cityDataSet = new DataSet();

            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Population", typeof(string));

            DataRow dr = dt.NewRow();
            dr["Id"] = 1;
            dr["Name"] = "First city";
            dr["Population"] = "hmm...";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 2;
            dr["Name"] = "Second city";
            dr["Population"] = "hmm...";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 3;
            dr["Name"] = "Third city";
            dr["Population"] = "hmm...";
            dt.Rows.Add(dr);

            dt.AcceptChanges();

            cityDataSet.Tables.Add(dt);
            cityDataSet.AcceptChanges();

            Assert.Throws<ArgumentException>(() => DataSetConverter.ConvertTo<DbCity>(cityDataSet, ignoreGrouping: true).ToArray());

        }
    }
}
