using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;
namespace Comlib.Common.Helpers.Extensions
{
    public static class IDataReaderExtensions 
    {

        public static IEnumerable<T> Select<T>(this IDataReader reader,
                               Func<IDataReader, T> projection)
        {
            while (reader.Read())
            {
                yield return projection(reader);
            }
        }
        public static T NextSingle<T>(this IDataReader reader,
                               Func<IDataReader, T> projection)
        {
            reader.Read();
            return projection(reader);
        }

        public static T  Single<T>(this IDataReader reader,
                       Func<IDataReader, T> projection)
        {
            return projection(reader);
        }
        public static string GetString(this IDataReader reader, string name)
        {
            return reader.IsDBNull(reader.GetOrdinal(name)) ? string.Empty : reader.GetString(reader.GetOrdinal(name));
        }

        public static string GetString(this IDataReader reader, int index)
        {
            return reader.IsDBNull(index) ? string.Empty : reader.GetString(index);
        }

        public static int? GetInt32(this IDataReader reader, string name)
        {
            return reader.IsDBNull(reader.GetOrdinal(name)) ? null : (int?) reader.GetInt32(reader.GetOrdinal(name));
        }

        public static DateTime? GetDateTime(this IDataReader reader, string name)
        {
            return reader.IsDBNull(reader.GetOrdinal(name)) ? null : (DateTime?) reader.GetDateTime(reader.GetOrdinal(name));
        }

        public static bool? GetBoolean(this IDataReader reader, string name)
        {
            return reader.IsDBNull(reader.GetOrdinal(name)) ? null : (bool?) reader.GetBoolean(reader.GetOrdinal(name));
        }

        public static decimal? GetDecimal(this IDataReader reader, string name)
        {
            return reader.IsDBNull(reader.GetOrdinal(name)) ? null : (decimal?)reader.GetDecimal(reader.GetOrdinal(name));
        }
        public static T GetValue<T>(this IDataReader reader, string name) where T : class
        {
            return reader.IsDBNull(reader.GetOrdinal(name)) ? null : (T)reader.GetValue(reader.GetOrdinal(name));
        }

        public static bool HasColumn(this IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        public static List<T> FillCollection<T>(this IDataReader dr, bool closeDataReader = true)
        {
            List<T> list = new List<T>();

            while (dr.Read())
            {
                T obj = IDataReaderExtensionHelper.CreateObject<T>(dr);
                list.Add(obj);
            }

            if (closeDataReader)
            {
                dr.Close();
            }

            return list;
        }

        public static T Single<T>(this IDataReader dr, bool closeDataReader = true)
        {
            T obj = default(T);

            while (dr.Read())
            {
                obj = IDataReaderExtensionHelper.CreateObject<T>(dr);

            }

            if (closeDataReader)
            {
                dr.Close();
            }

            return obj;
        }

        public static double? GetDouble(this IDataReader reader, string name)
        {
            return reader.IsDBNull(reader.GetOrdinal(name)) ? null : (double?)reader.GetDouble(reader.GetOrdinal(name));
        }
    }
}
