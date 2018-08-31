using M3Pact.Infrastructure.Common;
using System.Collections.Generic;

namespace M3Pact.ViewModel.Admin
{
    public class CheckListViewModel
    {
        public int ChecklistId { get; set; }
        public string Name { get; set; }
        public List<KeyValueModel> Sites { get; set; }
        public List<KeyValueModel> Systems { get; set; }
        public List<KeyValueModel> ChecklistItems { get; set; }

        public KeyValueModel ChecklistType { get; set; }
    }
}
