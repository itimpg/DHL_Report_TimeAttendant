using DHL.Report.TimeAttendance.Messages;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;

namespace DHL.Report.TimeAttendance.Views
{
    public partial class SettingsView : MetroWindow
    {
        public SettingsView()
        {
            InitializeComponent();
            Messenger.Default.Register<CloseWindowNotificationMessage>(this, ReplyToCloseWindowMessage);
        }

        private void ReplyToCloseWindowMessage(CloseWindowNotificationMessage msg)
        {
            if (msg.TargetWindowType == WindowType.About)
            {
                Close();
            }
        }
    }
}
