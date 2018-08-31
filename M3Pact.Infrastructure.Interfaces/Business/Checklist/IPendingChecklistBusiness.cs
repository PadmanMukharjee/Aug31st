using M3Pact.ViewModel;
using M3Pact.ViewModel.Checklist;
using System;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Business.Checklist
{
    public interface IPendingChecklistBusiness
    {
        List<DateTime> GetWeeklyPendingChecklist(string clientCode);
        List<DateTime> GetMonthlyPendingChecklist(string clientCode);
        List<ClientChecklistResponseViewModel> GetWeeklyPendingChecklistQuestions(string clientCode, DateTime pendingChecklistDate, string checklistType);
        bool SaveOrSubmitChecklistResponse(string clientCode, DateTime pendingDate, bool isSubmit, List<ClientChecklistResponseViewModel> clientChecklistResponse);
        string GetCompletedChecklistsForADateRange(ChecklistDataRequestViewModel checklistDataRequest);
        ValidationViewModel OpenChecklist(ChecklistDataRequestViewModel checklistDataRequest);
        List<ChecklistDataRequestViewModel> GetClientChecklistTypeData(string clientCode);
    }
}
