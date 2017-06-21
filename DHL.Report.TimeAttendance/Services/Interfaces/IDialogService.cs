namespace DHL.Report.TimeAttendance.Services.Interfaces
{
    public interface IDialogService
    {
        bool BrowseFolder(out string selectedPath);
        void ShowMessage(string message, string caption);
        bool ShowConfirmationMessage(string message, string caption);
    }
}
