using DHL.Report.TimeAttendance.Messages;
using DHL.Report.TimeAttendance.Views;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using System.Windows;

namespace DHL.Report.TimeAttendance
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Messenger.Default.Register<OpenWindowNotificationMessage>(this, ReplyToOpenWindowMessage);
        }

        private void ReplyToOpenWindowMessage(OpenWindowNotificationMessage msg)
        {
            Window window = null;
            switch (msg.TargetWindowType)
            {
                case WindowType.Settings:
                    window = new SettingsView();
                    break;
                case WindowType.About:
                    window = new AboutView();
                    break;
            }
            if (window != null)
            {
                window.Owner = this;
                window.ShowDialog();
            }
        }
    }
}