using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class CheckListAttribute
    {
        public CheckListAttribute()
        {
            CheckListAttributeMap = new HashSet<CheckListAttributeMap>();
        }

        public int CheckListAttributeId { get; set; }
        public string AttributeCode { get; set; }
        public string AttributeName { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<CheckListAttributeMap> CheckListAttributeMap { get; set; }
    }
}
