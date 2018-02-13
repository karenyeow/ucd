
using System;
namespace Comlib.Common.Helpers.Utils
{
        [Serializable]
 public class TextValuePair
    {
            public TextValuePair()
            {
                
            }

            public TextValuePair(string text, string value)
            {
                Text = text;
                Value = value;
            }
        public  const  string FieldValueName = "Value";
        public const  string FieldTextName = "Text";
         public string Value { get; set; }
         public string Text { get; set; }
     }
}
