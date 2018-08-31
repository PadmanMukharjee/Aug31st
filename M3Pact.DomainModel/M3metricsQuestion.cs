using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class M3metricsQuestion
    {
        public int M3metricsQuestionId { get; set; }
        public string M3metricsQuestionCode { get; set; }
        public string M3metricsQuestionText { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string M3metricsUnit { get; set; }
    }
}
