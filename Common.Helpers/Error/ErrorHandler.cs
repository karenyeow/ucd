using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace Comlib.Common.Helpers.Error
{
    public class ErrorHandler
    {
        public static void PopulateExceptionWithCommandParameterData(Exception ex, DbCommand cmd)
        {
            if (cmd != null)
            {
                AddExceptionDataValue(ex, "CommandType", cmd.CommandType.ToString());
                //if (cmd.Connection != null)
                //{
                //    AddExceptionDataValue(ex, "ConnectionString", cmd.Connection.ConnectionString);
                //}
                AddExceptionDataValue(ex, "CommandText", cmd.CommandText);
                if (cmd.Parameters != null)
                {
                    foreach (SqlParameter parameter in cmd.Parameters)
                    {
                        AddExceptionDataValue(ex, parameter.ParameterName, parameter.Value);
                    }
                }
            }
            else
            {
                AddExceptionDataValue(ex, "Additional Info: ", "Failed to create command object.");
            }
        }
        private static void AddExceptionDataValue(Exception ex, object key, object value)
        {

            if (!ex.Data.Contains(key))
            {
                ex.Data.Add(key, (value as string)?.Trim());
            }
        }
    }
}
