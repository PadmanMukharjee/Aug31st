using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Interfaces.Business.Admin;
using M3Pact.ViewModel.Admin;
using M3Pact.ViewModel.Client;
using M3Pact.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace M3Pact.WebAPI.Controllers.Admin
{
    [Route("api/[controller]/[Action]")]
    public class KPIController : Controller
    {
        private IKPIBusiness _kpiBusiness;
        public KPIController(IKPIBusiness kpiBusiness)
        {
            _kpiBusiness = kpiBusiness;

        }

        /// <summary>
        /// API call to get KPI questions
        /// </summary>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager)]
        [HttpGet]
        public List<MeasureViewModel> GetKPIQuestionBasedOnCheckListType(string checkListTypeCode)
        {
            return _kpiBusiness.GetKPIQuestionBasedOnCheckListType(checkListTypeCode);
        }

        /// <summary>
        /// API call to get ChecklistType
        /// </summary>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager)]
        [HttpGet]
        public List<CheckListTypeViewModel> GetCheckListTypes()
        {
            return _kpiBusiness.GetCheckListTypes();
        }

        /// <summary>
        /// API call to save KPI details
        /// </summary>
        /// <param name="kpiViewModel"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive)]
        [HttpPost]
        public bool SaveKPIDetails([FromBody] KPIViewModel kpiViewModel)
        {
            return _kpiBusiness.SaveKPIDetails(kpiViewModel);
        }


        /// <summary>
        /// API call to get measure based on checkListType
        /// </summary>
        /// <param name="checkListTypeId"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager)]
        [HttpGet]
        public List<KPIMeasureViewModel> GetMeasureBasedOnClientTypeID(int checkListTypeId)
        {
            return _kpiBusiness.GetMeasureBasedOnClientTypeID(checkListTypeId);
        }

        /// <summary>
        /// API call to get all KPIS
        /// </summary>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager)]
        [HttpGet]
        public List<KPIViewModel> GetAllKPIs()
        {
            return _kpiBusiness.GetAllKPIs();
        }

        /// <summary>
        /// API call to get all M3Metrics KPIS
        /// </summary>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager)]
        [HttpGet]
        public List<KPIViewModel> GetAllM3MetricsKPIs()
        {
            return _kpiBusiness.GetAllM3MetricsKPIs();
        }

        /// <summary>
        /// API call to get KPI based on ID
        /// </summary>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager)]
        [HttpGet]
        public KPIViewModel GetKPIById(int KPIId)
        {
            return _kpiBusiness.GetKPIById(KPIId);
        }

        /// <summary>
        /// To get KPI questions that can be assigned for Client
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager)]
        [HttpGet]
        public List<KPIQuestionViewModel> GetKPIQuestionsForClient(string clientCode)
        {
            return _kpiBusiness.GetKPIQuestionsForClient(clientCode);
        }

        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager)]
        [HttpPost]
        public bool SaveKPIForClient([FromBody]ClientKPISetupViewModel clientKPI)
        {
            return _kpiBusiness.SaveKPIForClient(clientKPI);
        }

        /// <summary>
        /// To get Assigned KPIs for Client
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager, Roles.Client, Roles.User)]
        [HttpGet]
        public ClientKPISetupViewModel GetAssignedKPIForClient(string clientCode)
        {
            return _kpiBusiness.GetClientAssignedKPIs(clientCode);
        }

        /// <summary>
        /// To get the kpi ids which are heat map items.
        /// </summary>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager)]
        [HttpGet]
        public List<int> GetKpiHeatMapItems()
        {
            return _kpiBusiness.GetKpiHeatMapItems();
        }
    }
}
