using M3Pact.Infrastructure.Common;
using System.Collections.Generic;

namespace M3Pact.BusinessModel.BusinessModels
{
    public class CheckList
    {
        public int CheckListId { get; set; }
        public string CheckListName { get; set; }
        public List<KeyValueModel> Sites { get; set; }
        public List<KeyValueModel> Systems { get; set; }
        public List<KeyValueModel> Questions { get; set; }

        public KeyValueModel CheckListType { get; set; }
    }
}
