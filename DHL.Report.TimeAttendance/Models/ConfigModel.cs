using System;

namespace DHL.Report.TimeAttendance.Models
{
    public class ConfigModel
    {
        public Shift MorningShift { get; set; }
        public Shift EveningShift { get; set; }
        public Shift NightShift { get; set; }
        public Shift MorningShiftWithOT { get; set; }
        public Shift NightShiftWithOT { get; set; }
    }

    public class Shift
    {
        public string Name { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
    }
}
