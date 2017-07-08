using System;
using DHL.Report.TimeAttendance.Managers.Interfaces;
using System.IO;
using DHL.Report.TimeAttendance.Models;
using OfficeOpenXml;

namespace DHL.Report.TimeAttendance.Managers
{
    public class ExcelReportManager : IExcelReportManager
    {
        public void ValidateParameters(string dirPath, EmployeeReportResultModel report)
        {
            if (string.IsNullOrEmpty(dirPath))
            {
                throw new ArgumentNullException("Output directory path.");
            }

            if (!Directory.Exists(dirPath))
            {
                throw new DirectoryNotFoundException(dirPath + " dose not exists.");
            }

            if (report == null)
            {
                throw new ArgumentNullException("report");
            }
        }

        public void CreateDailyReport(string dirPath, EmployeeReportResultModel report)
        {
            ValidateParameters(dirPath, report);

            string directoryName = $"รายงานประจำวัน_{report.ReportYear}_{report.ReportMonth}";
            string dailyReportDir = Path.Combine(dirPath, directoryName);
            if (!Directory.Exists(dailyReportDir))
            {
                Directory.CreateDirectory(dailyReportDir);
            }

            foreach (var employee in report.Employees)
            {
                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage())
                {
                    // add a new worksheet to the empty workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sales list - " + DateTime.Now.ToShortDateString());

                    // --------- Data and styling goes here -------------- //


                    string fileName = $"{employee.Info.EmployeeId}.xlsx";
                    string filePath = Path.Combine(dailyReportDir, fileName);
                    package.SaveAs(new FileInfo(filePath));
                }
            }
        }

        public void CreateMonthlyReport(string dirPath, EmployeeReportResultModel report)
        {
            ValidateParameters(dirPath, report);
        }

        public void CreateAverageReport(string dirPath, EmployeeReportResultModel report)
        {
            ValidateParameters(dirPath, report);
        }

        public void CreateDailySummaryReport(string dirPath, EmployeeReportResultModel report)
        {
            ValidateParameters(dirPath, report);
        }
    }
}
