using DHL.Report.TimeAttendance.Managers.Interfaces;
using DHL.Report.TimeAttendance.Models;
using DHL.Report.TimeAttendance.Services.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows.Input;

namespace DHL.Report.TimeAttendance.ViewModel
{
    public class ShiftViewModel : ViewModelBase
    {
        #region Field
        private readonly IDialogService _dialogService;
        private readonly IShiftManager _shiftManager;
        #endregion

        #region Property
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { Set(() => IsLoading, ref _isLoading, value); }
        }

        private ShiftModel _model;
        public ShiftModel Model
        {
            get { return _model; }
            set { Set(() => Model, ref _model, value); }
        }
        #endregion

        #region Command
        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand<ShiftModel>(async model =>
                {
                    try
                    {
                        IsLoading = true;
                        await _shiftManager.SaveShiftAsync(model);
                        // TODO: close this 
                        // TODO: update main
                    }
                    catch (Exception ex)
                    {
                        _dialogService.ShowMessage(ex.Message, "Error");
                    }
                    finally
                    {
                        IsLoading = false;
                    }
                }, (model) =>
                {
                    // TODO: validate model before allow save
                    return true;
                }));
            }
        }
        #endregion

        #region Constructor
        public ShiftViewModel(IShiftManager shiftManager, IDialogService dialogService)
        {
            _shiftManager = shiftManager;
            _dialogService = dialogService;
        }
        #endregion
    }
}
