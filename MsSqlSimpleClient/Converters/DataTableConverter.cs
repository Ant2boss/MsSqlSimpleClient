using MsSqlSimpleClient.Collections;
using MsSqlSimpleClient.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MsSqlSimpleClient.Converters
{
    public static class DataTableConverter
    {
        private static ConcurrentDictionary<Type, DataTypeInformation> _cachedObjectTypes = new ConcurrentDictionary<Type, DataTypeInformation>();

        public static IEnumerable<T> ConvertTo<T>(this DataTable dataTable, bool ignoreGrouping = false)
        {
            if (dataTable is null || dataTable.Rows.Count == 0)
            {
                return Enumerable.Empty<T>();
            }

            if (ignoreGrouping == false)
            {
                GroupedCollection<T> collection = new GroupedCollection<T>();

                foreach (DataRow row in dataTable.Rows)
                {
                    collection.AddRow(row);
                }

                return collection;
            }

            var dataTypeInformation = _cachedObjectTypes.GetOrAdd(typeof(T), new DataTypeInformation(typeof(T)));
            return _ProcessDataTableToRowsNoGrouping<T>(dataTable, dataTypeInformation);
        }

        private static IEnumerable<T> _ProcessDataTableToRowsNoGrouping<T>(DataTable dataTable, DataTypeInformation dataTypeInformation)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                yield return DataRowConverter.ProcessDataRow<T>(row, dataTypeInformation);
            }
        }
    }
}
