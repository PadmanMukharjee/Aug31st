using System;
using System.Collections.Generic;
using System.Text;

namespace M3Pact.ViewModel.HeatMap
{
    public class ClientsHeatMapRequestViewModel
    {
        public string BusinessUnitCode { get; set; }
        public string SystemCode { get; set; }
        public string SpecialtyCode { get; set; }
        public string EmployeeID { get; set; }
    }
}