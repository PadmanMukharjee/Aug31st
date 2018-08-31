using System.Collections.Generic;

namespace M3Pact.BusinessModel
{
    public  class ManuallyEditedTargetsBusinessModel
    {
      public List<ChargesBusinessModel> charges { get; set; }
        public List<PaymentsBusinessModel> payments { get; set; }
        public List<RevenueBusinessModel> revenue { get; set; }
        public string clientCode { get; set; }
        public int year { get; set; }
    }
}
