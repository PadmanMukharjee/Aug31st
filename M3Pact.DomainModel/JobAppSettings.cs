using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class JobAppSettings
    {
        public int JobAppSettingsId { get; set; }
        public string SettingName { get; set; }
        public string SettingValue { get; set; }
        public string Description { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}
