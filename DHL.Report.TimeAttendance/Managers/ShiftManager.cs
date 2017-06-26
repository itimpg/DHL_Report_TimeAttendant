using DHL.Report.TimeAttendance.Managers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DHL.Report.TimeAttendance.Models;

namespace DHL.Report.TimeAttendance.Managers
{
    public class ShiftManager : IShiftManager
    {
        public Task DeleteShiftAsync(int id)
        {
            return null;
        }

        public Task<ShiftModel> GetShiftAsync(int id)
        {
            return null;
        }

        public async Task<IEnumerable<ShiftModel>> GetShiftsAsync()
        {
            return await Task.Run(() =>
            {
                return new List<ShiftModel>
                {
                    new ShiftModel
                    {
                        Id = 1,
                        Code = "02",
                        WorkFrom = new TimeSpan(8, 30, 0),
                        WorkTo = new TimeSpan(17, 30, 0),
                        MealFrom = new TimeSpan(12, 0, 0),
                        MealTo = new TimeSpan(13, 0, 0),
                        BreakFrom = new TimeSpan(17, 30, 0),
                        BreakTo = new TimeSpan(18, 0, 0)
                    },

                    new ShiftModel
                    {
                        Id = 2,
                        Code = "09",
                        WorkFrom = new TimeSpan(21, 0, 0),
                        WorkTo = new TimeSpan(6, 0, 0),
                        MealFrom = new TimeSpan(1, 0, 0),
                        MealTo = new TimeSpan(2, 0, 0),
                        BreakFrom = new TimeSpan(6, 0, 0),
                        BreakTo = new TimeSpan(6, 30, 0)
                    },

                    new ShiftModel
                    {
                        Id = 3,
                        Code = "10",
                        WorkFrom = new TimeSpan(6, 0, 0),
                        WorkTo = new TimeSpan(15, 0, 0),
                        MealFrom = new TimeSpan(11, 0, 0),
                        MealTo = new TimeSpan(12, 0, 0),
                        BreakFrom = new TimeSpan(15, 0, 0),
                        BreakTo = new TimeSpan(15, 30, 0)
                    },

                    new ShiftModel
                    {
                        Id = 4,
                        Code = "33",
                        WorkFrom = new TimeSpan(14, 0, 0),
                        WorkTo = new TimeSpan(23, 0, 0),
                        MealFrom = new TimeSpan(18, 0, 0),
                        MealTo = new TimeSpan(19, 0, 0),
                        BreakFrom = new TimeSpan(23, 0, 0),
                        BreakTo = new TimeSpan(23, 30, 0)
                    },

                    new ShiftModel
                    {
                        Id = 5,
                        Code = "34",
                        WorkFrom = new TimeSpan(22, 0, 0),
                        WorkTo = new TimeSpan(7, 0, 0),
                        MealFrom = new TimeSpan(2, 0, 0),
                        MealTo = new TimeSpan(3, 0, 0),
                        BreakFrom = new TimeSpan(7, 0, 0),
                        BreakTo = new TimeSpan(7, 30, 0)
                    },

                    new ShiftModel
                    {
                        Id = 6,
                        Code = "35",
                        WorkFrom = new TimeSpan(18, 0, 0),
                        WorkTo = new TimeSpan(3, 0, 0),
                        MealFrom = new TimeSpan(22, 0, 0),
                        MealTo = new TimeSpan(23, 0, 0),
                        BreakFrom = new TimeSpan(3, 0, 0),
                        BreakTo = new TimeSpan(3, 30, 0)
                    },
                };
            });
        }

        public Task<int> SaveShiftAsync(ShiftModel shift)
        {
            return null;
        }
    }
}
