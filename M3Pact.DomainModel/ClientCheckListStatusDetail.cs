using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class ClientCheckListStatusDetail
    {
        public ClientCheckListStatusDetail()
        {
            ClientCheckListQuestionResponse = new HashSet<ClientCheckListQuestionResponse>();
        }

        public int ClientCheckListStatusDetailId { get; set; }
        public int ClientCheckListMapId { get; set; }
        public DateTime CheckListEffectiveDate { get; set; }
        public string ChecklistStatus { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public ClientCheckListMap ClientCheckListMap { get; set; }
        public ICollection<ClientCheckListQuestionResponse> ClientCheckListQuestionResponse { get; set; }
    }
}
