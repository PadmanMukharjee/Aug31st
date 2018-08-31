using Meridian.AuthServer.BusinessModel;

namespace Meridian.AuthServer.RepositoryInterfaces
{
    public interface IUserLoginRepository
    {
        UserLogin GetUserById(string id);
        UserLogin GetUserByEmail(string email);
    }
}