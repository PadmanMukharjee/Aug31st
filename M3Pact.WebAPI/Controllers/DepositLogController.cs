using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using M3Pact.Infrastructure.Interfaces.Business.DepositLog;
using M3Pact.ViewModel;
using System.Globalization;
using M3Pact.ViewModel.DepositLog;
using Microsoft.AspNetCore.Authorization;
using M3Pact.WebAPI.Filters;
using M3Pact.Infrastructure;
using Newtonsoft.Json;

namespace M3Pact.WebAPI.Controllers
{
    [AuthorizationFilter]
    [Route("api/[controller]/[Action]")]
    public class DepositLogController : Controller
    {
        private IDepositLogBusiness _depositLogBusiness;

        public DepositLogController(IDepositLogBusiness depositLogBusiness)
        {
            _depositLogBusiness = depositLogBusiness;
        }
        /// <summary>
        /// To get DeppositLogs of a Client
        /// </summary>
        /// <param name="ClientCode"></param>
        /// <returns></returns>
       // [AuthorizationFilter(Roles.Manager,Roles.Admin,Roles.User)]
        [HttpGet]
        public DepositLogClientDataAmountViewModel GetClientDepositLogs(string clientCode, int month, int year)
        {
            return _depositLogBusiness.GetClientDepositLogData(clientCode, month, year);
        }

        /// <summary>
        /// Tp Post DepositLogs of a Client to Save
        /// </summary>
        /// <param name="DepositeLogData"></param>
        [AuthorizationFilter(Roles.Manager, Roles.Admin, Roles.User)]
        [HttpPost]
        public ValidationViewModel SaveClientDepositLogs([FromBody]DepositLogClientDataViewModel depositeLogData)
        {
            return _depositLogBusiness.SaveClientDepositLogData(depositeLogData);
        }

