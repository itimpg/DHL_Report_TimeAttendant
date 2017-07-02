using DHL.Report.TimeAttendance.Managers.Interfaces;
using DHL.Report.TimeAttendance.Messages;
using DHL.Report.TimeAttendance.Models;
using DHL.Report.TimeAttendance.Services.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using DHL.Report.TimeAttendance.Models.Session;

namespace DHL.Report.TimeAttendance.ViewModel
{
    public class SettingViewModel : ViewModelBase
    {
        #region Field
        private readonly IShiftManager _shiftManager;
        private readonly IDialogService _dialogService;
        private readonly IConfigManager _configManager;
        #endregion

        #region Property
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { Set(() => IsLoading, ref _isLoading, value); }
        }

        public ObservableCollection<ShiftModel> ShiftItems { get; set; }

        #endregion

        #region Command
        private ICommand _onLoadCommand;
        public ICommand OnLoadCommand
        {
            get
            {
                return _onLoadCommand ?? (_onLoadCommand = new RelayCommand(async () =>
               {
                   await LoadShifts();
               }));
            }
        }

        private async Task LoadShifts()
        {
            try
            {
                IsLoading = true;

                var items = await _shiftManager.GetShiftsAsync();
                ShiftItems.Clear();
                foreach (var item in items)
                {
                    ShiftItems.Add(item);
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
        }

        private ICommand _addShiftCommand;
        public ICommand AddShiftCommand
        {
            get
            {
                return _addShiftCommand ?? (_addShiftCommand = new RelayCommand(() =>
                {
                    AppSessionModel.Instance().ShiftId = -1;
                    Messenger.Default.Send(new OpenWindowNotificationMessage("Open Config", WindowType.Shift));
                }));
            }
        }

        private ICommand _editShiftCommand;
        public ICommand EditShiftCommand
        {
            get
            {
                return _editShiftCommand ?? (_editShiftCommand = new RelayCommand<ShiftModel>(shift =>
                {
                    AppSessionModel.Instance().ShiftId = shift.Id;
                    Messenger.Default.Send(new OpenWindowNotificationMessage("Open Config", WindowType.Shift));
                }));
            }
        }

        private ICommand _deleteShiftCommand;
        public ICommand DeleteShiftCommand
        {
            get
            {
                return _deleteShiftCommand ?? (_deleteShiftCommand = new RelayCommand<int>(async id =>
                {
                    try
                    {
                        if (_dialogService.ShowConfirmationMessage("Do you want to delete this shift?", "Comfirm Delete"))
                        {
                            IsLoading = true;
                            await _shiftManager.DeleteShiftAsync(id);
                            var removedShift = ShiftItems.FirstOrDefault(x => x.Id == id);
                            ShiftItems.Remove(removedShift);
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

        #endregion

        #region Constructor
        public SettingViewModel(
            IConfigManager configManager,
            IShiftManager shiftManager,
            IDialogService dialogService)
        {
            _shiftManager = shiftManager;
            _configManager = configManager;
            _dialogService = dialogService;

            ShiftItems = new ObservableCollection<ShiftModel>();

            Messenger.Default.Register<DataChangedNotificationMessage>(this, async (msg) =>
            {
                if (msg.DataChanged == DataChangedType.Shift)
                {
                    await LoadShifts();
                }
            });
        }
        #endregion
    }
}
