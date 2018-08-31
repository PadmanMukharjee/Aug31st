using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class RoleAction
    {
        public int RoleActionId { get; set; }
        public int RoleId { get; set; }
        public int ScreenActionId { get; set; }
        public string RecordStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public Roles Role { get; set; }
        public ScreenAction ScreenAction { get; set; }
    }
}
