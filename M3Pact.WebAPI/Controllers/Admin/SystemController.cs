using M3Pact.Infrastructure.Interfaces.Business.Admin;
using M3Pact.ViewModel.Admin;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace M3Pact.WebAPI.Controllers.Admin
{
    [Produces("application/json")]
    [Route("api/[controller]/[Action]")]
    public class SystemController : Controller
    {
        private ISystemBusiness _systemBusiness;

        public SystemController(ISystemBusiness systemBusiness)
        {
            _systemBusiness = systemBusiness;
        }

        /// <summary>
        /// To get all the System Data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<SystemViewModel> GetAllSystems(bool fromClient = false)
        {
            return _systemBusiness.GetAllSystems(fromClient);
        }

        /// <summary>
        /// To save the Systems
        /// </summary>
        /// <param name="systems"></param>
        /// <returns></returns>
        [HttpPost]

        public bool SaveSystems([FromBody]List<SystemViewModel> systems)
        {
            if (systems != null && systems.Count > 0)
            {
                return _systemBusiness.SaveSystems(systems);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// To get clients associated with system
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        [HttpGet]
        public List<string> getClientsAssociatedWithSystem(int systemId)
        {
            return _systemBusiness.GetClientsAssociatedWithSystem(systemId);
        }

        /// <summary>
        /// Activating or InActivating systems
        /// </summary>
        /// <param name="systemViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public bool ActivateOrInactivateSystems([FromBody]SystemViewModel systemViewModel)
        {
            return _systemBusiness.ActivateOrInactivateSystem(systemViewModel);
        }
    }
}