using M3Pact.ViewModel.Admin;
using System;
using System.Collections.Generic;

namespace M3Pact.ViewModel.Client
{
    public class ClientKPISetupViewModel
    {
        public ClientKPISetupViewModel()
        {
            KpiQuestions = new List<KPISetupViewModel>();
        }

        public string ClientCode { get; set; }
        public List<KPISetupViewModel> KpiQuestions { get; set; }

    }
    public class KPIQuestionViewModel
    {
        public KPIQuestionViewModel()
        {
            ChecklistTypeViewModel = new CheckListTypeViewModel();
            M3MeasureViewModel = new MeasureViewModel();
        }

        public int KpiId { get; set; }
        public string KpiDescription { get; set; }
        public CheckListTypeViewModel ChecklistTypeViewModel { get; set; }
        public string CompanyStandard { get; set; }
        public bool? IsUniversal { get; set; }
        public DateTime? EndDate { get; set; }
        public MeasureViewModel M3MeasureViewModel { get; set; }
    }

    public class KPISetupViewModel
    {
        public KPISetupViewModel()
        {
            Kpi = new KPIQuestionViewModel();
            SendTo = new List<AllUsersViewModel>();
        }

        public int clientKPIMapId{ get; set; }
        public KPIQuestionViewModel Kpi { get; set; }
        public string ClientStandard { get; set; }
        public string AlertLevel { get; set; }
        public string AlertValue { get; set; }
        public bool? Sla { get; set; }
        public string Info { get; set; }
        public bool FutureRemoverOrUniversal { get; set; }
        public List<AllUsersViewModel> SendTo { get; set; }
    }

    public class ClientData
    {
        public string ClientCode { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
    }

}
