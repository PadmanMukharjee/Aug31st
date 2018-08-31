using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class Kpimeasure
    {
        public Kpimeasure()
        {
            Kpi = new HashSet<Kpi>();
        }

        public int KpimeasureId { get; set; }
        public int CheckListTypeId { get; set; }
        public string Measure { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public CheckListType CheckListType { get; set; }
        public ICollection<Kpi> Kpi { get; set; }
    }
}
