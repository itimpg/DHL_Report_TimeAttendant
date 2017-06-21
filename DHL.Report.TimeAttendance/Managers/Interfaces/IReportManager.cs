using DHL.Report.TimeAttendance.Models;
using System.Threading.Tasks;

namespace DHL.Report.TimeAttendance.Managers.Interfaces
{
    public interface IReportManager
    {
        Task CreateReport1Async(ConfigModel config, ReportCriteriaModel criteria);
        Task CreateReport2Async(ConfigModel config, ReportCriteriaModel criteria);
        Task CreateReport3Async(ConfigModel config, ReportCriteriaModel criteria);
        Task CreateReport4Async(ConfigModel config, ReportCriteriaModel criteria);
    }
}