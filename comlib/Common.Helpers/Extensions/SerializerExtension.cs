using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Common.Helpers.Extensions
{
    public enum SerializerType
    {
        DataContractSerializer,
        XmlSerializer,
    }

    public static class SerializerExtension
    {
        public static string Serialize<T>(this T obj, SerializerType serializerType = SerializerType.DataContractSerializer, IEnumerable<Type> knownTypes = null)
        {
            if (obj == null)
                return null;

            return serializerType == SerializerType.XmlSerializer ? SerializeUsingXmlSerializer(obj) : SerializeUsingDataContractSerializer(obj, knownTypes);
        }


        private static string SerializeUsingXmlSerializer<T>(this T obj)
        {
            using (var stream = new MemoryStream())
            {
                var xs = new XmlSerializer(obj.GetType());
                xs.Serialize(stream, obj);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        private static string SerializeUsingDataContractSerializer<T>(this T obj, IEnumerable<Type> knownTypes)
        {
            using (var writer = new StringWriter())
            {
                using (var xmlTextWriter = new XmlTextWriter(writer))
                {
                    var dataContractSerializer = new DataContractSerializer(typeof(T), knownTypes);
                    dataContractSerializer.WriteObject(xmlTextWriter, obj);
                    return writer.ToString();
                }
            }
        }

        public static string SerializeUsingDataContractSerializer(object obj, Type t)
        {
            using (var writer = new StringWriter())
            {
                using (var xmlTextWriter = new XmlTextWriter(writer))
                {
                    var dataContractSerializer = new DataContractSerializer(t);
                    dataContractSerializer.WriteObject(xmlTextWriter, obj);
                    return writer.ToString();
                }
            }
        }

        public static string JsonString(this object obj)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            return JsonConvert.SerializeObject(obj, jsonSerializerSettings);
        }

        public static T DeserializeJsonString<T>(this string jsonString)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            return JsonConvert.DeserializeObject<T>(jsonString, jsonSerializerSettings);
        }
    }
}
