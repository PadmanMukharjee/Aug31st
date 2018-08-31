using M3Pact.BusinessModel.BusinessModels;
using M3Pact.BusinessModel.DepositLog;
using M3Pact.ViewModel;
using System;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Repository
{
    public interface IDepositLogRepository
    {
        DepositLogClientDataAmount GetDepositLogs(string clientCode, int month, int year);
        bool SaveDepositLogs(List<BusinessModel.BusinessModels.ClientPayer> clientDepositLogs, string clientCode, int savedLastNumberOfBusinessDays);
        List<Payer> GetPayerDetailsForClient(string clientCode);
        List<BusinessModel.BusinessModels.ClientPayer> GetDepositLogsForClientDate(string clientCode, DateTime depositDate);
        DepositLogMTD GetDepositLogMTD(string clientCode, int month, int year);
        DepositLogMonthlyTarget GetDepositLogMonthlyTargets(string clientCode, int month, int year);
        int? GetTotalBusinessDaysInaMonthOfAnYear(int year, int month);
        DepositLogSimpleBusinessDays GetProjectedCashFromSimpleBusinessDays(string clientCode, int numberOfDays, int month, int year, DateTime? startDate, DateTime endDate);
        List<DateTime> GetHolidayDates(DateTime startDate, DateTime endDate);
        List<DepositLogWeekDays> GetProjectedCashOfPreviousWeeks(string clientCode, int month, int year, int numberOfWeeks, DateTime endDate);
        long? GetClientPaymentForMonth(string clientCode, int month, int year);
        decimal? GetDepositLogOfCurrentMonth(string clientCode, int month, int year);
        decimal? GetDepositLogAmountFromGivenDatesExcludingWeekEnds(string clientCode, DateTime? startDate, DateTime endDate);
        decimal? GetDepositLogAmountFromGivenDatesIncludingWeekEnds(string clientCode, int year, int month, DateTime startDate, DateTime endDate);
        List<DepositLogProjectedCash> GetProjectedCashOfLastWorkingDays(string clientCode, int month, int year, int numberOfLastWorkingDays, DateTime? startDate, DateTime endDate);
        int GetNumberOfBusinessDaysFromStartDateAndEndDate(DateTime? startDate, DateTime endDate);
        ClientDepositLogInfo GetDepositLogStartDateAndNumberOfDepositDates(string clientCode, int lastNumberOfDays, DateTime endDate);
        int GetNumberOfDepositDaysOfClient(string clientCode, DateTime endDate);
        int GetNumberOfDepositDaysOfClientForGivenMonth(string clientCode, int month, int year);
        int GetNumberOfDepositWeeksForClient(string clientCode, DateTime endDate);
        bool SaveLastEnteredBusinessDaysOrWeeks(string clientCode, int lastEnteredBusiessDaysOrWeeks, string attributeCode);
        Dictionary<string, string> GetSavedLastNumberOfBusinessDaysOrWeeks(string clientCode);
        bool OpenOrCloseMonthForAClient(string clientCode, int monthID, int year, bool isCloseMonth);
        List<MonthDeposit> GetMonthWiseDepositsInAYearForClient(string clientCode, int year);
        bool IsMonthOfAYearClosedForAClient(string clientCode, int monthID, int year);
        List<DateTime> GetEnteredDepositDatesForClient(string clienCode);
        void SaveProjectedCashOfClientForAGivenMonth(string clientCode, int month, int year, decimal? projectedCash);
        int GetMinimumDepositYear(string clientCode);
        DepositLogProjectionViewModel GetProjectedCashForAClient(int clientId, int savedLastNumberOfBusinessDays, int month, int year);
        void SaveProjectedCashOfAClient(int clientId, DepositLogProjectionViewModel depositLogSimpleBusinessDaysViewModel);
    }
}
