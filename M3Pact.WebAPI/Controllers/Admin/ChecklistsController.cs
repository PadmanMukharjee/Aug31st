using M3Pact.Infrastructure.Interfaces.Business.Admin;
using M3Pact.ViewModel.Admin;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using M3Pact.WebAPI.Filters;
using M3Pact.Infrastructure;
using System.Threading.Tasks;
using M3Pact.ViewModel.Mapper;
using M3Pact.Infrastructure.Common;
using System.Linq;

namespace M3Pact.WebAPI.Controllers.Admin
{
    [Route("api/[controller]")]
    [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager, Roles.User)]
    public class ChecklistsController : Controller
    {
        private readonly ICheckListBusiness _checkListBusiness;
        public ChecklistsController(ICheckListBusiness checkListBusiness)
        {
            _checkListBusiness = checkListBusiness;
        }

        /// <summary>
        /// to get a checklist by id if exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _checkListBusiness.GetCheckListById(id);
            return Ok(new { Checklists = result.CheckListMap() });
        }

        /// <summary>
        /// To get All the checklists 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllChecklists")]
        public List<AllChecklistViewModel> GetAllChecklists()
        {
            return _checkListBusiness.GetAllChecklists();
        }

        /// <summary>
        /// To create a checklist
        /// </summary>
        /// <param name="checkListViewModel"></param>
        /// <returns>asynchronous IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CheckListViewModel checkListViewModel)
        {
            if (checkListViewModel == default(CheckListViewModel))
            {
                return BadRequest();
            }
            var result = await _checkListBusiness.CreateCheckList(checkListViewModel.CheckListMap());

            if (result == null || (result != null && result.CheckListId < 1))
            {
                return BadRequest();
            }

            checkListViewModel.ChecklistId = result.CheckListId;
            return Created(Request.Path, checkListViewModel);
        }

        /// <summary>
        /// To get checklist types
        /// </summary>
        /// <returns>Asynchronous Action Result</returns>
        [HttpGet]
        [Route("checklisttypes")]
        public async Task<IActionResult> GetCheckListTypes()
        {
            var result = await _checkListBusiness.GetCheckListTypes();
            var keyvalueModel = result.CheckListTypeMap();
            if (keyvalueModel == default(List<KeyValueModel>) || !keyvalueModel.Any())
            {
                return NotFound();
            }
            return Ok(keyvalueModel);

        }

        /// <summary>
        /// To check whether the client dependency exists when a system is removed from a checklist
        /// </summary>
        /// <param name="checklistId"></param>
        /// <param name="systemId"></param>
        /// <returns>Asynchronous ok result</returns>
        [HttpGet]
        [Route("checkclientdependencyonsystem")]
        public async Task<IActionResult> CheckClientDependencyOnDelteSystem([FromQuery]int checklistId, [FromQuery]int systemId)
        {
            var result = _checkListBusiness.CheckClientDependencyOnDelteSystem(systemId, checklistId);
            return Ok(new { Dependent = result });
        }

        /// <summary>
        /// To check whether the client dependency exists when a site is removed from a checklist
        /// </summary>
        /// <param name="checklistId"></param>
        /// <param name="siteId"></param>
        /// <returns>Asynchronous ok result</returns>
        [HttpGet]
        [Route("checkclientdependencyonsite")]
        public async Task<IActionResult> CheckClientDependencyOnDelteSite([FromQuery]int checklistId, [FromQuery]int siteId)
        {
            var result = _checkListBusiness.CheckClientDependencyOnDelteSite(siteId, checklistId);
            return Ok(new { Dependent = result });
        }

        /// <summary>
        /// To update checklist resource
        /// </summary>
        /// <param name="checkListViewModel"></param>
        /// <returns>Asynchronous IActionResult</returns>

        [HttpPost]
        [Route("save")]
        public async Task<IActionResult> Save([FromBody] CheckListViewModel checkListViewModel)
        {
            if (checkListViewModel == default(CheckListViewModel))
            {
                return BadRequest();
            }
            var result = await _checkListBusiness.UpdateCheckList(checkListViewModel.CheckListMap());

            if (result == null || (result != null && result.CheckListId < 1))
            {
                return BadRequest();
            }

            return Ok(checkListViewModel);
        }

        /// <summary>
        /// to get weekly checklist by querying
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("weeklychecklists")]
        public async Task<IActionResult> GetWeeklyCheckLists([FromQuery] int systemId, [FromQuery] int siteId)
        {
            if (systemId == 0 || siteId == 0)
            {
                return BadRequest();
            }
            var result = await _checkListBusiness.GetCheckListByQuery(new CheckListQueryModel { CheckListTypeCode = DomainConstants.WEEK, SiteId = siteId, SystemId = systemId });

            return Ok(new { checklists = result.KeyValueMap() });
        }

        /// <summary>
        /// to get monthly checklist by querying
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("monthlychecklists")]
        public async Task<IActionResult> GetMonthyCheckLists([FromQuery] int systemId, [FromQuery] int siteId)
        {
            if (systemId == 0 || siteId == 0)
            {
                return BadRequest();
            }
            var result = await _checkListBusiness.GetCheckListByQuery(new CheckListQueryModel { CheckListTypeCode = DomainConstants.MONTH, SiteId = siteId, SystemId = systemId });

            return Ok(new { checklists = result.KeyValueMap() });
        }

    }
}