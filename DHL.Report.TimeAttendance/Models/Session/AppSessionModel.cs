namespace DHL.Report.TimeAttendance.Models.Session
{
    public class AppSessionModel
    {
        public int ShiftId { get; set; }

        private AppSessionModel() { }

        private static AppSessionModel _session;

        public static AppSessionModel Instance()
        {
            return _session ?? (_session = new AppSessionModel());
        }
    }
}
