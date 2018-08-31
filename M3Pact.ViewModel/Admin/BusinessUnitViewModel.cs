using System.Collections.Generic;

namespace M3Pact.ViewModel
{
    public class BusinessUnitViewModel : BusinessBaseViewModel
    {
        public BusinessUnitViewModel()
        {
            Site = new SiteViewModel();
        }
        public string BusinessUnitCode { get; set; }
        public string BusinessUnitName { get; set; }
        public string BusinessUnitDescription { get; set; }
        public SiteViewModel Site { get; set; }
        public List<string> Clients { get; set; }
    }
}
