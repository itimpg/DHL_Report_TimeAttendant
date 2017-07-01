using GalaSoft.MvvmLight;
using System;

namespace DHL.Report.TimeAttendance.Models
{
    public class ShiftModel : ObservableObject
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { Set(() => Id, ref _id, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { Set(() => Name, ref _name, value); }
        }

        private string _code;
        public string Code
        {
            get { return _code; }
            set { Set(() => Code, ref _code, value); }
        }

        private TimeSpan _workFrom;
        public TimeSpan WorkFrom
        {
            get { return _workFrom; }
            set { Set(() => WorkFrom, ref _workFrom, value); }
        }

        private TimeSpan _workTo;
        public TimeSpan WorkTo
        {
            get { return _workTo; }
            set { Set(() => WorkTo, ref _workTo, value); }
        }

        private TimeSpan _mealFrom;
        public TimeSpan MealFrom
        {
            get { return _mealFrom; }
            set { Set(() => MealFrom, ref _mealFrom, value); }
        }

        private TimeSpan _mealTo;
        public TimeSpan MealTo
        {
            get { return _mealTo; }
            set { Set(() => MealTo, ref _mealTo, value); }
        }

        private TimeSpan _breakFrom;
        public TimeSpan BreakFrom
        {
            get { return _breakFrom; }
            set { Set(() => BreakFrom, ref _breakFrom, value); }
        }

        private TimeSpan _breakTo;
        public TimeSpan BreakTo
        {
            get { return _breakTo; }
            set { Set(() => BreakTo, ref _breakTo, value); }
        }
    }
}
