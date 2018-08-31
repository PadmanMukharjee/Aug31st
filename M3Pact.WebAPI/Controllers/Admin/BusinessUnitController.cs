using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Interfaces.Business.Admin;
using M3Pact.ViewModel;
using M3Pact.WebAPI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace M3Pact.WebAPI.Controllers.Admin
{
    [AuthorizationFilter]
    [Route("api/[controller]/[Action]")]
    public class BusinessUnitController : Controller
    {
        private IBusinessUnitBusiness _businessUnitBusiness;
        public BusinessUnitController(IBusinessUnitBusiness businessUnitBusiness)
        {
            _businessUnitBusiness = businessUnitBusiness;

        }

        /// <summary>
        /// API call to return the BusinessUnits
        /// </summary>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager, Roles.User)]
        [HttpGet]
        public List<BusinessUnitViewModel> GetBusinessUnits(bool fromClient = false)
        {
            return _businessUnitBusiness.GetBusinessUnits(fromClient);
        }

        /// <summary>
        /// API call to Save the BusinessUnits
        /// </summary>
        /// <param name="businessUnits"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin)]
        [HttpPost]
        public bool SaveBusinessUnits([FromBody]List<BusinessUnitViewModel> businessUnits)
        {
            if (businessUnits != null && businessUnits.Count > 0)
            {
                return _businessUnitBusiness.SaveBusinessUnits(businessUnits);
            }
            else
            {
                return false;
            }
        }

        [AuthorizationFilter(Roles.Admin)]
        [HttpPost]
        public bool ActiveOrInactiveBusinessUnit([FromBody] BusinessUnitViewModel businessUnit)
        {
            if (businessUnit != null)
            {
                return _businessUnitBusiness.ActiveOrInactiveBusinessUnit(businessUnit);
            }
            else
            {
                return false;
            }
        }

        [AuthorizationFilter(Roles.Admin)]
        [HttpGet]
        public List<string> GetClientsAssociatedWithBusinessUnit(int businessUnitId)
        {
            return _businessUnitBusiness.GetClientsAssociatedWithBusinessUnit(businessUnitId);
        }
    }
}
