using M3Pact.ViewModel;
using M3Pact.ViewModel.User;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Business.User
{
    public interface IUserBusiness
    {
        List<string> GetUserScreenActions(string screenCode);
        MenuItemViewModelList GetScreensOfNavMenuBasedOnRole();
    }
}
