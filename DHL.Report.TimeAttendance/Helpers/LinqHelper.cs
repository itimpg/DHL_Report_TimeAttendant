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

        public static IEnumerable<TResult> SelectWithPrev<TSource, TResult>
            (this IEnumerable<TSource> source,
            Func<TSource, TSource, bool, TResult> projection)
        {
            using (var iterator = source.GetEnumerator())
            {
                var isfirst = true;
                var previous = default(TSource);
                while (iterator.MoveNext())
                {
                    yield return projection(iterator.Current, previous, isfirst);
                    isfirst = false;
                    previous = iterator.Current;
                }
            }
        }
    }
}
