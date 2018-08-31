using M3Pact.ViewModel.Admin;
using M3Pact.Infrastructure.Interfaces.Business.Admin;
using M3Pact.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using M3Pact.WebAPI.Filters;
using M3Pact.Infrastructure;

namespace M3Pact.WebAPI.Controllers.Admin
{
    [AuthorizationFilter]
    [Route("api/[controller]/[Action]")]
    public class BusinessDaysController : Controller
    {
        #region Private Properties

        private IBusinessDaysBusiness _businessDaysBusiness;

        #endregion Private Properties

        #region Constructor
        public BusinessDaysController(IBusinessDaysBusiness businessDaysBusiness)
        {
            _businessDaysBusiness = businessDaysBusiness;
        }

        #endregion Constructor

        #region API Methods

        /// <summary>
        /// API call to return the Businessdays
        /// </summary>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager)]
        [HttpGet]
        public List<BusinessDaysViewModel> GetBusinessDays()
        {
            return _businessDaysBusiness.GetBusinessDays();
        }

        /// <summary>
        /// API call to Save the Businessdays
        /// </summary>
        /// <param name="businessDays"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive)]
        [HttpPost]
        public bool SaveBusinessDays(List<BusinessDaysViewModel> businessDays)
        {
            if (businessDays != null && businessDays.Count > 0)
            {
                return _businessDaysBusiness.SaveBusinessDays(businessDays);
            }
            else
            {
                return false;
            }
        }

        [AuthorizationFilter(Roles.Admin, Roles.Executive)]
        [HttpGet]
        public List<HolidayViewModel> GetHolidaysOfYear(int year)
        {
            return _businessDaysBusiness.GetHolidaysOfYear(year);
        }

        [AuthorizationFilter(Roles.Admin, Roles.Executive)]
        [HttpGet]
        public Dictionary<string, int> GetHolidaysByMonth(int year)
        {
            return _businessDaysBusiness.GetHolidaysByMonth(year);
        }

        /// <summary>
        /// Edits Exixting Holiday
        /// </summary>
        /// <param name="holiday"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin)]
        [HttpPost]
        public bool AddOrEditHoliday([FromBody]HolidayViewModel holiday)
        {
            return _businessDaysBusiness.AddOrEditHoliday(holiday);
        }

        /// <summary>
        /// Deletes Existing Holiday
        /// </summary>
        /// <param name="holiday"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin)]
        [HttpPost]
        public bool DeleteHoliday([FromBody]HolidayViewModel holiday)
        {
            return _businessDaysBusiness.DeleteHoliday(holiday);
        }

        /// <summary>
        /// updates client targets.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin)]
        [HttpGet]
        public bool UpdateClientTargets(int year)
        {
            return _businessDaysBusiness.UpdateClientTargets(year);
        }

        #endregion API Methods

    }
}
