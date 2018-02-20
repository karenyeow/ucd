using Newtonsoft.Json;
using System.Collections.Generic;

namespace UCD.Model.Drools.V1
{
    public class ResponseExecutionResultClass
    {
       
        [JsonProperty("results")]
        public List<ResponseExecutionResultResultClass> ExecutionResultResult { get; set; }
    }
}