
using M3Pact.ViewModel;
using M3Pact.ViewModel.Admin;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Business.Admin
{
    public interface IClientUserBusiness
    {
        List<AllUsersViewModel> GetAllUsersData();
        List<AllUsersViewModel> GetAllM3Users();
        bool SaveClientUsers(ClientUserMapViewModel clientUserMapViewModel);
        ClientUserMapViewModel GetUsersForClient(string clientCode);


    }
}
