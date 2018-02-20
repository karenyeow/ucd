using System.ComponentModel.DataAnnotations;

namespace UCD.Model.V1
{
    public class ClaimSearchRequestClass
    {
        [Required(ErrorMessage= "searchScope is missing") ]
        public ClaimSearchScopeRequestClass searchScope { get; set; }

        [Required(ErrorMessage="claim is missing")]
        public ClaimSearchClaimRequestClass claim { get; set; }

    }


    public class ClaimSearchClaimRequestClass
    {
        [Required(ErrorMessage = "claimID is missing")]
        public string claimID { get; set; }
        [Required(ErrorMessage = "managingInsCode is missing")]
        public string managingInsCode { get; set; }
    }
    public class ClaimSearchScopeRequestClass
    {
        [Required(ErrorMessage = "includeNullClaim is missing. Valid values are Y or N")]
        public string includeNullClaim { get; set; }

        public int? minTier { get; set; }

    }


}