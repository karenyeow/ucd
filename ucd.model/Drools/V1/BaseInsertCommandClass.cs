using Newtonsoft.Json;

namespace UCD.Model.Drools.V1
{
    public class BaseInsertCommandClass
    {
        [JsonProperty("return-object")]
        public bool ReturnObject { get; set; }

        [JsonProperty("out-identifier")]
        public string OutIdentifier { get; set; }
    }
}