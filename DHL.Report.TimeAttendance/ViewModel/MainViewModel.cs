using DHL.Report.TimeAttendance.Managers.Interfaces;
using DHL.Report.TimeAttendance.Messages;
using DHL.Report.TimeAttendance.Models;
using DHL.Report.TimeAttendance.Services.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Linq;
using System.Threading.Tasks;
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

        private ReportCriteriaModel _reportCriteria;
        public ReportCriteriaModel ReportCriteria
        {
            get { return _reportCriteria; }
            set { Set(() => ReportCriteria, ref _reportCriteria, value); }
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
                return _openAboutCommand ?? (_openAboutCommand = new RelayCommand(() =>
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
                return _generateReportCommand ?? (_generateReportCommand = new RelayCommand<ReportCriteriaModel>(async (criteria) =>
                {
                    IsLoading = true;
                    try
                    {
                        await Task.Run(async () =>
                        {
                            await _reportManager.CreateReportAsync(criteria);
                            _dialogService.ShowMessage("Success", "Finish");
                        });
                    }
                    catch (Exception ex)
                    {
                        _dialogService.ShowMessage(ex.Message, "Error");
                    }
                    finally
                    {
                        IsLoading = false;
                    }
                }, (criteria) =>
                {
                    return true || (criteria != null &&
                        !string.IsNullOrEmpty(criteria.AccessFilePath) &&
                        !string.IsNullOrEmpty(criteria.ExcelFilePath) &&
                        !string.IsNullOrEmpty(criteria.OutputDir) &&
                        (criteria.IsOption1 || criteria.IsOption2 || criteria.IsOption3 || criteria.IsOption4));
                }));
            }
        }

        private ICommand _selectDbFileCommand;
        public ICommand SelectDbFileCommand
        {
            get
            {
                return _selectDbFileCommand ?? (_selectDbFileCommand = new RelayCommand(() =>
                {
                    string selectedFilePath;
                    if (_dialogService.BrowseFile(out selectedFilePath, "Access Files|*.mdb;*.accdb"))
                    {
                        ReportCriteria.AccessFilePath = selectedFilePath;
                    }
                }));
            }
        }

        private ICommand _selectExcelCommand;
        public ICommand SelectExcelCommand
        {
            get
            {
                return _selectExcelCommand ?? (_selectExcelCommand = new RelayCommand(() =>
                {
                    string selectedFilePath;
                    if (_dialogService.BrowseFile(out selectedFilePath, "Excel Files|*.xls;*.xlsx"))
                    {
                        ReportCriteria.ExcelFilePath = selectedFilePath;
                    }
                }));
            }
        }

        private ICommand _setOutputDirCommand;
        public ICommand SetOutputDirCommand
        {
            get
            {
                return _setOutputDirCommand ?? (_setOutputDirCommand = new RelayCommand(() =>
                {
                    string selectedPath;
                    if (_dialogService.BrowseFolder(out selectedPath))
                    {
                        ReportCriteria.OutputDir = selectedPath;
                    }
                }));
            }
        }
        #endregion

        #region Constructor
        public MainViewModel(
            IReportManager reportManager,
            IDialogService dialogService)
        {
            _reportManager = reportManager;
            _dialogService = dialogService;
            ReportCriteria = new ReportCriteriaModel { SearchDate = DateTime.Today };

#if DEBUG
            ReportCriteria.AccessFilePath = @"‪C:\Users\itim\Desktop\DHL\iCCard3000.mdb";
            ReportCriteria.AccessPassword = "168168";
            ReportCriteria.ExcelFilePath = @"C:\Users\itim\Desktop\DHL\attendance_tops.xlsx";
#endif
        }
        #endregion  
    }
}