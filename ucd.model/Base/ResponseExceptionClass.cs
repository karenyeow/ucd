using System;
using System.Collections.Generic;
using System.Text;

namespace UCD.Model.Base
{
    public class ResponseExceptionClass
    {
        public int exceptionID { get; set; }

        public int tier { get; set; }

        public string type { get; set; }

        public string description { get; set; }

        public string exceptionRaisedDateTime { get; set; }

        public string regulatoryRequirementDate { get; set; }

        public string rule { get; set; }

        public string exceptionReference { get; set; }
    }
}
