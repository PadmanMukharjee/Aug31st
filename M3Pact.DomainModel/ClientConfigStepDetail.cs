using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class ClientConfigStepDetail
    {
        public int ClientConfigStepDetailId { get; set; }
        public int ClientId { get; set; }
        public int ClientConfigStepId { get; set; }
        public int ClientConfigStepStatusId { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public Client Client { get; set; }
        public ClientConfigStep ClientConfigStep { get; set; }
        public ClientConfigStepStatus ClientConfigStepStatus { get; set; }
    }
}
