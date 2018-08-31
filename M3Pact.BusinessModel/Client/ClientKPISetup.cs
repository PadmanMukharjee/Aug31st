using M3Pact.BusinessModel.Admin;
using System.Collections.Generic;

namespace M3Pact.BusinessModel.Client
{
    public class ClientKPISetup
    {
        public ClientKPISetup()
        {
            KPIQuestions = new List<KPISetup>();
        }
        public string ClientCode { get; set; }
        public List<KPISetup> KPIQuestions { get; set; }

    }

    public class KPISetup
    {
        public KPISetup()
        {
            Kpi = new KPI();
            SendTo = new List<AllUsers>();
        }

        public int ClientKPIMapId { get; set; }
        public KPI Kpi { get; set; }
        public string ClientStandard { get; set; }
        public string AlertLevel { get; set; }
        public string AlertValue { get; set; }
        public bool? Sla { get; set; }
        public List<AllUsers> SendTo { get; set; }
    }
}
