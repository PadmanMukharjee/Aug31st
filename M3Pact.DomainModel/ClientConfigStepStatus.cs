using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class ClientConfigStepStatus
    {
        public ClientConfigStepStatus()
        {
            ClientConfigStepDetail = new HashSet<ClientConfigStepDetail>();
        }

        public int ClientConfigStepStatusId { get; set; }
        public string ClientConfigStepStatusName { get; set; }
        public string ClientConfigStepStatusDescription { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<ClientConfigStepDetail> ClientConfigStepDetail { get; set; }
    }
}
