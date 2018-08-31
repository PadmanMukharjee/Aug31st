using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class ScreenAction
    {
        public ScreenAction()
        {
            RoleAction = new HashSet<RoleAction>();
        }

        public int ScreenActionId { get; set; }
        public int ScreenId { get; set; }
        public string ActionName { get; set; }
        public string RecordStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public Screen Screen { get; set; }
        public ICollection<RoleAction> RoleAction { get; set; }
    }
}
