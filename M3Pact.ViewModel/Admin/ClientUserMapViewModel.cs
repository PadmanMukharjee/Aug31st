using System.Collections.Generic;

namespace M3Pact.ViewModel.Admin
{
    public class ClientUserMapViewModel : ValidationViewModel
    {
        public ClientUserMapViewModel()
        {
            ClientUsers = new List<AllUsersViewModel>();
        }
        public string ClientCode { get; set; }
        public List<AllUsersViewModel> ClientUsers { get; set; }
    }
}
