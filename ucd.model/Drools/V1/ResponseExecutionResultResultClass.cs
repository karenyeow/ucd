using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UCD.Model.Drools.V1
{
    public class ResponseExecutionResultResultClass
    {
        [JsonProperty("value")]
        public JArray  Value { get; set; }

        //public ResponseValuesClass Value { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }
    }
}