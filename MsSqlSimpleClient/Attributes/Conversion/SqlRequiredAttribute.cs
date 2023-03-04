using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsSqlSimpleClient.Attributes.Conversion
{
    /// <summary>
    /// If a given property is required. If a required property is not located in the table, a <see cref="Exceptions.DataParserRequiredColumnMissingException"/> will be thrown.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class SqlRequiredAttribute : Attribute { }
}
