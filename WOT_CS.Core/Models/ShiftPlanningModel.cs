using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WOT_CS.Core.Models
{
    public class ShiftPlanningModel
    {
        [DataMember(Name = "EmpCode")]
        public string EmpCode { get; set; }

        [DataMember(Name = "Date")]
        public string Date { get; set; }

        [DataMember(Name = "StartTime1")]
        public string StartTime1 { get; set; }

        [DataMember(Name = "EndTime1")]
        public string EndTime1 { get; set; }

        [DataMember(Name = "StartTime2")]
        public string StartTime2 { get; set; }

        [DataMember(Name = "EndTime2")]
        public string EndTime2 { get; set; }
    }
    public class ShiftPlanningDetailsModel
    {
        [DataMember(Name = "EmpCode")]
        public string EmpCode { get; set; }

        [DataMember(Name = "EffectiveDate")]
        public string EffectiveDate { get; set; }

        [DataMember(Name = "ExpectedTimeIn1")]
        public string ExpectedTimeIn1 { get; set; }

        [DataMember(Name = "ExpectedTimeOut1")]
        public string ExpectedTimeOut1 { get; set; }

        [DataMember(Name = "ExpectedTimeIn2")]
        public string ExpectedTimeIn2 { get; set; }

        [DataMember(Name = "ExpectedTimeOut2")]
        public string ExpectedTimeOut2 { get; set; }

        [DataMember(Name = "OFFDays")]
        public string OFFDays { get; set; }
    }
}
