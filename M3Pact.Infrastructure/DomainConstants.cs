using System;

namespace M3Pact.Infrastructure
{
    public class DomainConstants
    {
        public const string RecordStatusActive = "A";
        public const string RecordStatusPartial = "P";
        public const string RecordStatusInactive = "I";
        public const string RecordStatusDelete = "D";
        public const string Client = "Client";
        public const string Payer = "Payer";
        public const string ClientPayer = "ClientPayer";
        public const string Admin = "Admin";
        public const string Role = "Role";

        public const string CheckListSiteAttributeCode = "SITE";
        public const string CheckListSystemAttributeCode = "SYS";
        public const string CheckListQuestionAttributeCode = "QUE";
        //Included this not to break pending checklist build
        public const int CheckListQuestionAttributeId = 3;
        public const int CheckListTypeId = 3;
        public const string Active = "Active";
        public const string InActive = "Inactive";
        public const string PartiallyCompleted = "Partially Completed";
        public const string Pending = "P";
        public const string Completed = "C";
        public const string ReOpened = "R";
        public const string Deleted = "D";

        public const string WEEK = "WEEK";
        public const int WeekChecklistTypeID = 1;
        public const string Weekly = "Weekly";
        public const string MONTH = "MONTH";
        public const int MonthChecklistTypeID = 2;
        public const string Monthly = "Monthly";
        public const string Daily = "Daily";
        public const string M3Metrics = "M3Metrics";
        public const string M3 = "M3";
        public const string M3DAILY = "M3DAILY";
        public const string M3WEEKLY = "M3WEEKLY";
        public const string M3MONTHLY = "M3MONTHLY";

        public static DateTime MinDate = new DateTime(1900, 01, 01);
        public const string ChecklistPending = "P";
        public const string ChecklistCompleted = "C";
        public const string ChecklistReopen = "R";

        public const int ClientStepInProgress = 1;
        public const int ClientStepCompleted = 2;

        public const int HeatMapScore = 20;
        public const string Update = "update";

        public const string BillingManager = "Billing";
        public const string RelationshipManager = "Relation";
        public const string AlertRecepient = "AlertRecepient";

        public const string Alert = "Alert";
        public const string Escalation = "Escalation";
        public const string AlertMailSubject = "M3Pact ALERT: Deviated KPIs on";
        public const string EscalatedAlertMailSubject = "M3Pact ESCALATED ALERT: Deviated KPIs";
        public const string m3PactLink = "http://10.101.4.37";

        public const string M3PactConnection = "M3PactConnection";
        public const string MeridianConnection = "MeridianConnection";

        // Stored Procedure Names
        public const string GetDepositLog = "usp_GetDepositLogForClient";
        public const string usp_GetDepositLogsForClientDate = "usp_GetDepositLogsForClientDate";
        public const string GetProjectedCashOfPreviousWeek = "usp_GetProjectedCashBasedOnLastNumberOfWeeks";
        public const string GetProjectedCashOfLastWorkingDays = "usp_GetProjectedCashOfLastWorkingDays";
        public const string GetKPIQuestionBasedOnCheckListType = "usp_GetKPIQuestionsBasedonCheckListType";
        public const string GetDepositLogStartDateAndNumberOfDepositDates = "usp_GetDepositLogStartDateAndNumberOfDepositDates";
        public const string GetAllChecklist = "usp_GetViewAllChecklists";
        public const string GetNumberOfDepositWeeksForClient = "usp_GetNumberOfDepositWeeksForClient";
        public const string GetkPIQuestionsForClient  = "usp_GetKPIQuestionsForClient";
        public const string GetAssignedM3KPIsForClient = "usp_GetAssignedM3KPIsForClient";
        public const string AddUniversalKPIsToNewClient = "usp_AddUniversalKPIsToNewClient";
        public const string questionGetNextValueSequence = "Question_Get_Next_Value_Sequence";
        public const string GetClientAssignedWeeklyMonthlyKPIs = "usp_GetClientAssignedWeeklyMonthlyKPIs";
        public const string OpenOrCloseMonth = "usp_CloseOrReopenMonthOfYearForClient";
        public const string GetMonthDepositsOfAYear = "usp_GetMonthDepositsOfAYearForClient";

        public const string UpdateClientKPIEffectiveDateOnChecklistUpdate = "usp_UpdateClientKPIEffectiveDateOnChecklistUpdate";
        public const string ClientUserGetNextValueSequence = "clientUser_Get_Next_Value_Sequence";
        public const string EscalateAlertJob = "usp_Escalate_Mail_Daily_Job";
        public const string GetActivePayersToAssign = "usp_Payer_Get_Active_Unassigned_To_Client";
        public const string GetHeatMapForApplicableClients = "usp_Get_HeatMap_For_Clients";
        public const string GetClientStepStatus = "usp_ClientConfigStep_Get_Step_Status_Details";
        public const string GetWeeklyPendingChecklist = "GetWeeklyPendingChecklist";
        public const string GetMonthlyPendingChecklist = "GetMonthlyPendingChecklist";
        public const string GetPendingChecklists = "GetPendingChecklistQuestions";
        public const string GetAllClientsData = "usp_GetAllClientsData";
        public const string GetToDoListActions = "usp_GetToDoListActions";
        public const string GetClientHistoryData = "usp_GetClientHistoryData";



        // attribute constants

        public const string LastEnteredBusinessDays = "LastEnteredBusinessDays";
        public const string LastEnteredWeeks = "LastEnteredWeeks";
        public const string CreatedDate = "CreatedDate";
        public const string CreatedBy = "CreatedBy";
        public const string ClientFirstStepCreatedDate = "ClientFirstStepCreatedDate";

    }
}
