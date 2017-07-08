using DHL.Report.TimeAttendance.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DHL.Report.TimeAttendance.Managers.Interfaces
{
    public interface IAccessDataManager
    {
        Task<IEnumerable<EmployeeSwipeInfoModel>> GetEmployeeSwipeInfo(string filePath, string password, DateTime dateFrom, DateTime dateTo);
    }
}
