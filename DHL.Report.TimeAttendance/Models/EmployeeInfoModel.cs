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
        public string SwipeCode { get; set; }
        public bool IsOut { get; set; }
        public DateTime ReadDate { get; set; }
    }

    public class EmployeeReportResultModel
    {
        public int ReportMonth { get; set; }
        public int ReportYear { get; set; }
        public IEnumerable<EmployeeReportModel> Employees { get; set; }
    }

    public class EmployeeReportModel
    {
        public EmployeeInfoModel Info { get; set; }
        public IEnumerable<EmployeeReportItemModel> Items { get; set; }
        public string SwipeCode { get; set; }
        public string ShiftCode { get; set; }
        public string ShiftName { get; set; }
    }

    public class EmployeeReportItemModel
    {
        public DateTime In { get; set; }
        public DateTime Out { get; set; }
        public TimeSpan WorkingTime
        {
            get { return Out.Subtract(In); }
        }
    }
}
