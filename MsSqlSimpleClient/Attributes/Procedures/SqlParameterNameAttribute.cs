﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsSqlSimpleClient.Attributes.Procedures
{
    /// <summary>
    /// Specify the name to use when converting the object to property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class SqlParameterNameAttribute : Attribute
    {
        public SqlParameterNameAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }

        public string ParameterName { get; }
    }
}
