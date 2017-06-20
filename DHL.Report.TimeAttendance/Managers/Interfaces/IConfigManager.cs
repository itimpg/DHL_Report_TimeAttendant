using DHL.Report.TimeAttendance.Models;
using System.Threading.Tasks;

namespace DHL.Report.TimeAttendance.Managers.Interfaces
{
    public interface IConfigManager
    {
        Task<ConfigModel> GetConfigAsync();
        Task SaveConfigAsync(ConfigModel model);
    }
}
