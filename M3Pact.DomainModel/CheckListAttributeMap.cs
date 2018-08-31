using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class CheckListAttributeMap
    {
        public CheckListAttributeMap()
        {
            ClientCheckListQuestionResponse = new HashSet<ClientCheckListQuestionResponse>();
        }

        public int CheckListAttributeMapId { get; set; }
        public int CheckListId { get; set; }
        public int CheckListAttributeId { get; set; }
        public string CheckListAttributeValueId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public CheckList CheckList { get; set; }
        public CheckListAttribute CheckListAttribute { get; set; }
        public ICollection<ClientCheckListQuestionResponse> ClientCheckListQuestionResponse { get; set; }
    }
}
