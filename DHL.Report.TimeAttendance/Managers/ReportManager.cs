using System.Threading.Tasks;
using DHL.Report.TimeAttendance.Managers.Interfaces;
using DHL.Report.TimeAttendance.Models;

namespace DHL.Report.TimeAttendance.Managers
{
    public class ReportManager : IReportManager
    {
        #region Fields
        private readonly IExcelDataManager _excelDataManager;
        private readonly IAccessDataManager _accessDataManager;
        #endregion

        #region Constructor
        public ReportManager(IExcelDataManager excelDataManager, IAccessDataManager accessDataManager)
        {
            _excelDataManager = excelDataManager;
            _accessDataManager = accessDataManager;
        }
        #endregion

        public async Task CreateReportAsync(ReportCriteriaModel criteria)
        {
            await Task.Run(() =>
            {
                var hrItems = _excelDataManager.GetHrSource(criteria.ExcelFilePath);
                var dbItems = _accessDataManager.ReadData(criteria.AccessFilePath, criteria.AccessPassword);

                string a = "";


            });
        }
    }
}
