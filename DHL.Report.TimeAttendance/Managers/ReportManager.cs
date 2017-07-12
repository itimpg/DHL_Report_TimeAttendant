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
                       group new { a, e, s } by new
                       {
                           ReportDate = a.ReportDate,
                           EmployeeId = a.EmployeeId,
                           Company = e?.Company ?? " - ",
                           Name = e?.Name ?? " - ",
                           Department = e?.Department ?? " - ",
                           ShiftCode = (e?.ShiftCode ?? " - "),
                           ShiftName = s?.Name ?? " - ",
                           WorkFrom = a.ReportDate.Add(s?.WorkFrom ?? TimeSpan.Zero),
                           WorkTo = a.ReportDate.Add(s?.WorkTo ?? TimeSpan.Zero),
                       } into g
                       select new
                       {
                           Key = g.Key,
                           Items = from x in g
                                   select new
                                   {
                                       DateIn = x.a.DateIn ?? x.a.ReportDate,
                                       DateOut = x.a.DateOut ?? x.a.ReportDate,
                                       WorkingTime = x.a.WorkingTime ?? TimeSpan.Zero,
                                       NotWorkingTime = x.a.NotWorkingTime ?? TimeSpan.Zero,
                                   },
                       });

            var src2 = from s in src
                       select new
                       {
                           s.Key,
                           HasEarlyWork = s.Items
                                .Where(x => x.DateIn < s.Key.WorkFrom)
                                .Select(x => (s.Key.WorkFrom < x.DateOut ? s.Key.WorkFrom : x.DateOut) - x.DateIn)
                                .Aggregate(TimeSpan.Zero, (time, x) => time + x).TotalHours > 2,
                           HasLatelyWork = s.Items
                                .Where(x => x.DateOut > s.Key.WorkTo)
                                .Select(x => x.DateOut - (s.Key.WorkTo < x.DateIn ? x.DateIn : s.Key.WorkTo))
                                .Aggregate(TimeSpan.Zero, (time, x) => time + x).TotalHours > 2,
                           Items = s.Items,
                       };

            var itemsSource = new List<ReportSourceModel>();
            foreach (var s in src2)
            {
                foreach (var i in s.Items.OrderBy(x => x.DateIn))
                {
                    var item = new ReportSourceModel
                    {
                        Company = s.Key.Company,
                        EmployeeId = s.Key.EmployeeId,
                        Department = s.Key.Department,
                        Name = s.Key.Name,
                        ShiftCode = s.Key.ShiftCode,
                        ShiftName = s.Key.ShiftName,
                        ReportDate = s.Key.ReportDate,
                        DateIn = i.DateIn,
                        DateOut = i.DateOut,
                        WorkingTime = i.WorkingTime,
                        NotWorkingTime = i.NotWorkingTime,
                    };

                    if (s.HasEarlyWork && i.DateIn < s.Key.WorkFrom)
                    {
                        item.IsEarlyOT = true;
                        if (i.DateOut > s.Key.WorkFrom)
                        {
                            item.DateOut = s.Key.WorkFrom;
                            item.WorkingTime = (item.DateOut - item.DateIn) ?? TimeSpan.Zero;
                            itemsSource.Add(new ReportSourceModel
                            {
                                Company = s.Key.Company,
                                EmployeeId = s.Key.EmployeeId,
                                Department = s.Key.Department,
                                Name = s.Key.Name,
                                ShiftCode = s.Key.ShiftCode,
                                ShiftName = s.Key.ShiftName,
                                ReportDate = s.Key.ReportDate,
                                DateIn = s.Key.WorkFrom,
                                DateOut = i.DateOut,
                                WorkingTime = i.DateOut - s.Key.WorkFrom,
                                NotWorkingTime = i.NotWorkingTime,
                            });
                        }
                    }

                    if (s.HasLatelyWork && i.DateOut > s.Key.WorkTo)
                    {
                        item.IsLateOT = true;
                        if (i.DateIn < s.Key.WorkTo)
                        {
                            item.DateIn = s.Key.WorkTo;
                            item.WorkingTime = (item.DateOut - item.DateIn) ?? TimeSpan.Zero;
                            itemsSource.Add(new ReportSourceModel
                            {
                                Company = s.Key.Company,
                                EmployeeId = s.Key.EmployeeId,
                                Department = s.Key.Department,
                                Name = s.Key.Name,
                                ShiftCode = s.Key.ShiftCode,
                                ShiftName = s.Key.ShiftName,
                                ReportDate = s.Key.ReportDate,
                                DateIn = i.DateIn,
                                DateOut = s.Key.WorkTo,
                                WorkingTime = s.Key.WorkTo - i.DateIn,
                                NotWorkingTime = i.NotWorkingTime,
                            });
                        }
                    }

                    itemsSource.Add(item);
                }
            }

            var reportData = itemsSource.GroupBy(x => x.Company)
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
    }
}
