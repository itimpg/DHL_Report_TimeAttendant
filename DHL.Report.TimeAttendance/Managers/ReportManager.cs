using System.Threading.Tasks;
using DHL.Report.TimeAttendance.Managers.Interfaces;
using DHL.Report.TimeAttendance.Models;
using System.Linq;
using System;
using System.Collections.Generic;

namespace DHL.Report.TimeAttendance.Managers
{
    public class ReportManager : IReportManager
    {
        #region Fields
        private readonly IExcelDataManager _excelDataManager;
        private readonly IAccessDataManager _accessDataManager;
        private readonly IShiftManager _shiftManager;
        private readonly IExcelReportManager _excelReportManager;
        #endregion

        #region Constructor
        public ReportManager(
            IExcelDataManager excelDataManager,
            IAccessDataManager accessDataManager,
            IShiftManager shiftManager,
            IExcelReportManager excelReportManager)
        {
            _excelDataManager = excelDataManager;
            _accessDataManager = accessDataManager;
            _shiftManager = shiftManager;
            _excelReportManager = excelReportManager;
        }
        #endregion

        public async Task CreateReportAsync(ReportCriteriaModel criteria)
        {
            DateTime searchDate = criteria.SearchDate;
            DateTime searchFrom = new DateTime(searchDate.Year, searchDate.Month, 1);
            DateTime searchTo = searchFrom.AddMonths(1).AddDays(-1);

            // var hrItems = _excelDataManager.GetHrSource(criteria.ExcelFilePath);
            // var shiftItems = await _shiftManager.GetShiftsAsync();
            // var swipeItems = (await _accessDataManager.GetEmployeeSwipeInfo(criteria.AccessFilePath, criteria.AccessPassword, searchFrom, searchTo));

            var result = new EmployeeReportResultModel()
            {
                ReportYear = searchDate.Year,
                ReportMonth = searchDate.Month,
                Employees = new List<EmployeeReportModel>
                {
                    new EmployeeReportModel
                    {
                        Info = new EmployeeInfoModel { Name = "Test1", Department = "Dept1", EmployeeId = "01", Company = "DHL" },
                        ShiftCode = "02",
                        ShiftName = "Early",
                        SwipeCode = "0001",
                        Items = new List<EmployeeReportItemModel>
                        {
                            new EmployeeReportItemModel { In = DateTime.Today, Out = DateTime.Today.AddMinutes(19) },
                            new EmployeeReportItemModel { In = DateTime.Today.AddHours(1), Out = DateTime.Today.AddMinutes(29) }
                        }
                    },
                    new EmployeeReportModel
                    {
                        Info = new EmployeeInfoModel { Name = "Test2", Department = "Dept2", EmployeeId = "02", Company = "DHL" },
                        ShiftCode = "09",
                        ShiftName = "EarlyOT",
                        SwipeCode = "0002",
                        Items = new List<EmployeeReportItemModel>
                        {
                            new EmployeeReportItemModel { In = DateTime.Today, Out = DateTime.Today.AddMinutes(19) },
                            new EmployeeReportItemModel { In = DateTime.Today.AddHours(1), Out = DateTime.Today.AddMinutes(29) }
                        }
                    },
                },
            };

            if (criteria.IsOption1)
            {
                _excelReportManager.CreateMonthlyReport(criteria.OutputDir, result);
            }

            if (criteria.IsOption2)
            {
                _excelReportManager.CreateDailyReport(criteria.OutputDir, result);
            }

            if (criteria.IsOption3)
            {
                _excelReportManager.CreateAverageReport(criteria.OutputDir, result);
            }

            if (criteria.IsOption4)
            {
                _excelReportManager.CreateDailySummaryReport(criteria.OutputDir, result);
            }
        }
    }
}
