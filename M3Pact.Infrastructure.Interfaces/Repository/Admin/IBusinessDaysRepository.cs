using M3Pact.BusinessModel.Admin;
using M3Pact.BusinessModel.BusinessModels;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Repository.Admin
{
    public interface IBusinessDaysRepository
    {
        List<BusinessDays> GetBusinessDays();

        bool SaveBusinessDays(List<BusinessDays> businessDays);

        IEnumerable<Holiday> GetHolidaysOfYear(int year);

        /// <summary>
        /// Edits Existing Holiday
        /// </summary>
        /// <param name="holiday"></param>
        /// <returns></returns>
        bool AddOrEditHoliday(Holiday holiday);

        /// <summary>
        /// Deletes Existing Holiday
        /// </summary>
        /// <param name="holiday"></param>
        /// <returns></returns>
        bool DeleteHoliday(Holiday holiday);
        Dictionary<string, int> GetHolidaysByMonth(int year);
        /// <summary>
        /// updates respective month days in business days table
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        bool UpdateBusinessDays(int year);

        /// <summary>
        /// gets businessdays of a month from date dimension table
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        Dictionary<int, int> GetBusinessDaysOfMonth(int year);
    }
}
