using System;

namespace M3Pact.BusinessModel.Admin
{
    public class Role
    {
        public string RoleCode { get; set; }
        public string RoleDesc { get; set; }
        public string RecordStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
