using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class Site
    {
        public Site()
        {
            BusinessUnit = new HashSet<BusinessUnit>();
            Client = new HashSet<Client>();
        }

        public int SiteId { get; set; }
        public string SiteCode { get; set; }
        public string SiteName { get; set; }
        public string SiteDescription { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<BusinessUnit> BusinessUnit { get; set; }
        public ICollection<Client> Client { get; set; }
    }
}
