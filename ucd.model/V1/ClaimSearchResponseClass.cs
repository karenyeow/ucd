using System.Collections.Generic;

namespace UCD.Model.V1
{
    public class ClaimSearchResponseClass
    {
        public List<ClaimSearchResultResponseClass> results { get; set; } = new List<ClaimSearchResultResponseClass>();
    }

    public class ClaimSearchResultResponseClass
    {
        public string resultNum { get; set; }

        public string apiReceivedDateTime { get; set; }

        public string providerSnapshotDateTime { get; set; }

        public string managingInsCode { get; set; }

        public ClaimClass claim { get; set; }
    }
}