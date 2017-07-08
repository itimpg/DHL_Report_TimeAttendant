using DHL.Report.TimeAttendance.Models;

namespace DHL.Report.TimeAttendance.Managers.Interfaces
{
    public interface IExcelReportManager
    {
        void CreateDailyReport(string dirPath, EmployeeReportResultModel report);
        void CreateMonthlyReport(string dirPath, EmployeeReportResultModel report);
        void CreateAverageReport(string dirPath, EmployeeReportResultModel report);
        void CreateDailySummaryReport(string dirPath, EmployeeReportResultModel report);
    }
}
