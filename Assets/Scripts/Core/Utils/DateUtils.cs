using System;

namespace Core.Utils
{
    internal static class DateUtils
    {
        private static readonly DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        internal static long ToUnixTime(this DateTime date)
        {
            var diff = date.ToUniversalTime() - origin;
            return (long) Math.Floor(diff.TotalMilliseconds);
        }

        internal static DateTime ToDateTime(this long millis)
        {
            return origin.AddMilliseconds(millis).ToLocalTime();
        }
    }
}