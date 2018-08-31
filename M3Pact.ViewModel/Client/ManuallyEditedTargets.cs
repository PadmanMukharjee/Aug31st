using System.Collections.Generic;

namespace M3Pact.ViewModel
{
    public  class ManuallyEditedTargets
    {
      public  List<Charges> charges { get; set; }
        public List<Payments> payments { get; set; }
        public List<Revenue> revenue { get; set; }
        public string clientCode { get; set; }
        public int year { get; set; }
    }
}
