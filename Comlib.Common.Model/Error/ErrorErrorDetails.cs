using System;
using System.Collections.Generic;
using System.Text;

namespace Comlib.Common.Model.Error
{
    public class ErrorErrorDetails
    {
        public ErrorErrorDetails(string code, string description)
        {
            this.errorDetails = new ErrorDetail(code, description);
        }

        public ErrorDetail errorDetails { get; set; } = new ErrorDetail();

    }
}
