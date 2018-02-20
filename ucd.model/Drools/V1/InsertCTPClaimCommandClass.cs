using Newtonsoft.Json;

namespace UCD.Model.Drools.V1
{
    public class InsertCTPClaimCommandClass:BaseInsertCommandClass 
    {

        [JsonProperty("object")]
        public InsertCTPClaimObjCommandClass   InsertObject { get; set; }

    }
}