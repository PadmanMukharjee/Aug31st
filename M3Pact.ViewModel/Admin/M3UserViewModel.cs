using System.Collections.Generic;

namespace M3Pact.ViewModel.Admin
{
    public class M3UserViewModel : ValidationViewModel
    {
        public M3UserViewModel()
        {
            User = new UserLoginViewModel();
            Clients = new List<string>();
        }
        public UserLoginViewModel User { get; set; }
        public string BusinessUnit { get; set; }
        public string Title { get; set; }
        public string ReportsTo { get; set; }
        public string Site { get; set; }
        public List<string> Clients { get; set; }
        public bool IsUserExist { get; set; }
        public bool IsActiveEmployee { get; set; }
    }
}
