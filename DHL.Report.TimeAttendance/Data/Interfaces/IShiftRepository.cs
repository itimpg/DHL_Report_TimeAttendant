using DHL.Report.TimeAttendance.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DHL.Report.TimeAttendance.Repositories.Interfaces
{
    public interface IShiftRepository
    {
        IEnumerable<Shift> GetShifts();
        Shift GetShift(int id);
        Task<int> AddShiftAsync(Shift shift);
        Task EditShiftAsync(Shift shift);
        Task DeleteShiftAsync(int id);
    }
}
