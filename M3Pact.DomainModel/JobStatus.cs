using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class JobStatus
    {
        public JobStatus()
        {
            JobRun = new HashSet<JobRun>();
        }

        public int JobStatusId { get; set; }
        public string JobStatusCode { get; set; }
        public string JobStatusName { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<JobRun> JobRun { get; set; }
    }
}
