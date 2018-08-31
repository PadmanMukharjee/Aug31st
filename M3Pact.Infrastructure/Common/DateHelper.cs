using System;
using System.Globalization;

namespace M3Pact.Infrastructure.Common
{
    public class DateHelper
    {
        /// <summary>
        /// Get coming Monday from given day
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetMondayOfEffectiveWeek(DateTime date)
        {
            DateTime effectiveMonday = date.DayOfWeek == DayOfWeek.Sunday ? date.AddDays(1) : date.AddDays(7 + 1 - (int)date.DayOfWeek);
            return effectiveMonday.Date;
        }

        /// <summary>
        /// Get next month's first day from given day
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfEffectiveMonth(DateTime date)
        {
            DateTime effectiveMonth = date.AddMonths(1); //Next month
            DateTime firstDayOfEffectiveMonth = new DateTime(effectiveMonth.Year, effectiveMonth.Month, 1);
            return firstDayOfEffectiveMonth.Date;
        }

        public static string ToMonthName(DateTime dateTime)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month);
        }

        public static string ToShortMonthName(DateTime dateTime)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(dateTime.Month);
        }

    }
}
