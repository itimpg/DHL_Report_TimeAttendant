using GalaSoft.MvvmLight.Messaging;

namespace DHL.Report.TimeAttendance.Messages
{
    public class DataChangedNotificationMessage : NotificationMessage
    {
        public DataChangedType DataChanged { get; private set; }

        public DataChangedNotificationMessage(string notification, DataChangedType dataChanged)
                    : base(notification)
        {
            DataChanged = dataChanged;
        }
    }

    public enum DataChangedType
    {
        Shift,
    }
}
