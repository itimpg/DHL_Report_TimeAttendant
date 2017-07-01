using DHL.Report.TimeAttendance.Data;
using DHL.Report.TimeAttendance.Data.Entities;
using DHL.Report.TimeAttendance.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DHL.Report.TimeAttendance.Repositories
{
    public class ShiftRepository : IShiftRepository
    {
        #region Field
        private IMyContext _context;
        #endregion

        #region constructor
        public ShiftRepository(IMyContext context)
        {
            _context = context;
        }
        #endregion

        public IEnumerable<Shift> GetShifts()
        {
            return _context.Shifts.ToList();
        }

        public Shift GetShift(int id)
        {
            return _context.Shifts.FirstOrDefault(x => x.Id == id);
        }

        public async Task<int> AddShiftAsync(Shift shift)
        {
            _context.Shifts.Add(shift);
            await _context.SaveChangesAsync();
            return shift.Id;
        }

        public async Task EditShiftAsync(Shift shift)
        {
            var s = _context.Shifts.First(x => x.Id == shift.Id);

            // TODO: implement here 

            await _context.SaveChangesAsync();
        }

        public async Task DeleteShiftAsync(int id)
        {
            var shift = _context.Shifts.FirstOrDefault(x => x.Id == id);
            _context.Shifts.Remove(shift);
            await _context.SaveChangesAsync();
        }
    }
}
