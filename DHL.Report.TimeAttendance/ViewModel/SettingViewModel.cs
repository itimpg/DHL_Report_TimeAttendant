using DHL.Report.TimeAttendance.Managers.Interfaces;
using DHL.Report.TimeAttendance.Messages;
using DHL.Report.TimeAttendance.Models;
using DHL.Report.TimeAttendance.Services.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DHL.Report.TimeAttendance.ViewModel
{
    public class SettingViewModel : ViewModelBase
    {
        #region Field
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
                return _onLoadCommand ?? (_onLoadCommand = new RelayCommand(() =>
               {
                   try
                   {
                       IsLoading = true;

                       var items = FAKE_GET_SHIFTS();

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
               }));
            }
        }

        private List<ShiftModel> FAKE_GET_SHIFTS()
        {
            return new List<ShiftModel>
            {
                new ShiftModel
                {
                    Code = "02",
                    WorkFrom = new TimeSpan(8, 30, 0),
                    WorkTo = new TimeSpan(17, 30, 0),
                    MealFrom = new TimeSpan(12, 0, 0),
                    MealTo = new TimeSpan(13, 0, 0),
                    BreakFrom = new TimeSpan(17, 30, 0),
                    BreakTo = new TimeSpan(18, 0, 0)
                },

                new ShiftModel
                {
                    Code = "09",
                    WorkFrom = new TimeSpan(21, 0, 0),
                    WorkTo = new TimeSpan(6, 0, 0),
                    MealFrom = new TimeSpan(1, 0, 0),
                    MealTo = new TimeSpan(2, 0, 0),
                    BreakFrom = new TimeSpan(6, 0, 0),
                    BreakTo = new TimeSpan(6, 30, 0)
                },

                new ShiftModel
                {
                    Code = "10",
                    WorkFrom = new TimeSpan(6, 0, 0),
                    WorkTo = new TimeSpan(15, 0, 0),
                    MealFrom = new TimeSpan(11, 0, 0),
                    MealTo = new TimeSpan(12, 0, 0),
                    BreakFrom = new TimeSpan(15, 0, 0),
                    BreakTo = new TimeSpan(15, 30, 0)
                },

                new ShiftModel
                {
                    Code = "33",
                    WorkFrom = new TimeSpan(14, 0, 0),
                    WorkTo = new TimeSpan(23, 0, 0),
                    MealFrom = new TimeSpan(18, 0, 0),
                    MealTo = new TimeSpan(19, 0, 0),
                    BreakFrom = new TimeSpan(23, 0, 0),
                    BreakTo = new TimeSpan(23, 30, 0)
                },

                new ShiftModel
                {
                    Code = "34",
                    WorkFrom = new TimeSpan(22, 0, 0),
                    WorkTo = new TimeSpan(7, 0, 0),
                    MealFrom = new TimeSpan(2, 0, 0),
                    MealTo = new TimeSpan(3, 0, 0),
                    BreakFrom = new TimeSpan(7, 0, 0),
                    BreakTo = new TimeSpan(7, 30, 0)
                },

                new ShiftModel
                {
                    Code = "35",
                    WorkFrom = new TimeSpan(18, 0, 0),
                    WorkTo = new TimeSpan(3, 0, 0),
                    MealFrom = new TimeSpan(22, 0, 0),
                    MealTo = new TimeSpan(23, 0, 0),
                    BreakFrom = new TimeSpan(3, 0, 0),
                    BreakTo = new TimeSpan(3, 30, 0)
                },

            };
        }

        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand<ConfigModel>(async c =>
                {
                    try
                    {
                        IsLoading = true;
                        await _configManager.SaveConfigAsync(c);
                        Messenger.Default.Send(new CloseWindowNotificationMessage("Close Settings", WindowType.Settings));
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
        public SettingViewModel(IConfigManager configManager, IDialogService dialogService)
        {
            _configManager = configManager;
            _dialogService = dialogService;

            ShiftItems = new ObservableCollection<ShiftModel>();
        }
        #endregion
    }
}
