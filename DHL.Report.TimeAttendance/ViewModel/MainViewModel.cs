using DHL.Report.TimeAttendance.Managers.Interfaces;
using DHL.Report.TimeAttendance.Messages;
using DHL.Report.TimeAttendance.Models;
using DHL.Report.TimeAttendance.Services.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
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
                return _generateReportCommand ?? (_generateReportCommand = new RelayCommand(async () =>
                {
                    try
                    {
                        IsLoading = true;
                        await _reportManager.CreateReport1Async(null, null);
                        _dialogService.ShowMessage("Success", "Finish");
                    }
                    catch (Exception ex)
                    {
                        _dialogService.ShowMessage(ex.Message, "Error");
                    }
                    finally
                    {
                        IsLoading = false;
                    }
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