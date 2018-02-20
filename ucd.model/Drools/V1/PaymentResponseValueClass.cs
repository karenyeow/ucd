using Newtonsoft.Json;

namespace UCD.Model.Drools.V1
{
    public class PaymentResponseValueClass: BaseResponseValueClass
    {
        [JsonProperty("sira.ucd.ctp.ctppayment.exception")]
        public override  ResponseExceptionClass ResponseException { get; set; }
    }
}