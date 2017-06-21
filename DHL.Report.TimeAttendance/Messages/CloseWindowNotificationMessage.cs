using GalaSoft.MvvmLight.Messaging;

namespace DHL.Report.TimeAttendance.Messages
{
    public class CloseWindowNotificationMessage : NotificationMessage
    {
        public WindowType TargetWindowType { get; private set; }

        public CloseWindowNotificationMessage(string notification, WindowType targetWindowType)
            : base(notification)
        {
            TargetWindowType = targetWindowType;
        }
    }
}
