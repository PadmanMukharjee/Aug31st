using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class System
    {
        public System()
        {
            Client = new HashSet<Client>();
        }

        public int SystemId { get; set; }
        public string SystemCode { get; set; }
        public string SystemName { get; set; }
        public string SystemDescription { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<Client> Client { get; set; }
    }
}
