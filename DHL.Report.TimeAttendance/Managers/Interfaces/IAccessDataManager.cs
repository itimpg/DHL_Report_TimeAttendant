using System.Data;

namespace DHL.Report.TimeAttendance.Managers.Interfaces
{
    public interface IAccessDataManager
    {
        DataTable ReadData(string filePath, string password);
    }
}
