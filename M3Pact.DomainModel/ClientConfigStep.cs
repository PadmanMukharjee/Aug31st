using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class ClientConfigStep
    {
        public ClientConfigStep()
        {
            ClientConfigStepDetail = new HashSet<ClientConfigStepDetail>();
        }

        public int ClientConfigStepId { get; set; }
        public string ClientConfigStepName { get; set; }
        public string ClientConfigStepDescription { get; set; }
        public string ScreenCode { get; set; }
        public int? DisplayOrder { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public Screen ScreenCodeNavigation { get; set; }
        public ICollection<ClientConfigStepDetail> ClientConfigStepDetail { get; set; }
    }
}
