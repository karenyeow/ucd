using Comlib.Common.Framework.Attributes;
using System.ComponentModel.DataAnnotations;
using UCD.Model.Base;

namespace UCD.Model.V1
{
    public class EmploymentClass : BaseSiraClass
    {
        [Display(Name = @"employment\averageWeeklyEarnings")]
        [ValidPrecision(10, 2)]
        public string averageWeeklyEarnings { get; set; }

        [Display(Name = @"employment\employer")]
        [ValidLengthLimit(150)]
        public string employer { get; set; }

        [Display(Name = @"employment\employerABN")]
        [ValidLengthLimit(11)]
        public string employerABN { get; set; }

        [Display(Name = @"employment\occupation")]
        [ValidLengthLimit(50)]
        public string occupation { get; set; }


        [Display(Name = @"employment\employerSuburb")]
        [ValidLengthLimit(100)]
        public string employerSuburb { get; set; }
       

    }
}