using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fgly.Common.Expand
{
    public static class _Long
    {
        public static DateTime ToDateTime(this System.Int64 UnixTime)
        {
            DateTime time = DateTime.MinValue;
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            time = startTime.AddSeconds(UnixTime);
            return time;
        }
    }
}
