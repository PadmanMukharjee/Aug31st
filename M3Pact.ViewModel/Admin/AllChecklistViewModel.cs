using System.Collections.Generic;

namespace M3Pact.ViewModel.Admin
{
    public class AllChecklistViewModel
    {
        public AllChecklistViewModel()
        {
            SelectedClients = new List<ClientViewModel>();
            SelectedSystems = new List<SystemViewModel>();
            SelectedSites = new List<SiteViewModel>();
        }

        public string Name { get; set;}
        public string Type { get; set; }
        public int Id { get; set; }
        public List<ClientViewModel> SelectedClients { get; set; }
        public List<SystemViewModel> SelectedSystems { get; set; }
        public List<SiteViewModel> SelectedSites { get; set; }
    }
}
