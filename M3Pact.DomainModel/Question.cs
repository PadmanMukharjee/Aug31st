using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class Question
    {
        public Question()
        {
            CheckListQuestionMap = new HashSet<CheckListQuestionMap>();
        }

        public int QuestionId { get; set; }
        public string QuestionCode { get; set; }
        public string QuestionText { get; set; }
        public bool ExpectedRespone { get; set; }
        public bool IsKpi { get; set; }
        public bool IsUniversal { get; set; }
        public bool RequireFreeform { get; set; }
        public int? CheckListTypeId { get; set; }
        public string RecordStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public CheckListType CheckListType { get; set; }
        public ICollection<CheckListQuestionMap> CheckListQuestionMap { get; set; }
    }
}
