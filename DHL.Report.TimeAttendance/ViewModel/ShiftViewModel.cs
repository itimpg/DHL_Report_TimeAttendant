using DHL.Report.TimeAttendance.Managers.Interfaces;
using DHL.Report.TimeAttendance.Messages;
using DHL.Report.TimeAttendance.Models;
using DHL.Report.TimeAttendance.Models.Session;
using DHL.Report.TimeAttendance.Services.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
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

        private string _title;
        public string Title
        {
            get { return _title; }
            set { Set(() => Title, ref _title, value); }
        }

        private ShiftModel _model;
        public ShiftModel Model
        {
            get { return _model; }
            set { Set(() => Model, ref _model, value); }
        }
        #endregion

        #region Command
        private ICommand _onLoadCommand;
        public ICommand OnLoadCommand
        {
            get
            {
                return _onLoadCommand ?? (_onLoadCommand = new RelayCommand(async () =>
                {
                    try
                    {
                        IsLoading = true;

                        int shiftId = AppSessionModel.Instance().ShiftId;
                        if (shiftId > 0 && (Model = await _shiftManager.GetShiftAsync(shiftId)) != null)
                        {
                            Title = "แก้ไขข้อมูลกะ";
                        }
                        else
                        {
                            Title = "เพิ่มกะใหม่";
                            Model = new ShiftModel();
                        }
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

                        Messenger.Default.Send(new DataChangedNotificationMessage("Camera Changed", DataChangedType.Shift));
                        Messenger.Default.Send(new CloseWindowNotificationMessage("Closed", WindowType.Shift));
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
