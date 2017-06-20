using DHL.Report.TimeAttendance.Managers.Interfaces;
using DHL.Report.TimeAttendance.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace DHL.Report.TimeAttendance.ViewModel
{
    public class SettingViewModel : ViewModelBase
    {
        #region Field
        private readonly IConfigManager _configManager;
        #endregion

        #region Property
        private ConfigModel _config;
        public ConfigModel Config
        {
            get { return _config; }
            set { Set(() => Config, ref _config, value); }
        }
        #endregion

        #region Command
        private ICommand _showCommand;
        public ICommand ShowCommand
        {
            get
            {
                return _showCommand ?? (_showCommand = new RelayCommand(async () =>
                {
                    Config = await _configManager.GetConfigAsync();
                }));
            }
        }

        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand<ConfigModel>(async c =>
                {
                    await _configManager.SaveConfigAsync(c);
                }));
            }
        }
        #endregion

        #region Constructor
        public SettingViewModel(IConfigManager configManager)
        {
            _configManager = configManager;
        }
        #endregion
    }
}
