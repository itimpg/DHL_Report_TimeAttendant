using DHL.Report.TimeAttendance.Messages;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
