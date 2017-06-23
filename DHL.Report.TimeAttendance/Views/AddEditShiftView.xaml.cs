using DHL.Report.TimeAttendance.Messages;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;

namespace DHL.Report.TimeAttendance.Views
{
    /// <summary>
    /// Interaction logic for AddEditShiftView.xaml
    /// </summary>
    public partial class AddEditShiftView : MetroWindow
    {
        public AddEditShiftView()
        {
            InitializeComponent();
            Messenger.Default.Register<CloseWindowNotificationMessage>(this, ReplyToCloseWindowMessage);
        }

        private void ReplyToCloseWindowMessage(CloseWindowNotificationMessage msg)
        {
            if (msg.TargetWindowType == WindowType.Shift)
            {
                Close();
            }
        }
    }
}
