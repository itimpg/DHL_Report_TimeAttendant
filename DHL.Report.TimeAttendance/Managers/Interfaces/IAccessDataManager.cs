using DHL.Report.TimeAttendance.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DHL.Report.TimeAttendance.Managers.Interfaces
{
    public interface IAccessDataManager
    {
        Task<IEnumerable<EmployeeSwipeInfoModel>> ReadData(string filePath, string password);
    }
}
