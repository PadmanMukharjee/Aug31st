using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class JobRun
    {
        public int JobRunId { get; set; }
        public int JobProcessGroupId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int JobStatusId { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public JobProcessGroup JobProcessGroup { get; set; }
        public JobStatus JobStatus { get; set; }
    }
}
