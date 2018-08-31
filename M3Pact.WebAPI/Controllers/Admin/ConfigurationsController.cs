using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using M3Pact.Infrastructure.Interfaces.Business.Admin;
using M3Pact.ViewModel.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace M3Pact.WebAPI.Controllers.Admin
{
    [Route("api/[controller]/[Action]")]
    public class ConfigurationsController : Controller
    {
        #region properties
        private IConfigurationsBusiness _configurationsBusiness;
        #endregion properties

        #region constructor
        public ConfigurationsController(IConfigurationsBusiness configurationsBusiness)
        {
            _configurationsBusiness = configurationsBusiness;
        }
        #endregion constructor
        /// <summary>
        /// To get the Admin Configs
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<AttributesViewModel> GetAttributesForConfig()
        {
            return _configurationsBusiness.GetAttributesForConfig();
        }

        /// <summary>
        /// to save the updated value of configuration
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        [HttpPost]
        public bool SaveAttributeValue([FromBody] AttributesViewModel attribute)
        {
            return _configurationsBusiness.SaveAttributeValue(attribute);
        }
    }
}