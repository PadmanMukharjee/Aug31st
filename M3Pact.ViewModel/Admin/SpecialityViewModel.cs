using System.Collections.Generic;

namespace M3Pact.ViewModel
{
    public class SpecialityViewModel : BusinessBaseViewModel
    {
        public string SpecialityCode { get; set; }
        public string SpecialityName { get; set; }
        public string SpecialityDescription { get; set; }     
        public List<string> Clients { get; set; }
    }
}
