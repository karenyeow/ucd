using Newtonsoft.Json;

namespace UCD.Model.Drools.V1
{
    public class ClaimResponseValueClass: BaseResponseValueClass 
    {
        [JsonProperty("sira.ucd.ctp.ctpclaim.exception")]
        public override  ResponseExceptionClass ResponseException { get; set; }
    }
}