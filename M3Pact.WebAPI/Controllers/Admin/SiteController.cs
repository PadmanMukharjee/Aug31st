using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Interfaces.Business.Admin;
using M3Pact.ViewModel;
using M3Pact.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace M3Pact.WebAPI.Controllers.Admin
{
    [AuthorizationFilter]
    [Route("api/[controller]/[Action]")]
    public class SiteController : Controller
    {
        private ISiteBusiness _siteBusiness;

        public SiteController(ISiteBusiness siteBusiness)
        {
            _siteBusiness = siteBusiness;

        }

        /// <summary>
        /// API call to return the Sites
        /// </summary>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive)]
        [HttpGet]
        public List<SiteViewModel> GetSites(bool isActiveSites)
        {
            return _siteBusiness.GetSites(isActiveSites);
        }

        /// <summary>
        /// To return sites in the dropdown
        /// </summary>
        /// <param name="isActiveSites"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager, Roles.User)]
        [HttpGet]
        public List<SiteViewModel> GetSitesDropdown(bool isActiveSites)
        {
            return _siteBusiness.GetSites(isActiveSites);
        }

        /// <summary>
        /// API call to save the Sites
        /// </summary>
        /// <param name="sites"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizationFilter(Roles.Admin)]
        public bool SaveSites([FromBody]List<SiteViewModel> sites)
        {
            if (sites != null && sites.Count > 0)
            {
                return _siteBusiness.SaveSites(sites);
            }
            else
            {
                return false;
            }
        }

        [AuthorizationFilter(Roles.Admin)]
        [HttpPost]
        public List<string> GetClientsAssociatedWithSite([FromBody] SiteViewModel site)
        {
            if (site != null)
            {
                return _siteBusiness.GetClientsAssociatedWithSite(site.SiteId);
            }
            else
            {
                return null;
            }
        }

        [AuthorizationFilter(Roles.Admin)]
        [HttpPost]
        public bool ActivateOrInactivateSites([FromBody]SiteViewModel siteViewModel)
        {
            return _siteBusiness.ActivateOrInactivateSites(siteViewModel);
        }
    }
}
