using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class CheckListType
    {
        public CheckListType()
        {
            CheckList = new HashSet<CheckList>();
            DeviatedClientKpi = new HashSet<DeviatedClientKpi>();
            Kpi = new HashSet<Kpi>();
            Kpimeasure = new HashSet<Kpimeasure>();
            Question = new HashSet<Question>();
        }

        public int CheckListTypeId { get; set; }
        public string CheckListTypeCode { get; set; }
        public string CheckListTypeName { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<CheckList> CheckList { get; set; }
        public ICollection<DeviatedClientKpi> DeviatedClientKpi { get; set; }
        public ICollection<Kpi> Kpi { get; set; }
        public ICollection<Kpimeasure> Kpimeasure { get; set; }
        public ICollection<Question> Question { get; set; }
    }
}
