using DHL.Report.TimeAttendance.Models;
using System.Collections.Generic;

namespace DHL.Report.TimeAttendance.Managers.Interfaces
{
    public interface IExcelDataManager
    {
        IEnumerable<EmployeeInfoModel> GetHrSource(string filePath);
    }
}
