using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class BusinessUnit
    {
        public BusinessUnit()
        {
            Client = new HashSet<Client>();
        }

        public int BusinessUnitId { get; set; }
        public string BusinessUnitCode { get; set; }
        public string BusinessUnitName { get; set; }
        public string BusinessUnitDescription { get; set; }
        public int? SiteId { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public Site Site { get; set; }
        public ICollection<Client> Client { get; set; }
    }
}
