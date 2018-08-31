using M3Pact.BusinessModel.Admin;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Repository.Admin
{
    public interface IClientUserRepository
    {
        List<AllUsers> GetAllUsersData();
        List<AllUsers> GetAllM3Users();
        bool SaveClientUsers(ClientUserMap clientUserMap);
        ClientUserMap GetUsersForClient(string clientCode);
    }
}
