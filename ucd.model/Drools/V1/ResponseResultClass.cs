using Newtonsoft.Json;

namespace UCD.Model.Drools.V1
{
    public class ResponseResultClass
    {
        [JsonProperty("execution-results")]
        public ResponseExecutionResultClass ExecutionResult { get; set; }
    }
}