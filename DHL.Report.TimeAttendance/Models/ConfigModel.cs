using GalaSoft.MvvmLight;
using System;

namespace DHL.Report.TimeAttendance.Models
{
    public class ConfigModel : ObservableObject
    {
        private Shift _morningShift;
        public Shift MorningShift
        {
            get { return _morningShift; }
            set { Set(() => MorningShift, ref _morningShift, value); }
        }

        private Shift _eveningShift;
        public Shift EveningShift
        {
            get { return _eveningShift; }
            set { Set(() => EveningShift, ref _eveningShift, value); }
        }

        private Shift _nightShift;
        public Shift NightShift
        {
            get { return _nightShift; }
            set { Set(() => NightShift, ref _nightShift, value); }
        }

        private Shift _morningShiftWithOT;
        public Shift MorningShiftWithOT
        {
            get { return _morningShiftWithOT; }
            set { Set(() => MorningShiftWithOT, ref _morningShiftWithOT, value); }
        }

        private Shift _nightShiftWithOT;
        public Shift NightShiftWithOT
        {
            get { return _nightShiftWithOT; }
            set { Set(() => NightShiftWithOT, ref _nightShiftWithOT, value); }
        }
    }

    public class Shift : ObservableObject
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { Set(() => Name, ref _name, value); }
        }

        private TimeSpan _from;
        public TimeSpan From
        {
            get { return _from; }
            set { Set(() => From, ref _from, value); }
        }

        private TimeSpan _to;
        public TimeSpan To
        {
            get { return _to; }
            set { Set(() => To, ref _to, value); }
        }
    }
}
