using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class Apimethod
    {
        public Apimethod()
        {
            ApimethodsRoleMap = new HashSet<ApimethodsRoleMap>();
        }

        public int ApimethodId { get; set; }
        public string ApicontrollerName { get; set; }
        public string Apimethod1 { get; set; }
        public string RecordStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public ICollection<ApimethodsRoleMap> ApimethodsRoleMap { get; set; }
    }
}
