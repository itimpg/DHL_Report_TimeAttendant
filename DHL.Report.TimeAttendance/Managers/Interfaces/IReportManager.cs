using System.Threading.Tasks;

namespace DHL.Report.TimeAttendance.Managers.Interfaces
{
    public interface IReportManager
    {
        Task CreateReport1Async(string dbFilePath, string excelFilePath, string outputFullName);
        Task CreateReport2Async(string dbFilePath, string excelFilePath, string outputFullName);
        Task CreateReport3Async(string dbFilePath, string excelFilePath, string outputFullName);
        Task CreateReport4Async(string dbFilePath, string excelFilePath, string outputFullName);
    }
}