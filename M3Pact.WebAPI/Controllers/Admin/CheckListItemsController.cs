using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using M3Pact.ViewModel.Admin;
using M3Pact.Infrastructure.Interfaces.Business.Admin;
using M3Pact.BusinessModel.BusinessModels;
using M3Pact.WebAPI.Filters;
using M3Pact.Infrastructure;
using M3Pact.BusinessModel.Admin;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace M3Pact.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [AuthorizationFilter(Roles.Admin, Roles.Executive)]
    public class CheckListItemsController : Controller
    {

        private readonly ICheckListBusiness _checkListBusiness;
        public CheckListItemsController(ICheckListBusiness checkListBusiness)
        {
            _checkListBusiness = checkListBusiness;
        }

        /// <summary>
        /// To get all checklist
        /// </summary>
        // GET: api/<controller>
        [HttpGet("{checklistType}")]
        public async Task<IActionResult> Get(string checklistType = null)
        {

            var result = await _checkListBusiness.GetQuestions(checklistType);
            if (result == default(List<Question>))
            {
                return NotFound();
            }

            return Ok(result.Select(t => new CheckListItemViewModel
            {
                ExpectedResponse = t.ExpectedResponse,
                Freeform = t.IsFreeform,
                Kpi = t.IsKpi,
                Question = t.QuestionText,
                QuestionID = t.QuestionId,
                Universal = t.IsUniversal,
                QuestionCode = t.QuestionCode,
                checkListType = new CheckListTypeViewModel
                {
                    CheckListTypeID = t.checkListType.CheckListTypeID,
                    CheckListTypeCode = t.checkListType.CheckListTypeCode,
                    CheckListTypeName = t.checkListType.CheckListTypeName
                },
                KPIDescription = t.IsKpi ? new KPIViewModel
                {
                    KPIDescription = t.KPIDescription.KPIDescription,
                    KPIID = t.KPIDescription.KPIID,
                    SendAlert = t.KPIDescription.KPIAlert.SendAlert,
                    EscalateAlert = t.KPIDescription.KPIAlert.EscalateAlert,
                    EscalateTriggerTime = t.KPIDescription.KPIAlert.EscalateTriggerTime,
                    SendToRelationshipManager = t.KPIDescription.KPIAlert.SendToRelationshipManager,
                    SendToBillingManager = t.KPIDescription.KPIAlert.SendToBillingManager,
                    KPIAlertId = t.KPIDescription.KPIAlert.KPIAlertId
                } : new KPIViewModel()
            }));
        }
        
        /// <summary>
        /// Post call to save questions as checklistitems
        /// </summary>
        /// <param name="checkListItemViewModel"></param>
        /// <returns></returns>
        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CheckListItemViewModel checkListItemViewModel)
        {
            if (checkListItemViewModel == default(CheckListItemViewModel))
            {
                return BadRequest();
            }


            var result = await _checkListBusiness.CreateQuestion(checkListItemViewModel);

            if (result == null || (result != null && result.QuestionId < 1))
            {
                return BadRequest();
            }

            checkListItemViewModel.QuestionID = result.QuestionId;
            return Created(Request.Path, checkListItemViewModel);
        }

        /// <summary>
        /// Update checklist items
        /// </summary>
        /// <param name="checkListItemViewModel"></param>
        /// <returns></returns>
        // PUT api/<controller>
        [HttpPost]
        [Route("save")]
        public async Task<IActionResult> Save([FromBody]CheckListItemViewModel checkListItemViewModel)
        {

            if (checkListItemViewModel == default(CheckListItemViewModel) || (checkListItemViewModel != null && checkListItemViewModel.QuestionID < 1))
            {
                return BadRequest();
            }

            var checklistitem = new Question
            {
                ExpectedResponse = checkListItemViewModel.ExpectedResponse,
                IsFreeform = checkListItemViewModel.Freeform,
                IsKpi = checkListItemViewModel.Kpi,
                QuestionId = checkListItemViewModel.QuestionID,
                QuestionText = checkListItemViewModel.Question,
                IsUniversal = checkListItemViewModel.Universal
            };

            var result = await _checkListBusiness.UpdateQuestion(checkListItemViewModel);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(checkListItemViewModel);
        }

        /// <summary>
        /// To get the checklist question ids which are heat map items.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetChecklistHeatMapQuestions")]
        public List<string> GetChecklistHeatMapQuestions()
        {
            return _checkListBusiness.GetChecklistHeatMapQuestions();
        }


    }
}
