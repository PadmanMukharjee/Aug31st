using System;

namespace M3Pact.ViewModel.Checklist
{
    public class ChecklistDataRequestViewModel
    {
        public string ClientCode { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ChecklistType { get; set; }
    }
}
