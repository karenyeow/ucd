using Newtonsoft.Json;

namespace UCD.Model.Drools.V1
{
    public class InsertCTPClaimPayObjCommandClass
    {
        [JsonProperty("paymentPayload")]
        public InsertCTPPayPayloadObjCommandClass paymentPayload { get; set; } = new InsertCTPPayPayloadObjCommandClass();
    }
}