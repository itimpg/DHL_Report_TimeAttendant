using DHL.Report.TimeAttendance.Managers.Interfaces;
using DHL.Report.TimeAttendance.Messages;
using DHL.Report.TimeAttendance.Services.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;

namespace DHL.Report.TimeAttendance.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Field
        private readonly IDialogService _dialogService;
        private readonly IReportManager _reportManager;
        #endregion

        #region Properties
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { Set(() => IsLoading, ref _isLoading, value); }
        }

        private string _accessFilePath;
        public string AccessFilePath
        {
            get { return _accessFilePath; }
            set { Set(() => AccessFilePath, ref _accessFilePath, value); }
        }

        private string _excelFilePath;
        public string ExcelFilePath
        {
            get { return _excelFilePath; }
            set { Set(() => ExcelFilePath, ref _excelFilePath, value); }
        }

        private bool _isOption1;
        public bool IsOption1
        {
            get { return _isOption1; }
            set { Set(() => IsOption1, ref _isOption1, value); }
        }

        private bool _isOption2;
        public bool IsOption2
        {
            get { return _isOption2; }
            set { Set(() => IsOption2, ref _isOption2, value); }
        }

        private bool _isOption3;
        public bool IsOption3
        {
            get { return _isOption3; }
            set { Set(() => IsOption3, ref _isOption3, value); }
        }

        private bool _isOption4;
        public bool IsOption4
        {
            get { return _isOption4; }
            set { Set(() => IsOption4, ref _isOption4, value); }
        }
        #endregion

        #region Commands
        private ICommand _openSettingsCommand;
        public ICommand OpenSettingsCommand
        {
            get
            {
                return _openSettingsCommand ?? (_openSettingsCommand = new RelayCommand(() =>
                {
                    Messenger.Default.Send(new OpenWindowNotificationMessage("Open Settings", WindowType.Settings));
                }));
            }
        }

        private ICommand _openAboutCommand;
        public ICommand OpenAboutCommand
        {
            get
            {
                return _openSettingsCommand ?? (_openAboutCommand = new RelayCommand(() =>
                {
                    Messenger.Default.Send(new OpenWindowNotificationMessage("Open About", WindowType.About));
                }));
            }
        }

        private ICommand _generateReportCommand;
        public ICommand GenerateReportCommand
        {
            get
            {
                return _generateReportCommand ?? (_generateReportCommand = new RelayCommand(async () =>
                {
                    await _reportManager.CreateReport1Async("", "", "");
                }));
            }
        }
        #endregion

        #region Constructor
        public MainViewModel(IReportManager reportManager, IDialogService dialogService)
        {
            _reportManager = reportManager;
            _dialogService = dialogService;
        }
        #endregion  
    }
}