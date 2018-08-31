using M3Pact.ViewModel;
using M3Pact.ViewModel.Admin;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Business.Admin
{
    public interface IBusinessDaysBusiness
    {
        List<BusinessDaysViewModel> GetBusinessDays();

        bool SaveBusinessDays(List<BusinessDaysViewModel> businessDays);

        List<HolidayViewModel> GetHolidaysOfYear(int year);

        Dictionary<string,int> GetHolidaysByMonth(int year);

        /// <summary>
        /// Edits Existing Holiday
        /// </summary>
        /// <param name="holiday"></param>
        /// <returns></returns>
        bool AddOrEditHoliday(HolidayViewModel holiday);

        /// <summary>
        /// Deletes Existing Holiday
        /// </summary>
        /// <param name="holiday"></param>
        /// <returns></returns>
        bool DeleteHoliday(HolidayViewModel holiday);

        /// <summary>
        /// updates clients targets
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        bool UpdateClientTargets(int year);

    }
}
