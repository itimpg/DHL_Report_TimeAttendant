using System;

namespace DHL.Report.TimeAttendance.Helpers
{
    public static class Converter
    {
        public static string ToData(this TimeSpan ts)
        {
            return $"{(ts.Days * 24 + ts.Hours).ToString("00")}:{ts.Minutes.ToString("00")}";
        }

        public static string ToData(this TimeSpan? ts)
        {
            if (!ts.HasValue)
            {
                return " - ";
            }
            return ToData(ts.Value);
        }

        public static string ToData(this DateTime dt)
        {
            return dt.ToString("dd/MM/yyyy HH:mm");
        }

        public static string ToData(this DateTime? dt)
        {
            if (!dt.HasValue)
            {
                return " - ";
            }
            return ToData(dt.Value);
        }
    }
}
