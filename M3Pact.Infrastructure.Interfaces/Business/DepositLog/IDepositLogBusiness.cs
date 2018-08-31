using M3Pact.ViewModel;
using M3Pact.ViewModel.DepositLog;
using System;
using System.Collections.Generic;
using System.IO;

namespace M3Pact.Infrastructure.Interfaces.Business.DepositLog
{
    public interface IDepositLogBusiness
    {
        DepositLogClientDataAmountViewModel GetClientDepositLogData(string clientCode,int month,int year);
        ValidationViewModel SaveClientDepositLogData(DepositLogClientDataViewModel clientDepositLog);
        DepositLogClientDataViewModel GetPayerDetailsForClient(string clientCode);
        DepositLogMTDViewModel GetDepositLogMTD(string clientCode,int month,int year);
        DepositLogClientDataViewModel GetDepositLogsOfClientForDate(string clientCode, DateTime depositDate);
        DepositLogMonthlyTargetViewModel GetDepositLogMonthlyTargets(string clientCode,int month,int year);
        int? GetTotalBusinessDaysInaMonthOfAnYear(int year, int month);
        DepositLogProjectionViewModel GetProjectedCashFromSimpleBusinessDays(string clientCode, int numberOfLastWorkingDays, int month, int year, int savedLastNumberOfBusinessDays);
        DepositLogProjectionViewModel GetProjectedCashOfPreviousWeeks(string clientCode, int month, int year, int numberOfWeeks);
        DepositLogProjectedCashAmountWithPayment GetProjectedCashOfLastWorkingDays(string clientCode, int month, int year, int numberOfLastWorkingDays);
        List<int> GetNumberOfDepositDaysOfClient(string clientCode, int month, int year);
        int GetNumberOfDepositDaysOfClientForGivenMonth(string clientCode, int month, int year);
        List<DateTime> GetHolidayDates();
        ValidationViewModel SaveLastEnteredBusinessDaysOrWeeks(string clientCode, int lastEnteredBusiessDaysOrWeeks, string attributeName);
        Dictionary<string, string> GetSavedLastNumberOfBusinessDaysOrWeeks(string clientCode);
        ValidationViewModel CloseOrReopenAMonthOfAYearForClient(string clientCode, int monthID, int year, bool isCloseMonth);
        List<MonthDepositViewModel> GetClientMonthWiseDepositLogForAYear(string clientCode, int year);
        List<DateTime> GetEnteredDepositDatesForClient(string clientCode);
        MemoryStream GetDepositLogsForSelectedMonths(string clientCode,List<DateTime> months);
        int GetMinimumDepositYear(string clientCode);
    }
}
