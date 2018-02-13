using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Comlib.Common.Helpers.Utils
{
public static    class DictionaryUtils
    {
        public static Dictionary<string, object> AnonToDictionary(object anonObject)
        {
            var dictionary = new Dictionary<string, object>();
            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(anonObject))
            {
                object obj = propertyDescriptor.GetValue(anonObject);
                dictionary.Add(propertyDescriptor.Name, obj);
            }
            return dictionary;
        }
    }
}
