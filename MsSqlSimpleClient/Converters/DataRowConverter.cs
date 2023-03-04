using MsSqlSimpleClient.Collections;
using MsSqlSimpleClient.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsSqlSimpleClient.Converters
{
    public sealed class DataRowConverter
    {
        public static T CreateInstanceOfData<T>(DataTypeInformation dataTypeInformation)
        {
            T result = Activator.CreateInstance<T>();

            // For every GroupedCollection property instantiate my special collection to help with item loading.
            foreach (var prop in dataTypeInformation.AdditionalCollections)
            {
                var genericCollectionType = typeof(GroupedCollection<>);
                var concreteCollectionType = genericCollectionType.MakeGenericType(prop.PropertyType.GenericTypeArguments[0]);

                prop.SetValue(result, Activator.CreateInstance(concreteCollectionType));
            }

            return result;
        }

        public static T ProcessDataRow<T>(DataRow dataRow, DataTypeInformation dataTypeInformation)
        {
            T result = CreateInstanceOfData<T>(dataTypeInformation);

            foreach (var prop in dataTypeInformation.Proprties)
            {
                if (dataRow.Table.Columns.Contains(prop.Key))
                {
                    if (dataRow[prop.Key] is not null && dataRow[prop.Key] != DBNull.Value)
                    {
                        prop.Value.SetValue(result, dataRow[prop.Key]);
                    }
                }
                else if (dataTypeInformation.RequiredProperties.ContainsKey(prop.Key))
                {
                    throw new DataParserRequiredColumnMissingException($"Provided data table contains no column with the name '{prop.Key}'. Even though the coulmn is marked as required");
                }
            }

            return result;
        }

        public static object ProcessDataRow(DataRow dataRow, DataTypeInformation dataTypeInformation, Type t)
        {
            object result = Activator.CreateInstance(t)!;

            foreach (var prop in dataTypeInformation.Proprties)
            {
                if (dataRow.Table.Columns.Contains(prop.Key))
                {
                    if (dataRow[prop.Key] is not null && dataRow[prop.Key] != DBNull.Value)
                    {
                        prop.Value.SetValue(result, dataRow[prop.Key]);
                    }
                }
                else if (dataTypeInformation.RequiredProperties.ContainsKey(prop.Key))
                {
                    throw new DataParserRequiredColumnMissingException($"Provided data table contains no column with the name '{prop.Key}'. Even though the coulmn is marked as required");
                }
            }

            return result;
        }
    }
}
