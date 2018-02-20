using Newtonsoft.Json;
using UCD.Model.V1;

namespace UCD.Model.Drools.V1
{
    public class InsertCTPPayPayloadObjCommandClass
    {
        [JsonProperty("claim")]
        public ClaimClass Claim { get; set; }

        [JsonProperty("payment")]
        public PaymentClass Payment { get; set; }
    }
}