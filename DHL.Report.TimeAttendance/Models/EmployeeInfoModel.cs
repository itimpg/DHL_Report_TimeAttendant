using System;
using System.Collections.Generic;
using System.Linq;

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
        public string ShiftCode { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
    }

    public class EmployeeSwipeInfoModel
    {
        public string EmployeeId { get; set; }
        public DateTime DateIn { get; set; }
        public DateTime DateOut { get; set; }
        public TimeSpan WorkingTime { get; set; }
    }

    public class EmployeeReportResultModel
    {
        public DateTime ReportMonth { get; set; }
        public IList<EmployeeReportCompanyModel> Companies { get; set; }
    }

    public class EmployeeReportCompanyModel
    {
        public string Company { get; set; }
        public IList<EmployeeReportModel> Employees { get; set; }
    }

    public class EmployeeReportModel
    {
        public DateTime ReportDate { get; set; }
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string ShiftCode { get; set; }
        public string ShiftName { get; set; }

        public IList<EmployeeReportSwipeModel> WorkingSwipes { get; set; }
        public TimeSpan TotalWorkingTime
        {
            get
            {
                if (WorkingSwipes == null)
                    return TimeSpan.Zero;
                return WorkingSwipes.Aggregate(TimeSpan.Zero, (time, x) => time + x.WorkingTime);
            }
        }
        public TimeSpan TotalNotWorkingTime
        {
            get
            {
                if (WorkingSwipes == null)
                    return TimeSpan.Zero;
                return WorkingSwipes.Aggregate(TimeSpan.Zero, (time, x) => time + x.NotWorkingTime);
            }
        }

        public IList<EmployeeReportSwipeModel> OtSwipes { get; set; }
        public TimeSpan TotalWorkingTimeOT
        {
            get
            {
                if (OtSwipes == null)
                    return TimeSpan.Zero;
                return OtSwipes.Aggregate(TimeSpan.Zero, (time, x) => time + x.WorkingTime);
            }
        }
        public TimeSpan TotalNotWorkingTimeOT
        {
            get
            {
                if (OtSwipes == null)
                    return TimeSpan.Zero;
                return OtSwipes.Aggregate(TimeSpan.Zero, (time, x) => time + x.NotWorkingTime);
            }
        }
    }

    public class EmployeeReportSwipeModel
    {
        public int No { get; set; }
        public DateTime In { get; set; }
        public DateTime Out { get; set; }
        public TimeSpan WorkingTime { get; set; }
        public TimeSpan NotWorkingTime { get; set; }
    }
}
