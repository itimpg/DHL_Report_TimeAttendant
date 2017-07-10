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

        private EmployeeReportResultModel GetMockData()
        {
            return new EmployeeReportResultModel()
            {
                ReportMonth = new DateTime(2017, 6, 1),
                Companies = new List<EmployeeReportCompanyModel>
                {
                    new EmployeeReportCompanyModel
                    {
                        Company = "DHL",
                        Employees = new List<EmployeeReportModel>
                        {
                            new EmployeeReportModel
                            {
                                ReportDate = DateTime.Today.AddDays(1),
                                Name = "Test1",
                                Department = "Dept1",
                                EmployeeId = "01",
                                ShiftCode = "02",
                                ShiftName = "Early",
                                WorkingSwipes = new List<EmployeeReportSwipeModel>
                                {
                                    new EmployeeReportSwipeModel
                                    {
                                        No = 1,
                                        In = DateTime.Now,
                                        Out = DateTime.Now.AddHours(1),
                                        WorkingTime = new TimeSpan(1,0,0),
                                    },
                                    new EmployeeReportSwipeModel
                                    {
                                        No = 1,
                                        In = DateTime.Now.AddHours(2),
                                        Out = DateTime.Now.AddHours(5),
                                        WorkingTime = new TimeSpan(3,0,0),
                                        NotWorkingTime = new TimeSpan(1,0,0)
                                    },
                                },
                                OtSwipes = new List<EmployeeReportSwipeModel>
                                {
                                    new EmployeeReportSwipeModel
                                    {
                                        No = 1,
                                        In = DateTime.Now.AddHours(3),
                                        Out = DateTime.Now.AddHours(9),
                                    },
                                },
                            },
                            new EmployeeReportModel
                            { ReportDate = DateTime.Today,
                                Name = "Test2",
                                Department = "Dept1",
                                EmployeeId = "02",
                                ShiftCode = "02",
                                ShiftName = "Early",
                                WorkingSwipes = new List<EmployeeReportSwipeModel>
                                {
                                    new EmployeeReportSwipeModel
                                    {
                                        No = 1,
                                        In = DateTime.Now.AddHours(2),
                                        Out = DateTime.Now.AddHours(4),
                                        WorkingTime = new TimeSpan(2,0,0),
                                    },
                                    new EmployeeReportSwipeModel
                                    {
                                        No = 1,
                                        In = DateTime.Now.AddHours(6),
                                        Out = DateTime.Now.AddHours(10),
                                        WorkingTime = new TimeSpan(4,0,0),
                                        NotWorkingTime = new TimeSpan(2,0,0)
                                    },
                                },
                                OtSwipes = new List<EmployeeReportSwipeModel>
                                {
                                    new EmployeeReportSwipeModel
                                    {
                                        No = 1,
                                        In = DateTime.Now.AddHours(3),
                                        Out = DateTime.Now.AddHours(9),
                                        WorkingTime = new TimeSpan(3,0,0),
                                    },
                                },
                            }
                        }
                    }
                }
            };
        }

        public async Task CreateReportAsync(ReportCriteriaModel criteria)
        {
            DateTime searchDate = criteria.SearchDate;
            DateTime searchFrom = new DateTime(searchDate.Year, searchDate.Month, 1);
            DateTime searchTo = searchFrom.AddMonths(1).AddDays(-1);

            var accessData = (await _accessDataManager.GetEmployeeSwipeInfo(criteria.AccessFilePath, criteria.AccessPassword, searchFrom, searchTo)).ToList();
            //var excelData = _excelDataManager.GetHrSource(criteria.ExcelFilePath);
            //var sqLiteData = await _shiftManager.GetShiftsAsync();
            
            var result = GetMockData();

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
