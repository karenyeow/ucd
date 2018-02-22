using System;
using System.Collections.Generic;
using System.Text;

namespace Comlib.Common.Model.Exceptions
{
    public class AppException:ApplicationException 
    {
        public AppException (string code, string message): base(message)
        {
            this.Code = code;
        }

        public string Code { get; set; }

    }
}
