using System;
using System.Collections.Generic;

namespace M3Pact.Business.Common
{
    public class Utility
    {

        /// <summary>
        /// To get the working day start date including holidays , weekends when number of last working days are given.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="numberOfLastWorkingDays"></param>
        /// <param name="holidays"></param>
        /// <returns></returns>
        public DateTime GetWorkingDayStartDate(DateTime date, int numberOfLastWorkingDays, List<DateTime> holidays)
        {
            DateTime newDate = date;
            while (numberOfLastWorkingDays != 0)
            {
                newDate = newDate.AddDays(-1);
                if (newDate.DayOfWeek != DayOfWeek.Saturday &&
                    newDate.DayOfWeek != DayOfWeek.Sunday &&
                    !holidays.Contains(newDate.Date))
                {
                    numberOfLastWorkingDays--;
                }
            }
            return newDate;
        }

        /// <summary>
        /// To get the list of week ends when start date and end date are given.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<string> GetWeekEndsBetweenStartDateAndEndDate(DateTime startDate, DateTime endDate)
        {
            List<string> weekEndsList = new List<string>();
            TimeSpan diff = endDate - startDate;
            int days = diff.Days;
            for (int i = 0; i <= days; i++)
            {
                DateTime testDate = startDate.AddDays(i);
                switch (testDate.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                    case DayOfWeek.Sunday:
                        weekEndsList.Add(testDate.ToShortDateString());
                        break;
                }
            }
            return weekEndsList;
        }

        /// <summary>
        /// Get start date when last number of weeks are given.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="numberOfWeeks"></param>
        /// <returns></returns>
        public DateTime GetStartDateFromNumberOfWeeks(DateTime date, int numberOfWeeks)
        {
            DateTime newDate = date;
            return newDate.AddDays(numberOfWeeks * -7);
        }

    }
}
