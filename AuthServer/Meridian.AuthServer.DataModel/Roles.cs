using System;
using System.Collections.Generic;

namespace Meridian.AuthServer.DataModel
{
    public partial class Roles
    {
        public Roles()
        {
            UserLogin = new HashSet<UserLogin>();
        }

        public int RoleId { get; set; }
        public string RoleCode { get; set; }
        public string RoleDesc { get; set; }
        public string RecordStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        public ICollection<UserLogin> UserLogin { get; set; }
    }
}
