using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class CheckListQuestionMap
    {
        public int CheckListQuestionMapId { get; set; }
        public int? CheckListId { get; set; }
        public int? QuestionId { get; set; }
        public string QuestionResponse { get; set; }
        public string FreeFormText { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public CheckList CheckList { get; set; }
        public Question Question { get; set; }
    }
}
