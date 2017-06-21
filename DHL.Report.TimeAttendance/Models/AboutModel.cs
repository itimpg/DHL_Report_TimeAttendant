using GalaSoft.MvvmLight;
using System;

namespace DHL.Report.TimeAttendance.Models
{
    public class AboutModel : ObservableObject
    {
        private string _version;
        public string Version
        {
            get { return _version; }
            set { Set(() => Version, ref _version, value); }
        }

        private DateTime _latestUpdatedDate;
        public DateTime LatestUpdatedDate
        {
            get { return _latestUpdatedDate; }
            set { Set(() => LatestUpdatedDate, ref _latestUpdatedDate, value); }
        }
    }
}
