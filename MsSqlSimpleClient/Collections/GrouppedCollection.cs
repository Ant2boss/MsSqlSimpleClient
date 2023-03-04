using MsSqlSimpleClient.Converters;
using MsSqlSimpleClient.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MsSqlSimpleClient.Collections
{
    public sealed class GroupedCollection<T> : IEnumerable<T>
    {
        private readonly DataTypeInformation _dataTypeInformation;
        private readonly Dictionary<object, T> _values;

        public GroupedCollection()
        {
            this._dataTypeInformation = new DataTypeInformation(typeof(T));
            this._values = new Dictionary<object, T>();
        }

        public void AddRow(DataRow row)
        {
            if (this._dataTypeInformation.Identity is null || this._dataTypeInformation.IdentityColumnName is null)
            {
                throw new DataParserIdentityMissingException($"{this._dataTypeInformation.OriginalType.Name} does not have an [SqlIdentity] attribute.");
            }

            if (row.Table.Columns.Contains(this._dataTypeInformation.IdentityColumnName) == false)
            {
                throw new DataParserIdentityMissingException($"Data row does not contain the identity column with the name: {this._dataTypeInformation.IdentityColumnName}");
            }

            object identity = row[this._dataTypeInformation.IdentityColumnName];

            if (identity == DBNull.Value)
            {
                return;
            }

            if (this._values.ContainsKey(identity) == false)
            {
                this._values.Add(identity, DataRowConverter.ProcessDataRow<T>(row, this._dataTypeInformation));
            }

            foreach (var additionalCollection in this._dataTypeInformation.AdditionalCollections)
            {
                T value = this._values[identity];
                object? collection = additionalCollection.GetValue(value);
                try
                {
                    collection!
                        .GetType()
                        .GetMethod(nameof(AddRow))!
                        .Invoke(collection, new object[] { row });
                }
                catch (TargetInvocationException ex) when (ex.InnerException is DataParserRequiredColumnMissingException)
                {
                    throw ex.InnerException;
                }
            }
        }

        public IEnumerator<T> GetEnumerator() => this._values.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this._values.Values.GetEnumerator();
    }
}
