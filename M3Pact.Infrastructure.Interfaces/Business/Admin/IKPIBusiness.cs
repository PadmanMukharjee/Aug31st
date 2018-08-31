using M3Pact.ViewModel.Admin;
using M3Pact.ViewModel.Client;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Business.Admin
{
    public interface IKPIBusiness
    {
        List<CheckListTypeViewModel> GetCheckListTypes();
        List<MeasureViewModel> GetKPIQuestionBasedOnCheckListType(string checkListTypeCode);
        bool SaveKPIDetails(KPIViewModel kpiViewModel);
        List<KPIMeasureViewModel> GetMeasureBasedOnClientTypeID(int checkListTypeId);
        List<KPIViewModel> GetAllKPIs();
        KPIViewModel GetKPIById(int KPIId);
        int GetKPIIdBasedonQuestion(int questionId, string checkListTypeCode);
        List<KPIQuestionViewModel> GetKPIQuestionsForClient(string clientCode);
        bool SaveKPIForClient(ClientKPISetupViewModel clientKPI);
        ClientKPISetupViewModel GetClientAssignedKPIs(string clientCode);
        List<KPIViewModel> GetAllM3MetricsKPIs();
        List<int> GetKpiHeatMapItems();
    }
}
