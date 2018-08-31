using M3Pact.BusinessModel.Admin;
using M3Pact.BusinessModel.BusinessModels;
using M3Pact.BusinessModel.Client;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Repository.Admin
{
    public interface IKPIRepository
    {
        List<CheckListType> GetCheckListTypes();
        IEnumerable<Question> GetKPIQuestionBasedOnCheckListType(string checkListTypeCode);
        bool SaveKPIs(KPI kpiViewModel, bool? isQuestionUniversal = null, bool? isOldQuestion = null, bool? existingKPI = null);
        List<M3metricsQuestion> GetM3metricsQuestion();
        List<KPIMeasure> GetMeasureBasedOnClientTypeID(int checkListTypeId);
        List<KPI> GetAllKPIs();
        List<KPI> GetM3MetricsKPIs();
        KPI GetKPIById(int KPIId);
        int GetKPIIdBasedonQuestion(int questionId, string checkListTypeCode);
        List<KPI> GetKPIQuestionsForClient(string clientCode);
        bool SaveKPIForClient(ClientKPISetup clientKPISetup);

        ClientKPISetup GetClientAssignedM3KPIs(string clientCode);
        ClientKPIDetails GetClientAssignedWeeklyMonthlyKPIs(string clientCode);
        KPI GetKpiBaesdOnQuestionCode(string questionCode);
        List<int> GetKpiHeatMapItems();
    }
}
