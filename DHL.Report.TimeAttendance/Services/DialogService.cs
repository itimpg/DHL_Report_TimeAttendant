using DHL.Report.TimeAttendance.Services.Interfaces;
using System.Windows;
using Forms = System.Windows.Forms;

namespace DHL.Report.TimeAttendance.Services
{
    public class DialogService : IDialogService
    {
        public bool BrowseFolder(out string selectedPath)
        {
            selectedPath = string.Empty;

            var dlg = new Forms.FolderBrowserDialog();
            var result = dlg.ShowDialog();
            if (result == Forms.DialogResult.OK)
            {
                selectedPath = dlg.SelectedPath;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ShowMessage(string message, string caption)
        {
            MessageBox.Show(message, caption);
        }

        public bool ShowConfirmationMessage(string message, string caption)
        {
            var result = MessageBox.Show(message, caption, MessageBoxButton.YesNo);
            return result == MessageBoxResult.Yes;
        }
    }
}
