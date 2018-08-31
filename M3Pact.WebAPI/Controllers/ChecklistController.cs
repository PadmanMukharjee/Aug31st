using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Interfaces.Business.Checklist;
using M3Pact.ViewModel;
using M3Pact.ViewModel.Checklist;
using M3Pact.WebAPI.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace M3Pact.WebAPI.Controllers
{    
    [Route("api/[controller]/[Action]")]
    public class ChecklistController : Controller
    {
        #region properties
        private readonly IPendingChecklistBusiness _pendingCheckListBusiness;
        #endregion properties

        #region constructor
        public ChecklistController(IPendingChecklistBusiness pendingCheckListBusiness)
        {
            _pendingCheckListBusiness = pendingCheckListBusiness;
        }
        #endregion constructor

        /// <summary>
        /// To Get the Weekly Pending Checklists
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager, Roles.User)]
        [HttpGet]        
        public List<DateTime> GetWeeklyPendingChecklist(string clientCode)
        {
            return _pendingCheckListBusiness.GetWeeklyPendingChecklist(clientCode);
        }

        /// <summary>
        /// To get the monthly Pending checklists
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager, Roles.User)]
        [HttpGet]
        public List<DateTime> GetMonthlyPendingChecklist(string clientCode)
        {
            return _pendingCheckListBusiness.GetMonthlyPendingChecklist(clientCode);
        }

        /// <summary>
        /// To get the Weeklist Questions both monthly and weekly
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="pendingChecklistDate"></param>
        /// <param name="checklistType"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager, Roles.User)]
        [HttpGet]
        public List<ClientChecklistResponseViewModel> GetPendingChecklistQuestions(string clientCode, DateTime pendingChecklistDate,string checklistType)
        {
            return _pendingCheckListBusiness.GetWeeklyPendingChecklistQuestions(clientCode, pendingChecklistDate,checklistType);
        }

        /// <summary>
        /// To Save or Submit Checklist Response both weekly and monthly
        /// </summary>
        /// <param name="submit"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager, Roles.User)]
        [HttpPost]
        public bool SaveOrSubmitChecklistResponse([FromBody]SubmitChecklistResponse submit)
        {           
            return _pendingCheckListBusiness.SaveOrSubmitChecklistResponse(submit.clientCode, submit.pendingDate, submit.isSubmit, submit.clientChecklistResponse);
        }

        /// <summary>
        /// Get completed checklists within a selected date range
        /// </summary>
        /// <param name="checklistDataRequest"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager, Roles.User)]
        [HttpPost]
        public string GetCompletedChecklistsForADateRange([FromBody]ChecklistDataRequestViewModel checklistDataRequest)
        {
            return _pendingCheckListBusiness.GetCompletedChecklistsForADateRange(checklistDataRequest);
        }

        [AuthorizationFilter(Roles.Admin)]
        [HttpPost]
        public ValidationViewModel OpenSelectedChecklist([FromBody]ChecklistDataRequestViewModel checklistDataRequest)
        {
            return _pendingCheckListBusiness.OpenChecklist(checklistDataRequest);
        }

        /// <summary>
        /// Get Client ChecklistType Data ie., Start of Weekly & Monthly Effective Dates
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        [HttpGet]
        public List<ChecklistDataRequestViewModel> GetClientChecklistTypeData(string clientCode)
        {
            return _pendingCheckListBusiness.GetClientChecklistTypeData(clientCode);
        }
    }
}