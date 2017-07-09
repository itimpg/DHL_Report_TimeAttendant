using System;
using System.Collections.Generic;
using System.Linq;

namespace DHL.Report.TimeAttendance.Helpers
{
    public static class LinqHelper
    {
        public static TimeSpan Average(this IEnumerable<TimeSpan> source)
        {
            return new TimeSpan((long)source.Average(x => x.Ticks));
        }
    }
}
