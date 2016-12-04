/*技术支持QQ群：226090960*/
using System;

namespace SmartAuthority.Util
{
    public static class DateHelper
    {
        public static DateTime ToDayStart(this string value)
        {
            DateTime date = Convert.ToDateTime(value);
            string shortDate = date.ToShortDateString();
            return Convert.ToDateTime(shortDate);
        }

        public static DateTime ToDayStart(this DateTime value)
        {
            string shortDate = value.ToShortDateString();
            return Convert.ToDateTime(shortDate);
        }

        public static DateTime ToDayEnd(this string value)
        {
            DateTime date = Convert.ToDateTime(value);
            DateTime shortDate = Convert.ToDateTime(date.ToShortDateString());
            return shortDate.AddDays(1).AddSeconds(-1);
        }

        public static DateTime ToDayEnd(this DateTime value)
        {
            DateTime shortDate = Convert.ToDateTime(value.ToShortDateString());
            return shortDate.AddDays(1).AddSeconds(-1);
        }

        public static DateTime ToWeekStart(this string value)
        {
            DateTime date = Convert.ToDateTime(value);
            DateTime weekStart = ToWeekStart(date, date.Date.DayOfWeek);
            string shortDate = weekStart.ToShortDateString();
            return Convert.ToDateTime(shortDate);
        }

        public static DateTime ToWeekStart(this DateTime value)
        {
            DateTime weekStart = ToWeekStart(value, value.Date.DayOfWeek);
            string shortDate = weekStart.ToShortDateString();
            return Convert.ToDateTime(shortDate);
        }

        public static DateTime ToWeekEnd(this string value)
        {
            DateTime date = Convert.ToDateTime(value);
            DateTime weekEnd = ToWeekEnd(date, date.Date.DayOfWeek);
            string shortDate = weekEnd.ToShortDateString();
            return Convert.ToDateTime(shortDate).AddDays(1).AddSeconds(-1);
        }

        public static DateTime ToWeekEnd(this DateTime value)
        {
            DateTime weekEnd = ToWeekEnd(value, value.Date.DayOfWeek);
            string shortDate = weekEnd.ToShortDateString();
            return Convert.ToDateTime(shortDate).AddDays(1).AddSeconds(-1);
        }

        private static DateTime ToWeekStart(this DateTime date, DayOfWeek week)
        {
            DateTime WeekStart = new DateTime();
            switch (week)
            {
                case DayOfWeek.Monday:
                    WeekStart = date;
                    break;
                case DayOfWeek.Tuesday:
                    WeekStart = date.AddDays(-1);
                    break;
                case DayOfWeek.Wednesday:
                    WeekStart = date.AddDays(-2);
                    break;
                case DayOfWeek.Thursday:
                    WeekStart = date.AddDays(-3);
                    break;
                case DayOfWeek.Friday:
                    WeekStart = date.AddDays(-4);
                    break;
                case DayOfWeek.Saturday:
                    WeekStart = date.AddDays(-5);
                    break;
                case DayOfWeek.Sunday:
                    WeekStart = date.AddDays(-6);
                    break;
            }
            return WeekStart;
        }

        private static DateTime ToWeekEnd(this DateTime date, DayOfWeek week)
        {
            DateTime WeekStart = new DateTime();
            switch (week)
            {
                case DayOfWeek.Monday:
                    WeekStart = date.AddDays(6);
                    break;
                case DayOfWeek.Tuesday:
                    WeekStart = date.AddDays(5);
                    break;
                case DayOfWeek.Wednesday:
                    WeekStart = date.AddDays(4);
                    break;
                case DayOfWeek.Thursday:
                    WeekStart = date.AddDays(3);
                    break;
                case DayOfWeek.Friday:
                    WeekStart = date.AddDays(2);
                    break;
                case DayOfWeek.Saturday:
                    WeekStart = date.AddDays(1);
                    break;
                case DayOfWeek.Sunday:
                    WeekStart = date;
                    break;
            }
            return WeekStart;
        }

        public static DateTime ToMonthStart(this string value)
        {
            DateTime date = Convert.ToDateTime(value);
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime ToMonthStart(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }

        public static DateTime ToMonthEnd(this string value)
        {
            DateTime date = Convert.ToDateTime(value);
            DateTime monthStart = new DateTime(date.Year, date.Month, 1);
            return monthStart.AddMonths(1).AddSeconds(-1);
        }

        public static DateTime ToMonthEnd(this DateTime value)
        {
            DateTime monthStart = new DateTime(value.Year, value.Month, 1);
            return monthStart.AddMonths(1).AddSeconds(-1);
        }

        public static DateTime ToYearStart(this string value)
        {
            DateTime date = Convert.ToDateTime(value);
            return new DateTime(date.Year, 1, 1);
        }

        public static DateTime ToYearStart(this DateTime value)
        {
            return new DateTime(value.Year, 1, 1);
        }

        public static DateTime ToYearEnd(this string value)
        {
            DateTime date = Convert.ToDateTime(value);
            DateTime yearStart = new DateTime(date.Year, 1, 1);
            DateTime yearEnd = new DateTime(date.Year, 1, 1).AddYears(1).AddSeconds(-1);
            return yearEnd;
        }

        public static DateTime ToYearEnd(this DateTime value)
        {
            DateTime yearStart = new DateTime(value.Year, 1, 1);
            return new DateTime(value.Year, 1, 1).AddYears(1).AddSeconds(-1);
        }
    }
}
