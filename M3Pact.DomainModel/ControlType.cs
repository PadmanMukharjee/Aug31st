using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class ControlType
    {
        public ControlType()
        {
            Attribute = new HashSet<Attribute>();
        }

        public int ControlTypeId { get; set; }
        public string ControlName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<Attribute> Attribute { get; set; }
    }
}
