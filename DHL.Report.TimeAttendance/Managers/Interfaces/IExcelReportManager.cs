namespace DHL.Report.TimeAttendance.Managers.Interfaces
{
    public interface IExcelReportManager
    {
        void CreateReport(string dirPath, object reportObjects);
    }
}
