namespace DHL.Report.TimeAttendance.Services.Interfaces
{
    public interface IDialogService
    {
        bool BrowseFolder(out string selectedPath);
        bool BrowseFile(out string selectedFilePath, string extensionFilter);
        bool SaveFile(out string savedFilePath, string extensionFilter);
        void ShowMessage(string message, string caption);
        bool ShowConfirmationMessage(string message, string caption);
    }
}
