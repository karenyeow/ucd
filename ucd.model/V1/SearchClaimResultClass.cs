using System;

namespace UCD.Model.V1
{
    public class SearchClaimResultClass
    {
        public DateTime receivedDateTime { get; set; }

        public string providerSnapshotDateTime { get; set; }

        public string managingInsCode { get; set; }

        public string JSONText { get; set; }
    }
}
