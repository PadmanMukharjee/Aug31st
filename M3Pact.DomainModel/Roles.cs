using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class Roles
    {
        public Roles()
        {
            RoleAction = new HashSet<RoleAction>();
            ScreenUserRoleMap = new HashSet<ScreenUserRoleMap>();
            UserLogin = new HashSet<UserLogin>();
        }

        public int RoleId { get; set; }
        public string RoleCode { get; set; }
        public string RoleDesc { get; set; }
        public string RecordStatus { get; set; }
        public int? Level { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        public ICollection<RoleAction> RoleAction { get; set; }
        public ICollection<ScreenUserRoleMap> ScreenUserRoleMap { get; set; }
        public ICollection<UserLogin> UserLogin { get; set; }
    }
}
