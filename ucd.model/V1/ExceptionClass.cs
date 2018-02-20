using System;

namespace UCD.Model.V1
{
    public class ExceptionClass
    {
        public string exceptionType { get; set; }
        public string sourceID { get; set; }

        public int sequenceID { get; set; }

        public int tier { get; set; }

        public string type { get; set; }

        public string rule { get; set; }

        public string description { get; set; }

        public bool closed { get; set; }

        public DateTime exceptionRaisedDateTime { get; set; }

        public DateTime regulatoryRequirementDate { get; set; }

        public DateTime closedDate { get; set; }

        public string node { get; set; }

        public string element { get; set; }

        public int? index { get; set; }

        public string exceptionReference { get; set; }

        public bool isDrools { get; set; }

        public string categorySLA { get; set; }
    }
}
