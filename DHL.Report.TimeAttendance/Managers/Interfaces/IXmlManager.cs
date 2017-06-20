namespace DHL.Report.TimeAttendance.Managers.Interfaces
{
    public interface IXmlManager
    {
        string Serialize<T>(T dataToSerialize);
        T Deserialize<T>(string xmlText);
    }
}
