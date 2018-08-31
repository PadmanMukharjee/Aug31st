using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Interfaces.Business.Admin;
using M3Pact.ViewModel;
using M3Pact.ViewModel.Admin;
using M3Pact.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace M3Pact.WebAPI.Controllers.Admin
{
    [AuthorizationFilter]
    [Route("api/[controller]/[Action]")]
    public class ClientUserController : Controller
    {
        #region Private Properties

        private IClientUserBusiness _clientUserBusiness;

        #endregion Private Properties

        #region Constructor
        public ClientUserController(IClientUserBusiness clientUserBusiness)
        {
            _clientUserBusiness = clientUserBusiness;
        }

        #endregion Constructor

        #region API Methods

        /// <summary>
        /// To get all the Users Data
        /// </summary>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager)]
        [HttpGet]
        public List<AllUsersViewModel> GetAllUsersData()
        {
            return _clientUserBusiness.GetAllUsersData();
        }

        /// <summary>
        /// To Save the Client User association
        /// </summary>
        /// <param name="clientUserMapViewModel"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager)]
        [HttpPost]
        public bool SaveClientUserMap([FromBody]ClientUserMapViewModel clientUserMapViewModel)
        {
            return _clientUserBusiness.SaveClientUsers(clientUserMapViewModel);

        }

        /// <summary>
        /// To get the users asociated with the client
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager)]
        [HttpGet]
        public ClientUserMapViewModel GetUsersForClient(string clientCode)
        {
            return _clientUserBusiness.GetUsersForClient(clientCode);
        }


        /// <summary>
        /// To get All M3 Users
        /// </summary>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager, Roles.User)]
        [HttpGet]
        public List<AllUsersViewModel> GetAllM3Users()
        {
            return _clientUserBusiness.GetAllM3Users();
        }
        #endregion API Methods
    }
}