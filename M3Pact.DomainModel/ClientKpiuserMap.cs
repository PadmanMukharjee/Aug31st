using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class ClientKpiuserMap
    {
        public int ClientKpiuserMapId { get; set; }
        public int ClientKpimapId { get; set; }
        public int UserId { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public ClientKpimap ClientKpimap { get; set; }
        public UserLogin User { get; set; }
    }
}
