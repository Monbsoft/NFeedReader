using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NFeedReader.Helpers
{
    public static class DateHelper
    {
        public static  string Display(DateTime date)
        {
            var timespan = DateTime.Now - date;

            if(timespan.TotalDays > 365)
            {
                return date.ToString("YYYY-MM-dd");
            }
            else if(timespan.TotalDays > 30)
            {
                int months = Convert.ToInt32(timespan.TotalDays / 30);
                return Write(months, "month");
            }
            else if(timespan.TotalDays >= 1)
            {
                int days = Convert.ToInt32(timespan.TotalDays);
                return Write(days, "day");
            }
            else if(timespan.TotalHours >= 1)
            {
                int hours = Convert.ToInt32(timespan.TotalHours);
                return Write(hours, "hour");
            }
            else if(timespan.TotalMinutes >= 1)
            {
                int minutes = Convert.ToInt32(timespan.TotalMinutes);
                return Write(minutes, "minute");
            }
            else
            {
                return "now";
            }
        }

        private static string Write(int units, string singular, string plural = null)
        {
            plural = $"{singular}s";
            if(units > 1)
            {
                return $"{units} {plural} ago";
            }
            else
            {
                return $"{units} {singular} ago";
            }
        }
    }
}
