using System;
using System.Collections.Generic;
using System.Text;

namespace UCD.Model.V1
{
    public class SLAReferenceDataClass
    {

        public int Id { get; set; }

        public string Severity { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public string ErrorMechanism { get; set; }


        public int? SLA_Hour { get; set; }

        public int? SLA_Day { get; set; }

        public DateTime DXStartDate { get; set; }

        public DateTime DXEndDate { get; set; }


        public string DXIsDelete { get; set; }
    }
}
