using Newtonsoft.Json;

namespace UCD.Model.Drools.V1
{
    public class SetGlobalCommandClass
    {
        public string identifier { get; set; } = "ruleFactDate";

        [JsonProperty("object")]
        public JavaUtilDateCommandClass JavaUtilDateCommand { get; set; } = new JavaUtilDateCommandClass();
    }
} 