using Meridian.AuthServer.BusinessModel;
using System.Collections.Generic;

namespace Meridian.AuthServer.BusinessInterfaces
{
    public interface IUserLoginBusiness
    {
        UserLogin FindUserById(string userID);
        UserLogin FindUserByEmail(string email);
        UserLogin ValidateUserCredentials(string userName, string password, string applicationName = null);
    }
}