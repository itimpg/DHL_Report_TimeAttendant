using System;
using System.Threading.Tasks;
using DHL.Report.TimeAttendance.Managers.Interfaces;

namespace DHL.Report.TimeAttendance.Managers
{
    public class ReportManager : IReportManager
    {
        private readonly IConfigManager _configManager;

        public ReportManager(IConfigManager configManager)
        {
            _configManager = configManager;
        }

        public Task CreateReport1Async(string dbFilePath, string excelFilePath, string outputFullName)
        {
            throw new NotImplementedException();
        }

        public Task CreateReport2Async(string dbFilePath, string excelFilePath, string outputFullName)
        {
            throw new NotImplementedException();
        }

        public Task CreateReport3Async(string dbFilePath, string excelFilePath, string outputFullName)
        {
            throw new NotImplementedException();
        }

        public Task CreateReport4Async(string dbFilePath, string excelFilePath, string outputFullName)
        {
            throw new NotImplementedException();
        }
    }
}
