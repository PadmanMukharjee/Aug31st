using System.Collections.Generic;

namespace M3Pact.ViewModel
{
    public class SiteViewModel
    {
        public string SiteCode { get; set; }
        public string SiteName { get; set; }
        public string SiteDescription { get; set; }
        public string RecordStatus { get; set; }
        public int SiteId { get; set; }
        public List<string> BusinessUnits { get; set; }
        public List<string> Clients { get; set; }
    }
}
