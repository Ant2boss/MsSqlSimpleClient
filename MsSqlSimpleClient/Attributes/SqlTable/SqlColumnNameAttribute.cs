using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsSqlSimpleClient.Attributes.SqlTable
{
    /// <summary>
    /// Used to ovveride the default name for the column lookup when reading a table.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class SqlColumnNameAttribute : Attribute
    {
        public SqlColumnNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
