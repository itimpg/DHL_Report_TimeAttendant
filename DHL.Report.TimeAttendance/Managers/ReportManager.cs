using System;
using System.Threading.Tasks;
using DHL.Report.TimeAttendance.Managers.Interfaces;
using DHL.Report.TimeAttendance.Models;

namespace DHL.Report.TimeAttendance.Managers
{
    public class ReportManager : IReportManager
    {
        public ReportManager()
        {
        }

        public Task CreateReport1Async(ConfigModel config, ReportCriteriaModel criteria)
        {
            throw new NotImplementedException();
        }

        public Task CreateReport2Async(ConfigModel config, ReportCriteriaModel criteria)
        {
            throw new NotImplementedException();
        }

        public Task CreateReport3Async(ConfigModel config, ReportCriteriaModel criteria)
        {
            throw new NotImplementedException();
        }

        public Task CreateReport4Async(ConfigModel config, ReportCriteriaModel criteria)
        {
            throw new NotImplementedException();
        }
    }
}
