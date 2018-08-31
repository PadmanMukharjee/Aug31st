using M3Pact.Infrastructure.Interfaces.Business.User;
using M3Pact.ViewModel;
using M3Pact.ViewModel.User;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace M3Pact.WebAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]/[Action]")]
    public class UserController : Controller
    {
        private IUserBusiness _userBusiness;

        public UserController(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }

        /// <summary>
        /// Method to get the screen actions based on user role
        /// </summary>
        /// <param name="screenName"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpGet]
        public List<string> GetUserScreenActions(string screenCode)
        {
            return _userBusiness.GetUserScreenActions(screenCode);
        }

        /// <summary>
        /// To get the left nav menu screens based on role.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MenuItemViewModelList GetScreensOfNavMenuBasedOnRole()
        {
            return _userBusiness.GetScreensOfNavMenuBasedOnRole();
        }

    }
}