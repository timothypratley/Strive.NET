using System;

namespace Strive.Common
{
    public static class Vernacular
    {
        public static string Description(this TimeSpan ts)
        {
            if (ts <= TimeSpan.Zero)
                return "now";
            if (ts < TimeSpan.FromMinutes(1))
                return "soon";
            if (ts < TimeSpan.FromHours(1))
                return "later";
            if (ts < TimeSpan.FromDays(1))
                return "today";
            if (ts < TimeSpan.FromDays(7))
                return "this week";
            if (ts < TimeSpan.FromDays(30))
                return "this month";
            if (ts < TimeSpan.FromDays(365))
                return "this year";
            return "never";
        }
    }
}
