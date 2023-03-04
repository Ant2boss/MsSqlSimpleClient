using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsSqlSimpleClient.Attributes.Conversion
{
    /// <summary>
    /// If a given attribute should be considered as collection. This collection will be generated from the additional table columns (if the items are present).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class SqlExtendedCollection : Attribute { }
}
