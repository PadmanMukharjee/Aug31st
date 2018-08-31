using System.Collections.Generic;

namespace M3Pact.ViewModel.Admin
{
    public class SystemViewModel : BusinessBaseViewModel
    {
        public string SystemCode { get; set; }
        public string SystemName { get; set; }
        public string SystemDescription { get; set; }
        public List<string> Clients { get; set; }
    }
}
