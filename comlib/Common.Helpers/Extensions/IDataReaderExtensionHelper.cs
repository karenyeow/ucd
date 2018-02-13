using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Comlib.Common.Helpers.Extensions
{
    public static class IDataReaderExtensionHelper
    {
        private static Dictionary<string, ConstructorInfo> _constructorCache = new Dictionary<string, ConstructorInfo>();
        private static Dictionary<string, PropertyInfo[]> _propertyInfoCache = new Dictionary<string, PropertyInfo[]>();
        private static Dictionary<string, Func<IDataReader, object>> _mappingCache = new Dictionary<string, Func<IDataReader, object>>();

        public static ConstructorInfo GetDefaultConstructor<T>()
        {
            Type type = typeof(T);

            if (!_constructorCache.ContainsKey(type.FullName))
            {
                _constructorCache.Add(type.FullName, type.GetConstructor(System.Type.EmptyTypes));
            }
            return _constructorCache[type.FullName];
        }

        private static T CreateInstance<T>()
        {
            Type type = typeof(T);

            ConstructorInfo constructor = GetDefaultConstructor<T>();
            return (T)constructor.Invoke(new object[0]);
        }

        static string GetMappingSignature(this IDataReader dr, Type type)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < dr.FieldCount; i++ )
            {
                builder.Append(dr.GetName(i));
            }

            builder.Append(type.FullName);

            return builder.ToString();
        }

        static Action<IDataReader, object> CreateSetValueAction(PropertyInfo info, Type columnType, string columnName)
        {
            Type propType = info.PropertyType;

            Action<IDataReader, object> SetValueAction;

            if (propType.BaseType.Equals(typeof(System.Enum)))
            {
                SetValueAction = (IDataReader dr, object obj) =>
                {
                    if (IDataReaderExtensionHelper.IsNumeric(dr[columnName]))
                    {
                        info.SetValue(obj, System.Enum.ToObject(propType, Convert.ToInt32(dr[columnName])), null);
                    }
                    else
                    {
                        if (!dr.GetString(columnName).IsNullOrEmptyAfterTrim() && Enum.IsDefined(propType, dr[columnName]))
                        {
                            object value = Enum.Parse(propType, dr[columnName].ToString());
                            info.SetValue(obj, value, null);
                        }
                    }
                };
            }
            else if (propType.Equals(columnType))
            {
                SetValueAction = (IDataReader dr, object obj) =>
                {
                    info.SetValue(obj, dr[columnName], null);
                };
            }
            else
            {
                //different types
                SetValueAction = (IDataReader dr, object obj) =>
                    {

                        Type t = Nullable.GetUnderlyingType(propType) ?? propType;
                        object safevalue = dr[columnName] == null ? null : Convert.ChangeType(dr[columnName], t);
                        info.SetValue(obj, safevalue, null);
                };
            }

            return (IDataReader dr, object obj) =>
            {
                if (dr.HasColumn(columnName))
                {
                    if (Convert.IsDBNull(dr[columnName]))
                    {
                        info.SetValue(obj, null, null);
                        //  info.SetValue(obj, Null.GetNull(info), null);
                    }
                    else
                    {
                        SetValueAction.Invoke(dr, obj);
                    }
                }
            };
        }

        static List<Action<IDataReader, object>> CreatePropertyMapping<T>(IDataReader dr)
        {
            List<PropertyInfo> properties = IDataReaderExtensionHelper.GetPropertyInfo<T>();
            List<Action<IDataReader, object>> mapping = new List<Action<IDataReader, object>>();

            int i;
            for (i = 0; i < dr.FieldCount; i++)
            {
                string columnName = dr.GetName(i);
                //now find matching property
                PropertyInfo propMatch = (from p in properties
                                          where p.Name.ToLower() == columnName.ToLower()
                                          select p).FirstOrDefault();
                if (propMatch != null)
                {
                    Type columnType = dr.GetFieldType(i);
                    Action<IDataReader, object> action = CreateSetValueAction(propMatch, columnType, columnName);
                    mapping.Add(action);
                }
            }
            return mapping;
        }

        static Func<IDataReader, object> CreateObjectMapping<T>(IDataReader dataReader)
        {
            List<Action<IDataReader, object>> mapping = CreatePropertyMapping<T>(dataReader);

            return (IDataReader dr) =>
            {
                object obj = CreateInstance<T>();

                for (int i = 0; i < mapping.Count; i++)
                {
                    Action<IDataReader, object> action = mapping[i];

                    action.Invoke(dr, obj);
                }

                return obj;
            };
        }

        public static T CreateObject<T>(IDataReader dr)
        {
            string mappingSignature = dr.GetMappingSignature(typeof(T));

            if (!_mappingCache.ContainsKey(mappingSignature))
            {
                _mappingCache.Add(mappingSignature, CreateObjectMapping<T>(dr));
            }

            return (T)_mappingCache[mappingSignature].Invoke(dr);
        }

        public static bool IsNumeric(object expression)
        {
            bool isNum;


            isNum = Double.TryParse(Convert.ToString(expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out double retNum);
            return isNum;
        }

        public static List<PropertyInfo> GetPropertyInfo<T>()
        {
            Type type = typeof(T);

            if (!_propertyInfoCache.ContainsKey(type.FullName))
            {
                _propertyInfoCache.Add(type.FullName, type.GetProperties());
            }

            return new List<PropertyInfo>(_propertyInfoCache[type.FullName]);
        }

        public static int ExecuteNonQuery<T>(IDbCommand command, T item)
        {
            IDataReaderExtensionHelper.FillParameters<T>(item, command);
            return command.ExecuteNonQuery();
        }

        public static void FillParameters<T>(object obj, IDbCommand command)
        {
            List<PropertyInfo> properties = GetPropertyInfo<T>();

            foreach (IDbDataParameter param in command.Parameters)
            {
                //find the right property
                PropertyInfo property = properties.Find(delegate(PropertyInfo p)
                {
                    return p.Name.ToLower() == param.SourceColumn.ToLower();
                });

                object value = property.GetValue(obj, null);

                if (value == null)
               // if (Null.IsNull(value))
                {
                    param.Value = DBNull.Value;
                }
                else
                {
                    param.Value = value;
                }
            }
        }
    }
}
