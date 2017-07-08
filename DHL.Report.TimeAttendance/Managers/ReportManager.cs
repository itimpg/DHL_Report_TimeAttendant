using System.Threading.Tasks;
using DHL.Report.TimeAttendance.Managers.Interfaces;
using DHL.Report.TimeAttendance.Models;
using System.Linq;
using System;

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
            //var hrItems = _excelDataManager.GetHrSource(criteria.ExcelFilePath);
            //var shiftItem = await _shiftManager.GetShiftsAsync();

            DateTime searchDate = criteria.SearchDate;
            DateTime searchFrom = new DateTime(searchDate.Year, searchDate.Month, 1);
            DateTime searchTo = searchFrom.AddMonths(1).AddDays(-1);

            var dbItems = (await _accessDataManager.GetEmployeeSwipeInfo(
                criteria.AccessFilePath,
                criteria.AccessPassword,
                searchFrom,
                searchTo
                )).ToList();

            var total = dbItems.Count();
        }
    }
}
