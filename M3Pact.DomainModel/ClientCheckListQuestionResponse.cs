using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class ClientCheckListQuestionResponse
    {
        public int ClientCheckListQuestionResponseId { get; set; }
        public int ClientCheckListMapId { get; set; }
        public int CheckListAttributeMapId { get; set; }
        public bool? ExpectedResponse { get; set; }
        public string FreeFormResponse { get; set; }
        public int ClientCheckListStatusDetailId { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public CheckListAttributeMap CheckListAttributeMap { get; set; }
        public ClientCheckListMap ClientCheckListMap { get; set; }
        public ClientCheckListStatusDetail ClientCheckListStatusDetail { get; set; }
    }
}
