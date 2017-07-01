using DHL.Report.TimeAttendance.Managers.Interfaces;
using DHL.Report.TimeAttendance.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Input;

namespace DHL.Report.TimeAttendance.ViewModel
{
    public class AboutViewModel : ViewModelBase
    {
        #region Field
        private readonly IAboutManager _aboutManager;
        #endregion

        #region Property
        private AboutModel _aboutModel;
        public AboutModel AboutModel
        {
            get { return _aboutModel; }
            set { Set(() => AboutModel, ref _aboutModel, value); }
        }
        #endregion

        #region Command
        private ICommand _showCommand;
        public ICommand ShowCommand
        {
            get
            {
                return _showCommand ?? (_showCommand = new RelayCommand(() =>
                {
                    AboutModel = _aboutManager.GetAbout();
                }));
            }
        }
        #endregion

        #region Constructor
        public AboutViewModel(IAboutManager aboutManager)
        {
            _aboutManager = aboutManager;
        }
        #endregion
    }
}
