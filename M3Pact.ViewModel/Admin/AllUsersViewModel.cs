using System.Collections.Generic;

namespace M3Pact.ViewModel
{
    public class AllUsersViewModel
    {
        public AllUsersViewModel()
        {
            SelectedClients = new List<ClientViewModel>();
        }
        public int ID { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string BusinessUnit { get; set; }
        public string ReportsTo { get; set; }
        public string Site { get; set; }
        public bool? IsMeridianUser { get; set; }
        public string Role { get; set; }
        public string RecordStatus { get; set; }
        public List<ClientViewModel> SelectedClients { get; set; }

    }
}
