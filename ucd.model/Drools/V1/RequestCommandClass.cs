using Newtonsoft.Json;

namespace UCD.Model.Drools.V1
{
    public class RequestCommandClass
    {
        [JsonProperty("set-global")]
        public SetGlobalCommandClass SetGlobalCommand { get; set; }

        [JsonProperty("insert")]
        public BaseInsertCommandClass  InsertCommand { get; set; }

        [JsonProperty("fire-all-rules")]
        public string FireAllRules { get; set; } = null;

        [JsonProperty("get-objects")]
        public GetObjectsCommandClass GetObjects { get; set; }


    }
}