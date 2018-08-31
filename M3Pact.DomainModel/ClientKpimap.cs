using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class ClientKpimap
    {
        public ClientKpimap()
        {
            ClientKpiuserMap = new HashSet<ClientKpiuserMap>();
        }

        public int ClientKpimapId { get; set; }
        public int ClientId { get; set; }
        public int Kpiid { get; set; }
        public string ClientStandard { get; set; }
        public bool? IsSla { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public Client Client { get; set; }
        public Kpi Kpi { get; set; }
        public ICollection<ClientKpiuserMap> ClientKpiuserMap { get; set; }
    }
}
