using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UCD.Model.V1
{
    public class ClearExceptionRequestClass
    {
        [Required(ErrorMessage = "exceptionID is missing")]
        public List<string> exceptionID { get; set; }
    }
}