using Newtonsoft.Json;

namespace UCD.Model.Drools.V1
{
    public class ResponseClass
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("msg")]
        public string Msg { get; set; }

        [JsonProperty("result")]
        public ResponseResultClass Result { get; set; }
    }
}