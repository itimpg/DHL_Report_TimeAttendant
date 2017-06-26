using DHL.Report.TimeAttendance.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DHL.Report.TimeAttendance.Managers.Interfaces
{
    public interface IShiftManager
    {
        Task<IEnumerable<ShiftModel>> GetShiftsAsync();
        Task<ShiftModel> GetShiftAsync(int id);
        Task<int> SaveShiftAsync(ShiftModel shift);
        Task DeleteShiftAsync(int id);
    }
}
