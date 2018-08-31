using System.Collections.Generic;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Interfaces.Business.HeatMap;
using M3Pact.ViewModel.HeatMap;
using M3Pact.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using M3Pact.Infrastructure.Interfaces.Business;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace M3Pact.WebAPI.Controllers
{

    [Route("api/[controller]/[Action]")]
    public class HeatMapController : Controller
    {
        #region Internal Properties
        private IClientsHeatMapBusiness _clientsHeatMapBusiness;
        #endregion Internal Properties

        #region Constructor
        public HeatMapController(IClientsHeatMapBusiness clientsHeatMapBusiness)
        {
            _clientsHeatMapBusiness = clientsHeatMapBusiness;
        }
        #endregion Constructor

        #region Public Methods
        /// <summary>
        /// To get the heat map filter data.
        /// </summary>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager)]
        [HttpGet]
        public HeatMapFiltersData GetHeatMapFilterData()
        {
            return _clientsHeatMapBusiness.GetHeatMapFilterData();
        }

        /// <summary>
        /// Get HeatMap For Clients
        /// </summary>
        /// <param name="heatMapRequest"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager)]
        [HttpPost]
        public string GetHeatMapForClients([FromBody]ClientsHeatMapRequestViewModel heatMapRequest)
        {
            string heatMapResponse = _clientsHeatMapBusiness.GetClientsHeatMap(heatMapRequest);
            return heatMapResponse;
        }
        #endregion Public Methods
    }
}
