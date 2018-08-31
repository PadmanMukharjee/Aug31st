using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Interfaces.Business.Admin;
using M3Pact.ViewModel;
using M3Pact.ViewModel.Admin;
using M3Pact.ViewModel.Common;
using M3Pact.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace M3Pact.WebAPI.Controllers.Admin
{
    [Route("api/[controller]/[Action]")]
    public class AllUsersController : Controller
    {
       private IAllUsersBusiness _allUsersBusiness;
        public AllUsersController(IAllUsersBusiness allUserBusiness)
        {
            _allUsersBusiness = allUserBusiness;

        }

        /// <summary>
        /// API call to return the Users
        /// </summary>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager)]
        [HttpGet]
        public List<AllUsersViewModel> GetAllUsersDataAndClientsAssigned(bool isActiveUser = true)
        {
            return _allUsersBusiness.GetAllUsersDataAndClientsAssigned(isActiveUser);
        }

        /// <summary>
        /// API call to return the Employee of given UserID
        /// </summary>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager)]
        [HttpGet]
        public M3UserViewModel GetEmployeeDetails(string employeeID)
        {
            return _allUsersBusiness.GetEmpDetails(employeeID);
        }


        /// <summary>
        /// API call to return the Employee of given UserID
        /// </summary>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager)]
        [HttpGet]
        public UserLoginViewModel GetUserDetails(string userID)
        {
            return _allUsersBusiness.GetUserDetails(userID);
        }


        /// <summary>
        /// API call to return the Employee of given UserID
        /// </summary>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager)]
        [HttpGet]
        public List<AutoFillViewModel> GetEmpNamesForAutoFill(string key)
       {
            return _allUsersBusiness.GetEmpNamesForAutoFill(key);
        }

        /// <summary>
        /// API call to save the Users
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager)]
        public ValidationViewModel SaveClientUser([FromBody]UserLoginViewModel clientUser)
        {

            return _allUsersBusiness.SaveClientUser(clientUser);
        }

        /// <summary>
        /// API call to get the Roles accessible to given roleID
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<RoleViewModel> GetRolesForLoggedInUser()
        {
            return _allUsersBusiness.GetRolesForLoggedInUser();
        }

        /// <summary>
        /// API call to save the Users
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager)]
        public ValidationViewModel SaveM3User([FromBody]M3UserViewModel M3User)
        {

            return _allUsersBusiness.SaveM3User(M3User);

        }

        /// <summary>
        /// saves new for password for the user
        /// </summary>
        /// <param name="resetPassword"></param>
        /// <returns></returns>
        [HttpPost]
        public ValidationViewModel SaveNewPasswordData([FromBody]UserLoginViewModel resetPassword)
        {
            return _allUsersBusiness.ValidateForgotPassword(resetPassword);
        }

        /// <summary>
        /// Validates username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet]
        public ValidationViewModel ValidateUsername(string email)
        {
            return _allUsersBusiness.ValidateUsername(email);
        }

        /// <summary>
        /// Validates username
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ValidationViewModel CheckUser([FromBody]UserLoginViewModel user)
        {
            return _allUsersBusiness.CheckUser(user);
        }

    }
}