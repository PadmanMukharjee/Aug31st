using M3Pact.BusinessModel.Admin;
using M3Pact.BusinessModel.BusinessModels;
using M3Pact.BusinessModel.Common;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Repository.Admin
{
    public interface IAllUserRepository
    {
        List<AllUsers> GetAllUsersDataAndClientsAssigned(List<int> clientsIDsOfLoggedInUser, bool isActiveUser = false);
        M3UserModel GetM3UserDetails(string EmpID);
        List<AutoFillDTO> GetEmpNamesForAutoFill(string key);
        bool SaveUser(UserLoginDTO userLogin);
        UserLoginDTO GetUserDetails(string userID);
        List<Role> GetRolesForLoggedInUser();
        string GetClientUserSequenceValue();
        string GetUserIDIfEmailAlreadyExists(string email);
        bool UpdatePassword(UserLoginDTO user);
        string ValidateUsername(string username,bool? isM3User=false);
        UserLoginDTO GetUser(string userID, string email);
        List<int> GetClientIDsOfLoggedInUser();
        string GetReportsToOfEmployee(string employeeId);
    }
}
