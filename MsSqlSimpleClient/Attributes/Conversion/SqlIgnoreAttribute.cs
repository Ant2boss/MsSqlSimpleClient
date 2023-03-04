using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsSqlSimpleClient.Attributes.Conversion
{
    /// <summary>
    /// If a given property should be ignored on table parse.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class SqlIgnoreAttribute : Attribute { }
}
