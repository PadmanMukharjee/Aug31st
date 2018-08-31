using M3Pact.BusinessModel.BusinessModels;
using System.Collections.Generic;

namespace M3Pact.BusinessModel.CheckList
{
    public class ViewChecklist
    {
        public ViewChecklist()
        {
            CheckListType = new CheckListType();
            Sites = new List<ChecklistSite>();
            Systems = new List<ChecklistSystem>();
            Clients = new List<ClientChecklist>();
        }

        public CheckListType CheckListType { get; set; }
        public string CheckListName { get; set; }
        public int checklistId { get; set; }
        public List<ChecklistSite> Sites { get; set; }
        public List<ChecklistSystem> Systems { get; set; }
        public List<ClientChecklist> Clients { get; set; }
    }
}
