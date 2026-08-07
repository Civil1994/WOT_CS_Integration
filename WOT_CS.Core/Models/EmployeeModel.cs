using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WOT_CS.Core.Models
{
    public class EmployeeModel
    {
        public string PersonnelNo { get; set; }
        public string CheckInId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Agency { get; set; }
        public string Status { get; set; }
        public string Department { get; set; }
        public string RoleName { get; set; }
        public string Title { get; set; }
        public string JobBand { get; set; }
        public string Casual { get; set; }
        public DateTime? StartDay { get; set; }
        public DateTime? EndDay { get; set; }
        public string Division { get; set; }

        public string WorkingPlace { get; set; }
    }
}
