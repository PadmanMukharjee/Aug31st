using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class ClientCheckListMap
    {
        public ClientCheckListMap()
        {
            ClientCheckListQuestionResponse = new HashSet<ClientCheckListQuestionResponse>();
            ClientCheckListStatusDetail = new HashSet<ClientCheckListStatusDetail>();
        }

        public int ClientCheckListMapId { get; set; }
        public int ClientId { get; set; }
        public int CheckListId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public CheckList CheckList { get; set; }
        public Client Client { get; set; }
        public ICollection<ClientCheckListQuestionResponse> ClientCheckListQuestionResponse { get; set; }
        public ICollection<ClientCheckListStatusDetail> ClientCheckListStatusDetail { get; set; }
    }
}
