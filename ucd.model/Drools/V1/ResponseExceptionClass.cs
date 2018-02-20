using Newtonsoft.Json;

namespace UCD.Model.Drools.V1
{
    public class ResponseExceptionClass
    {
        [JsonProperty("rule")]
        public string Rule { get; set; }

        [JsonProperty("shortDescription")]
        public string ShortDescription { get; set; }

        [JsonProperty("slaCategory")]
        public string SLACategory { get; set; }

        [JsonProperty("tier")]
        public int Tier { get; set; }


        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("exceptionReference")]
        public string exceptionReference { get; set; }
    }
}