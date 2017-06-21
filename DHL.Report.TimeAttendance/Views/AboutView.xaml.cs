using DHL.Report.TimeAttendance.Messages;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;

namespace DHL.Report.TimeAttendance.Views
{
    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    public partial class AboutView : MetroWindow
    {
        public AboutView()
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
