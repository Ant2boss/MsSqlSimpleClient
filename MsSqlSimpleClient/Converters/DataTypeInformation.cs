using MsSqlSimpleClient.Attributes.SqlTable;
using MsSqlSimpleClient.Collections;
using MsSqlSimpleClient.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MsSqlSimpleClient.Converters
{
    public sealed class DataTypeInformation
    {
        public DataTypeInformation(Type t)
        {
            this.OriginalType = t;

            var properties = t.GetProperties();

            this.Identity = this._GetIdentity(properties);
            this.IdentityColumnName = this._GetIdentityColumnName();
            this.Proprties = this._GetProperties(properties);
            this.RequiredProperties = this._GetRequired(properties);
            this.AdditionalCollections = this._GetExtendedCollections(properties);
        }

        public Type OriginalType { get; }

        public PropertyInfo? Identity { get; }
        public string? IdentityColumnName { get; }
        public IDictionary<string, PropertyInfo> Proprties { get; }
        public IDictionary<string, PropertyInfo> RequiredProperties { get; }

        public ISet<PropertyInfo> AdditionalCollections { get; }
        public bool ContainsAdditional => this.AdditionalCollections.Count > 0;

        private ISet<PropertyInfo> _GetExtendedCollections(PropertyInfo[] properties)
        {
            return properties.Where(p => p.GetCustomAttribute<SqlIgnoreAttribute>() is null).Where(p => p.GetType().IsEquivalentTo(typeof(GroupedCollection<>)) || p.GetCustomAttribute<SqlExtendedCollection>() is not null).ToHashSet();
        }

        private IDictionary<string, PropertyInfo> _GetRequired(PropertyInfo[] properties)
        {
            return properties
                .Where(p => p.GetCustomAttribute<SqlIgnoreAttribute>() is null)
                .Where(p => p.GetCustomAttribute<SqlRequiredAttribute>() is not null)
                .Where(prop =>
                {
                    //We ignore extended items for now
                    if (prop.GetCustomAttribute<SqlExtendedCollection>() is not null)
                    {
                        return false;
                    }

                    //If I cannot write to the property ignore it
                    if (prop.GetSetMethod() is null)
                    {
                        return false;
                    }

                    return true;
                })
                .ToDictionary(prop =>
                {
                    if (prop.GetCustomAttribute<SqlColumnNameAttribute>() is SqlColumnNameAttribute columnName)
                    {
                        return columnName.Name;
                    }

                    return prop.Name;
                });
        }

        private IDictionary<string, PropertyInfo> _GetProperties(PropertyInfo[] properties)
        {
            IDictionary<string, PropertyInfo> props = new Dictionary<string, PropertyInfo>();

            foreach (PropertyInfo prop in properties.Where(p => p.GetCustomAttribute<SqlIgnoreAttribute>() is null))
            {
                //We ignore extended items for now
                if (prop.GetCustomAttribute<SqlExtendedCollection>() is not null)
                {
                    continue;
                }

                //If I cannot write to the property ignore it
                if (prop.GetSetMethod() is null)
                {
                    continue;
                }

                if (prop.GetCustomAttribute<SqlColumnNameAttribute>() is SqlColumnNameAttribute columnName)
                {
                    props.Add(columnName.Name, prop);
                    continue;
                }

                props.Add(prop.Name, prop);
            }

            return props;
        }

        private string? _GetIdentityColumnName()
        {
            if (this.Identity?.GetCustomAttribute<SqlColumnNameAttribute>() is SqlColumnNameAttribute columnName)
            {
                return columnName.Name;
            }

            return this.Identity?.Name;
        }

        private PropertyInfo? _GetIdentity(PropertyInfo[] properties)
        {
            var value = properties.Where(p => p.GetCustomAttribute<SqlIdentityAttribute>() is not null).SingleOrDefault();

            if (value is null)
            {
                // throw new DataParserIdentityMissingException($"{this.OriginalType.Name} does not have an [SqlIdentity] attribute.");
                return null;
            }

            return value;
        }

    }
}
