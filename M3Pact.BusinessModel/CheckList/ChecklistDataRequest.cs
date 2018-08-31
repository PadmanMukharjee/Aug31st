using System;

namespace M3Pact.BusinessModel.CheckList
{
    public class ChecklistDataRequest
    {
        public string ClientCode { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ChecklistType { get; set; }
    }
}
