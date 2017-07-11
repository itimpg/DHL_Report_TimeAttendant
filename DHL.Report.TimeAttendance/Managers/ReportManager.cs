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
            DateTime searchDateFrom = new DateTime(searchDate.Year, searchDate.Month, 1);
            DateTime searchDateTo = searchDateFrom.AddMonths(1).AddDays(-1);

            var accessData = (await _accessDataManager.GetEmployeeSwipeInfo(
                criteria.AccessFilePath, criteria.AccessPassword, searchDateFrom, searchDateTo));
            var excelData = _excelDataManager.GetHrSource(criteria.ExcelFilePath);
            var sqLiteData = await _shiftManager.GetShiftsAsync();

            var src = (from a in accessData
                       join b in excelData on
                           new { a.ReportDate, a.EmployeeId } equals new { ReportDate = b.DataDate, b.EmployeeId } into lb
                       from e in lb.DefaultIfEmpty()
                       join c in sqLiteData on e?.ShiftCode ?? string.Empty equals c.Code into lc
                       from s in lc.DefaultIfEmpty()
                       let dateIn = a.DateIn ?? a.ReportDate
                       let dateOut = a.DateOut ?? a.ReportDate
                       let workFrom = a.ReportDate.Add(s?.WorkFrom ?? TimeSpan.Zero)
                       let workTo = a.ReportDate.Add(s?.WorkTo ?? TimeSpan.Zero)
                       select new ReportSourceModel
                       {
                           ReportDate = a.ReportDate,
                           EmployeeId = a.EmployeeId,
                           DateIn = a.DateIn,
                           DateOut = a.DateOut,
                           WorkingTime = a.WorkingTime ?? TimeSpan.Zero,
                           NotWorkingTime = a.NotWorkingTime ?? TimeSpan.Zero,
                           IsEarlyOT = a.DateIn.HasValue && a.DateOut.HasValue &&
                            (dateIn < workFrom && dateOut >= workFrom && (workFrom - dateIn).TotalHours >= 2),
                           IsLateOT = a.DateIn.HasValue && a.DateOut.HasValue &&
                            (dateIn <= workTo && dateOut > workTo && (dateOut - workTo).TotalHours >= 2),
                           Company = e?.Company ?? " - ",
                           Name = e?.Name ?? " - ",
                           Department = e?.Department ?? " - ",
                           ShiftCode = (e?.ShiftCode ?? " - "),
                           ShiftName = s?.Name ?? " - ",
                           WorkFrom = workFrom,
                           WorkTo = workTo,
                       })
                       .OrderBy(x => x.EmployeeId).ThenBy(x => x.DateIn)
                       .ToList();

            var addList = new List<ReportSourceModel>();
            for (int i = 0; i < src.Count(); i++)
            {
                var item = src[i];
                if (item.IsEarlyOT)
                {
                    var newItem = CopyFrom(item);
                    newItem.DateIn = item.WorkFrom;
                    newItem.NotWorkingTime = TimeSpan.Zero;
                    newItem.WorkingTime = (newItem.DateOut - newItem.DateIn) ?? TimeSpan.Zero;
                    newItem.IsEarlyOT = false;
                    newItem.IsLateOT = false;
                    addList.Add(newItem);

                    item.DateOut = item.WorkFrom;
                    item.WorkingTime = (item.DateOut - item.DateIn) ?? TimeSpan.Zero;
                }

                if (item.IsLateOT)
                {
                    var newItem = CopyFrom(item);
                    newItem.DateOut = item.WorkTo;
                    newItem.NotWorkingTime = TimeSpan.Zero;
                    newItem.WorkingTime = (newItem.DateOut - newItem.DateIn) ?? TimeSpan.Zero;
                    newItem.IsEarlyOT = false;
                    newItem.IsLateOT = false;
                    addList.Add(newItem);

                    item.DateIn = item.WorkTo;
                    item.WorkingTime = (item.DateOut - item.DateIn) ?? TimeSpan.Zero;
                }
            }

            var query = src.Union(addList);

            var reportData = query.GroupBy(x => x.Company)
                .Select(g => new EmployeeReportCompanyModel
                {
                    Company = g.Key,
                    Employees = g.GroupBy(x => new
                    {
                        ReportDate = x.ReportDate,
                        EmployeeId = x.EmployeeId,
                        Name = x.Name,
                        Department = x.Department,
                        ShiftCode = x.ShiftCode,
                        ShiftName = x.ShiftName,
                    })
                    .Select(ge => new EmployeeReportModel
                    {
                        ReportDate = ge.Key.ReportDate,
                        EmployeeId = ge.Key.EmployeeId,
                        Name = ge.Key.Name,
                        Department = ge.Key.Department,
                        ShiftCode = ge.Key.ShiftCode,
                        ShiftName = ge.Key.ShiftName,
                        WorkingSwipes = ge.Where(x => !(x.IsEarlyOT || x.IsLateOT)).Select((x, i) => new EmployeeReportSwipeModel
                        {
                            No = i + 1,
                            In = x.DateIn ?? x.ReportDate,
                            Out = x.DateOut,
                            WorkingTime = x.WorkingTime,
                            NotWorkingTime = x.NotWorkingTime,
                        }).OrderBy(x => x.In).ToList(),
                        OtSwipes = ge.Where(x => x.IsEarlyOT || x.IsLateOT).Select((x, i) => new EmployeeReportSwipeModel
                        {
                            No = i + 1,
                            In = x.DateIn ?? x.ReportDate,
                            Out = x.DateOut,
                            WorkingTime = x.WorkingTime,
                            NotWorkingTime = x.NotWorkingTime,
                        }).OrderBy(x => x.In).ToList()
                    })
                    .OrderBy(x => x.ReportDate)
                    .ThenBy(x => x.EmployeeId)
                    .ToList()
                }).ToList();

            var result = new EmployeeReportResultModel
            {
                ReportMonth = searchDateFrom,
                Companies = reportData
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

        private ReportSourceModel CopyFrom(ReportSourceModel item)
        {
            return new ReportSourceModel
            {
                ReportDate = item.ReportDate,
                EmployeeId = item.EmployeeId,
                DateIn = item.DateIn,
                DateOut = item.DateOut,
                WorkingTime = item.WorkingTime,
                NotWorkingTime = item.NotWorkingTime,
                IsEarlyOT = item.IsEarlyOT,
                IsLateOT = item.IsLateOT,
                Company = item.Company,
                Name = item.Name,
                Department = item.Department,
                ShiftCode = item.ShiftCode,
                ShiftName = item.ShiftName,
                WorkFrom = item.WorkFrom,
                WorkTo = item.WorkTo,
            };
        }
    }
}
