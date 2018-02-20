using Newtonsoft.Json;
using UCD.Model.V1;

namespace UCD.Model.Drools.V1
{
    public class InsertCTPClaimObjCommandClass
    {
        [JsonProperty("claim")]
        public ClaimClass Claim { get; set; }
    }
}