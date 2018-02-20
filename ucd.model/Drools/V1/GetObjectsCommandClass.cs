using Newtonsoft.Json;

namespace UCD.Model.Drools.V1
{
    public class GetObjectsCommandClass
    {
        [JsonProperty("out-identifier")]
        public string OutIdentifier { get; set; } = string.Empty;
    }
}