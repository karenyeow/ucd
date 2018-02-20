using Newtonsoft.Json;

namespace UCD.Model.Drools.V1
{
    public class FireAllRulesCommandClass
    {
        [JsonProperty("fire-all-rules")]
        public string FireAllRules { get; set; } = string.Empty;
    }
}