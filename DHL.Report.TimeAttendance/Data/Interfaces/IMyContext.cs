using DHL.Report.TimeAttendance.Data.Entities;
using System.Data.Entity;
using System.Threading.Tasks;

namespace DHL.Report.TimeAttendance.Data
{
    public interface IMyContext
    {
        DbSet<Shift> Shifts { get; }
        Task<int> SaveChangesAsync();
    }
}
