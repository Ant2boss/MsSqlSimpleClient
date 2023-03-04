using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsSqlSimpleClient.Converters
{
    public static class DataSetConverter
    {
        public static IEnumerable<T> ConvertTo<T>(this DataSet dataset, bool ignoreGrouping = false)
        {
            if (dataset is null || dataset.Tables.Count == 0)
            {
                yield break;
            }

            foreach (DataTable table in dataset.Tables)
            {
                foreach (T element in DataTableConverter.ConvertTo<T>(table, ignoreGrouping))
                {
                    yield return element;
                }
            }
        }
    }
}
