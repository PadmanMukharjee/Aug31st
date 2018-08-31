using M3Pact.ViewModel;
using M3Pact.ViewModel.Admin;
using M3Pact.ViewModel.Common;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Business.Admin
{
    public interface IAllUsersBusiness
    {
        List<AllUsersViewModel> GetAllUsersDataAndClientsAssigned(bool isActiveUser = false);
        M3UserViewModel GetEmpDetails(string userID);
        List<AutoFillViewModel> GetEmpNamesForAutoFill(string key);
        UserLoginViewModel GetUserDetails(string userID);
        ValidationViewModel SaveClientUser(UserLoginViewModel clientUser);
        List<RoleViewModel> GetRolesForLoggedInUser();
        ValidationViewModel SaveM3User(M3UserViewModel m3User);
        ValidationViewModel UpdatePassword(UserLoginViewModel user);
        ValidationViewModel ValidateForgotPassword(UserLoginViewModel resetPassword);
        ValidationViewModel CheckUser(UserLoginViewModel user);
        ValidationViewModel ValidateUsername(string email);
    }
}
