using M3Pact.BusinessModel;
using M3Pact.BusinessModel.CheckList;
using M3Pact.DomainModel.DomainModels;
using System;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Repository.Checklist
{
    public interface IPendingChecklistRepository
    {
        List<DateTime> GetWeeklyPendingChecklist(string clientCode);
        List<DateTime> GetMonthlyPendingChecklist(string clientCode);
        List<ClientChecklistResponseBusinessModel> GetWeeklyPendingChecklistQuestions(string clientCode, DateTime pendingChecklistDate, string checklistType);
        bool SaveOrSubmitChecklistResponse(string clientCode, DateTime pendingDate, bool isSubmit, List<ClientChecklistResponseBusinessModel> clientChecklistResponse);
        List<ChecklistDataResponse> GetChecklistsForADateRange(ChecklistDataRequest request);
        bool OpenChecklist(ChecklistDataRequest checklistDataRequest);
        List<ChecklistDataRequest> GetClientChecklistTypeData(string clientCode);
        List<ClientHeatMapItemScore> GetHeatMapScoresToUpdate(DateTime submittedDate, int clientId, int checklistTypeId);
        List<HeatMapItem> GetHeatMapWithType(int checklistTypeId);
        List<ClientHeatMapItemScore> MapHeatMapItemScores(int clientId, DateTime pendingDate, List<ClientChecklistResponseBusinessModel> clientChecklistResponse, List<HeatMapItem> heatMapItems);
        int? GetScore(int ClientHeatMapRiskId, string type, string action = "update");
        DateTime? GetLastSubmittedChecklist(int clientId, int typeId);
    }
}
