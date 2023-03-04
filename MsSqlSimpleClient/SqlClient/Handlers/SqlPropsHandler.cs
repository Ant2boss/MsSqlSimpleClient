using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MsSqlSimpleClient.Attributes.Procedures;

namespace MsSqlSimpleClient.SqlClient.Handlers
{
    public static class SqlPropsHandler
    {
        /// <summary>
        /// Reflectivley reads through the props object in order to fill the data of the SQL parameters.
        /// </summary>
        /// <param name="collection">Colleciton to setup.</param>
        /// <param name="props">Props to read from.</param>
        public static void SetupParameters(SqlParameterCollection collection, object props)
        {
            Type t = props.GetType();

            foreach (PropertyInfo pi in t.GetProperties())
            {
                string paramName = $"@{pi.Name}";
                SqlParameter sqlParameter;

                if (pi.GetCustomAttribute<SqlParameterNameAttribute>() is SqlParameterNameAttribute sqlParameterName && collection.Contains($"@{sqlParameterName.ParameterName}"))
                {
                    sqlParameter = collection[$"@{sqlParameterName.ParameterName}"];
                }
                else if (collection.Contains(paramName))
                {
                    sqlParameter = collection[paramName];
                }
                else
                {
                    continue;
                }

                sqlParameter.Direction = System.Data.ParameterDirection.Input;
                sqlParameter.Value = pi.GetValue(props);

                //Check if property is output
                var isOutput = pi.GetCustomAttribute<SqlOutputAttribute>();

                if (isOutput is not null)
                {
                    sqlParameter.Direction = System.Data.ParameterDirection.Output;
                }
            }

        }

        /// <summary>
        /// Reflectevley loops thorugh all the properties which should act like outputs and returns the values to them.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="props"></param>
        public static void ReadOutputParameters(SqlParameterCollection collection, object props)
        {
            Type t = props.GetType();

            foreach (PropertyInfo pi in t.GetProperties())
            {
                //Check if property is output
                var isOutput = pi.GetCustomAttribute<SqlOutputAttribute>();

                if (isOutput is null)
                {
                    continue;
                }

                string paramName = $"@{pi.Name}";
                SqlParameter sqlParameter;

                if (pi.GetCustomAttribute<SqlParameterNameAttribute>() is SqlParameterNameAttribute sqlParameterName && collection.Contains($"@{sqlParameterName.ParameterName}"))
                {
                    sqlParameter = collection[$"@{sqlParameterName.ParameterName}"];
                }
                else if (collection.Contains(paramName))
                {
                    sqlParameter = collection[paramName];
                }
                else
                {
                    continue;
                }

                pi.SetValue(props, sqlParameter.Value);
            }
        }
    }
}
