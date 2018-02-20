using Newtonsoft.Json;
using System;

namespace UCD.Model.Drools.V1
{
    public class JavaUtilDateCommandClass
    {


        [JsonProperty("java.util.Date")]
        public string JavaUtilDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
    }
}