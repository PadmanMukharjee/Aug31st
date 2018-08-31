using System;

namespace M3Pact.ViewModel.Client
{
    public class ClientHistoryViewModel
    {
        public string ModifiedOrAddedBy { get; set; }
        public DateTime ModifiedOrAddedDate { get; set; }
        public string UpdatedOrAddedProperty { get; set; }
        public string Action { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}
