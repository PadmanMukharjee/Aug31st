using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class QuestionType
    {
        public QuestionType()
        {
            Question = new HashSet<Question>();
        }

        public int QuestionTypeId { get; set; }
        public string QuestionTypeCode { get; set; }
        public string QuestionTypeName { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<Question> Question { get; set; }
    }
}
