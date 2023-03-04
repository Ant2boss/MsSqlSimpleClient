using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsSqlSimpleClient.Attributes.Conversion
{
    /// <summary>
    /// Identity is used to identify unique elements when trying to process a table into a collection or item.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class SqlIdentityAttribute : Attribute { }
}
