using Newtonsoft.Json;

namespace UCD.Model.Drools.V1
{
    public class InsertCTPClaimPayCommandClass:BaseInsertCommandClass 
    {
        [JsonProperty("object")]
        public InsertCTPClaimPayObjCommandClass InsertObject { get; set; } = new InsertCTPClaimPayObjCommandClass();
    }
}