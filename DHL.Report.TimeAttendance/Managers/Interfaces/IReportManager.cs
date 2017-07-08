using DHL.Report.TimeAttendance.Models;
using System.Threading.Tasks;

namespace DHL.Report.TimeAttendance.Managers.Interfaces
{
    public interface IReportManager
    {
        Task CreateReportAsync(ReportCriteriaModel criteria);
    }
}