using System;
using System.Collections.Generic;

namespace DHL.Report.TimeAttendance.Models
{
    public class EmployeeResultModel
    {
        public DateTime? DataDate { get; set; }
        public IList<EmployeeInfoModel> Employees { get; set; }
    }

    public class EmployeeInfoModel
    {
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
    }

    public class EmployeeSwipeInfoModel
    {
        public string EmployeeId { get; set; }
        public bool IsOut { get; set; }
        public DateTime ReadDate { get; set; }
    }
}
