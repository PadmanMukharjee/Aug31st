using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class CheckList
    {
        public CheckList()
        {
            CheckListAttributeMap = new HashSet<CheckListAttributeMap>();
            ClientCheckListMap = new HashSet<ClientCheckListMap>();
            ClientMonthlyCheckList = new HashSet<Client>();
            ClientWeeklyCheckList = new HashSet<Client>();
        }

        public int CheckListId { get; set; }
        public int CheckListTypeId { get; set; }
        public string CheckListName { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public CheckListType CheckListType { get; set; }
        public ICollection<CheckListAttributeMap> CheckListAttributeMap { get; set; }
        public ICollection<ClientCheckListMap> ClientCheckListMap { get; set; }
        public ICollection<Client> ClientMonthlyCheckList { get; set; }
        public ICollection<Client> ClientWeeklyCheckList { get; set; }
    }
}
