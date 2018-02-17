using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Comlib.Common.Helpers.Extensions
{
    public static  class SQLClientExtensions
    {
        public static SqlParameter AddOutputParameter (this SqlParameterCollection sqlParameters, string parameterName, System.Data.SqlDbType  dbType) 
        {
            var outputParam = new SqlParameter(parameterName, dbType)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            sqlParameters.Add(outputParam);
            return outputParam;
        }

        public static object ToDbNull(object value)
        {
            if (value != null)
            {
                return value;
            }

            return DBNull.Value;
        }



        public static void CloseAndDispose(this  SqlConnection  dataConnection)
        {
            if (dataConnection != null && dataConnection.State == System.Data.ConnectionState.Open)
            {
                dataConnection.Dispose();
                dataConnection.Close();
            }
        }



        public static void CloseAndDispose(this  SqlDataReader dataReader)
        {
            if (dataReader != null)
            {
                dataReader.Close();
                dataReader.Dispose();
            }
        }
    }
}
