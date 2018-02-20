using System.Collections.Generic;
using UCD.Model.Base;

namespace UCD.Model.V1
{
    public class PaymentResponseClass:BaseResponseClass
    {
        public List<PaymentClass> payment { get; set; }
    }
}