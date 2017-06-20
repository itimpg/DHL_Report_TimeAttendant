using DHL.Report.TimeAttendance.Models;
using System.Threading.Tasks;

namespace DHL.Report.TimeAttendance.Managers.Interfaces
{
    public interface IAboutManager
    {
        Task<AboutModel> GetAboutAsyc();
    }
}
