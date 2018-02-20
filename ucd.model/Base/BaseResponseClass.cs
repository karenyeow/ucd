using System;
using System.Collections.Generic;
using System.Text;

namespace UCD.Model.Base
{
    public abstract class BaseResponseClass
    {
        public string claimID { get; set; }

        public string providerProcessedDateTime { get; set; }

        public string submissionID { get; set; }

        public string receivedDateTime { get; set; }



        public List<ResponseExceptionClass> exception { get; set; }


    }
}
