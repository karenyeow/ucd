using System;
using System.Collections.Generic;
using System.Text;

namespace Comlib.Common.Model.Error
{
    public class ErrorDetail
    {
        public ErrorDetail()
        {

        }
        public ErrorDetail(string code, string description)
        {
            this.code = code;
            if (code.Equals("internal exception", System.StringComparison.CurrentCultureIgnoreCase))
            {
                this.description = "Internal Server Error";
            }
            else
            {
                this.description = description;
            }
        }
        public string code { get; private set; }
        public string description { get; private set; }
    }
}
