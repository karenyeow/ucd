using UCD.Model.Base;

namespace UCD.Model.V1
{
    public class ClaimResponseClass: BaseResponseClass 
    {
        public ClaimClass claim { get; set; }
    }
}