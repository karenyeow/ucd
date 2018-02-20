using System.Collections.Generic;

namespace UCD.Model.Drools.V1
{
    public class RequestClass
    {
        public string lookup { get; set; } = "RuleValidationSession";
        public List<RequestCommandClass> commands { get; set; } = new List<RequestCommandClass>();
    }
}