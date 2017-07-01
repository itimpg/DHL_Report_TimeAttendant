using AutoMapper;
using System;

namespace DHL.Report.TimeAttendance.TypeConverters
{
    public class TimeSpanToStringTypeConverter : ITypeConverter<string, TimeSpan>
    {
        public TimeSpan Convert(string source, TimeSpan destination, ResolutionContext context)
        {
            TimeSpan timeResult;
            if (TimeSpan.TryParse(source, out timeResult))
            {
                return timeResult;
            }
            return TimeSpan.Zero;
        }
    }
}
