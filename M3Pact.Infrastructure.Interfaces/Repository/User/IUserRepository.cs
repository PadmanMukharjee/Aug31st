using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Repository.User
{
    public interface IUserRepository
    {
        List<string> GetUserScreenActions(string screenCode, string role);
        List<M3Pact.DomainModel.DomainModels.Screen> GetScreensOfNavMenuBasedOnRole(string roleCode);
    }

}
