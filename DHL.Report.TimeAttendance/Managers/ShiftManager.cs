using DHL.Report.TimeAttendance.Managers.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using DHL.Report.TimeAttendance.Models;
using DHL.Report.TimeAttendance.Repositories.Interfaces;
using AutoMapper;
using DHL.Report.TimeAttendance.Data.Entities;

namespace DHL.Report.TimeAttendance.Managers
{
    public class ShiftManager : IShiftManager
    {
        #region Field
        private readonly IShiftRepository _shiftRepository;
        #endregion

        #region Constructor
        public ShiftManager(IShiftRepository shiftRepository)
        {
            _shiftRepository = shiftRepository;
        }
        #endregion

        public async Task DeleteShiftAsync(int id)
        {
            await _shiftRepository.DeleteShiftAsync(id);
        }

        public async Task<ShiftModel> GetShiftAsync(int id)
        {
            return await Task.Run(() =>
            {
                var shift = _shiftRepository.GetShift(id);
                return Mapper.Map<ShiftModel>(shift);
            });
        }

        public async Task<IEnumerable<ShiftModel>> GetShiftsAsync()
        {
            return await Task.Run(() =>
            {
                var shifts = _shiftRepository.GetShifts();
                return Mapper.Map<IEnumerable<ShiftModel>>(shifts);
            });
        }

        public async Task<int> SaveShiftAsync(ShiftModel shift)
        {
            var s = Mapper.Map<Shift>(shift);
            if (shift.Id == 0)
            {
                return await _shiftRepository.AddShiftAsync(s);
            }
            else
            {
                await _shiftRepository.EditShiftAsync(s);
                return shift.Id;
            }
        }
    }
}
