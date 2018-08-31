using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class ScreenUserRoleMap
    {
        public int ScreenUserRoleId { get; set; }
        public int ScreenId { get; set; }
        public int RoleId { get; set; }
        public string RecordStatus { get; set; }
        public bool? CanEdit { get; set; }
        public string DisplayScreenName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public Roles Role { get; set; }
        public Screen Screen { get; set; }
    }
}
