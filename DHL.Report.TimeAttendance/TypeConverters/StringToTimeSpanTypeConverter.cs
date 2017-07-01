using AutoMapper;
using System;

namespace DHL.Report.TimeAttendance.TypeConverters
{
    public class StringToTimeSpanTypeConverter : ITypeConverter<TimeSpan, string>
    {
        public string Convert(TimeSpan source, string destination, ResolutionContext context)
        {
            return source.ToString("hh:mm");
        }
    }
}
