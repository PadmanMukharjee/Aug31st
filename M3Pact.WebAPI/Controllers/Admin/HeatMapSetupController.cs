using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Interfaces.Business.Admin;
using M3Pact.ViewModel.Admin;
using M3Pact.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace M3Pact.WebAPI.Controllers.Admin
{
    [Route("api/[controller]/[Action]")]
    [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager)]
    public class HeatMapSetupController : Controller
    {
        private IHeatMapBusiness heatMapBusiness;

        public HeatMapSetupController(IHeatMapBusiness heatMapBusiness)
        {
            this.heatMapBusiness = heatMapBusiness;
        }

        /// <summary>
        /// To get all KPIs which are eligible for heat map
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<HeatMapViewModel> GetKpisforHeatMap()
        {
            return heatMapBusiness.GetKpisforHeatMap();
        }

        /// <summary>
        /// To get the existing Heatmap Items
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<HeatMapViewModel> GetHeatMapItems()
        {
            return heatMapBusiness.GetHeatMapItems();
        }

        /// <summary>
        /// Save the updated heat map items
        /// </summary>
        /// <param name="heatMapViewModels"></param>
        /// <returns></returns>
        [HttpPost]
        public bool SaveHeatMapItems([FromBody]List<HeatMapViewModel> heatMapViewModels)
        {
            return heatMapBusiness.SaveHeatMapItems(heatMapViewModels);
        }
    }
}