        /// <summary>
        /// To get Payers associated to a Client
        /// </summary>
        /// <param name="ClientCode"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Manager, Roles.Admin, Roles.User)]
        [HttpGet]
        public DepositLogClientDataViewModel GetPayersForClient(string clientCode)
        {
            return _depositLogBusiness.GetPayerDetailsForClient(clientCode);
        }
        /// <summary>
        /// To Get DepositLogs depending on Date and ClientCode
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="depositDate"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Manager, Roles.Admin, Roles.User)]
        [HttpGet]
        public DepositLogClientDataViewModel GetPayersForClientDate([FromQuery]string clientCode, [FromQuery]DateTime depositDate)
        {
            return _depositLogBusiness.GetDepositLogsOfClientForDate(clientCode, depositDate);
        }

        /// <summary>
        /// To get the Deposit Log MTD values
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet]
        public DepositLogMTDViewModel GetDepositLogMTD(string clientCode, int month, int year)
        {
            return _depositLogBusiness.GetDepositLogMTD(clientCode, month, year);
        }


        [HttpGet]
        public DepositLogMonthlyTargetViewModel GetDepositLogMonthlyTargets(string clientCode, int month, int year)
        {
            return _depositLogBusiness.GetDepositLogMonthlyTargets(clientCode, month, year);
        }

        /// <summary>
        /// Fetches total no of business days in a Month of an Year
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        [HttpGet]
        public int? GetTotalBusinessDaysInaMonthOfAnYear(int year, int month)
        {
            return _depositLogBusiness.GetTotalBusinessDaysInaMonthOfAnYear(year, month);
        }

        /// <summary>
        /// To get the number of deposit days of a client for a given month.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        [HttpGet]
        public int GetNumberOfDepositDaysOfClientForGivenMonth(string clientCode, int year, int month)
        {
            return _depositLogBusiness.GetNumberOfDepositDaysOfClientForGivenMonth(clientCode, month, year);

        }

        /// <summary>
        /// To get the projected cash From the deposit log when last number of wotking days are given.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="numberOfLastWorkingDays"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="savedLastNumberOfBusinessDays"></param>
        /// <returns></returns>
        [HttpGet]
        public DepositLogProjectionViewModel GetProjectedCashFromSimpleBusinessDays(string clientCode, int numberOfLastWorkingDays, int month, int year, int savedLastNumberOfBusinessDays)
        {
            return _depositLogBusiness.GetProjectedCashFromSimpleBusinessDays(clientCode, numberOfLastWorkingDays, month, year, savedLastNumberOfBusinessDays);
        }

        /// <summary>
        /// To get the projected cash From the deposit log when last number of weeks are given.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="numberOfWeeks"></param>
        /// <returns></returns>

        [HttpGet]
        public DepositLogProjectionViewModel GetProjectedCashOfPreviousWeeks(string clientCode, int month, int year, int numberOfWeeks)
        {
            return _depositLogBusiness.GetProjectedCashOfPreviousWeeks(clientCode, month, year, numberOfWeeks);
        }

        /// <summary>
        /// To get the projected cash of last working days.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="numberOfLastWorkingDays"></param>
        /// <returns></returns>
        [HttpGet]
        public DepositLogProjectedCashAmountWithPayment GetProjectedCashOfLastWorkingDays(string clientCode, int month, int year, int numberOfLastWorkingDays)
        {
            return _depositLogBusiness.GetProjectedCashOfLastWorkingDays(clientCode, month, year, numberOfLastWorkingDays);
        }

        /// <summary>
        /// To get the number of working days between deposit log Start date and the current date.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>

        [HttpGet]
        public List<int> GetNumberOfDepositDaysOfClient(string clientCode, int month, int year)
        {
            return _depositLogBusiness.GetNumberOfDepositDaysOfClient(clientCode, month, year);
        }

        /// <summary>
        /// To get holiday dates.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<DateTime> GetHolidayDates()
        {
            return _depositLogBusiness.GetHolidayDates();
        }

        /// <summary>
        /// Get Entered DepositDates For Client
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        [HttpGet]
        public List<DateTime> GetEnteredDepositDatesForClient(string clientCode)
        {
            return _depositLogBusiness.GetEnteredDepositDatesForClient(clientCode);
        }

        /// <summary>
        /// To save the last entered business days or weeks.
        /// </summary>
        /// <param name="depositeLogAttribute"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin)]
        [HttpPost]
        public ValidationViewModel SaveLastEnteredBusinessDaysOrWeeks([FromBody]DepositLogAttributeViewModel depositeLogAttribute)
        {
            return _depositLogBusiness.SaveLastEnteredBusinessDaysOrWeeks(depositeLogAttribute.ClientCode, depositeLogAttribute.BusinessDaysOrWeeksValue, depositeLogAttribute.AttributeName);
        }

        /// <summary>
        /// To get the saved last number of business days or weeks.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        [HttpGet]
        public Dictionary<string, string> GetSavedLastNumberOfBusinessDaysOrWeeks(string clientCode)
        {
            return _depositLogBusiness.GetSavedLastNumberOfBusinessDaysOrWeeks(clientCode);
        }

        /// <summary>
        /// Close Or Reopen A Month Of A Year For Client
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="monthID"></param>
        /// <param name="year"></param>
        /// <param name="isCloseMonth"></param>
        /// <returns></returns>
        [HttpPost]
        public ValidationViewModel CloseOrReopenAMonthOfAYearForClient(string clientCode, int monthID, int year, bool isCloseMonth)
        {
            return _depositLogBusiness.CloseOrReopenAMonthOfAYearForClient(clientCode, monthID, year, isCloseMonth);
        }

        /// <summary>
        /// Get Client DepositLog For A Year (Month wise)
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet]
        public List<MonthDepositViewModel> GetClientDepositLogForAYear(string clientCode, int year)
        {
            return _depositLogBusiness.GetClientMonthWiseDepositLogForAYear(clientCode, year);
        }

        /// <summary>
        /// To export deposit log data
        /// </summary>
        /// <param name="exportDepositViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult OnDepositDataExport([FromBody]ExportDepositViewModel exportDepositViewModel)
        {
            var data = JsonConvert.SerializeObject(exportDepositViewModel);
            Console.WriteLine(data);
            string sFileName = @"demo.xlsx";
            var memory = _depositLogBusiness.GetDepositLogsForSelectedMonths(exportDepositViewModel.ClientCode, exportDepositViewModel.ExportMonths);
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }

        /// <summary>
        /// To get the minimum deposit year for client
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        [HttpGet]
        public int GetMinimumDepositYear(string clientCode)
        {
            return _depositLogBusiness.GetMinimumDepositYear(clientCode);
        }
    }
}
