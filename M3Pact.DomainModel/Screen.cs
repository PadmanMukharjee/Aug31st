using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class Screen
    {
        public Screen()
        {
            ClientConfigStep = new HashSet<ClientConfigStep>();
            ScreenAction = new HashSet<ScreenAction>();
            ScreenUserRoleMap = new HashSet<ScreenUserRoleMap>();
        }

        public int ScreenId { get; set; }
        public string ScreenName { get; set; }
        public string ScreenCode { get; set; }
        public string ScreenDescription { get; set; }
        public string Icon { get; set; }
        public string ScreenPath { get; set; }
        public bool IsMenuItem { get; set; }
        public int? ParentScreenId { get; set; }
        public int? Displayorder { get; set; }
        public string RecordStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public ICollection<ClientConfigStep> ClientConfigStep { get; set; }
        public ICollection<ScreenAction> ScreenAction { get; set; }
        public ICollection<ScreenUserRoleMap> ScreenUserRoleMap { get; set; }
    }
}
