using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class JobProcessGroup
    {
        public JobProcessGroup()
        {
            JobRun = new HashSet<JobRun>();
        }

        public int JobProcessGroupId { get; set; }
        public string ProcessGroupCode { get; set; }
        public string ProcessGroupName { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<JobRun> JobRun { get; set; }
    }
}
