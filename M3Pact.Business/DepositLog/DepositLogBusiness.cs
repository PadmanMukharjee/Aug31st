using M3Pact.Business.Common;
using M3Pact.BusinessModel;
using M3Pact.BusinessModel.BusinessModels;
using M3Pact.BusinessModel.DepositLog;
using M3Pact.BusinessModel.Mapper;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Common;
using M3Pact.Infrastructure.Interfaces.Business.DepositLog;
using M3Pact.Infrastructure.Interfaces.Repository;
using M3Pact.LoggerUtility;
using M3Pact.ViewModel;
using M3Pact.ViewModel.DepositLog;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace M3Pact.Business.DepositLog
{
    public class DepositLogBusiness : IDepositLogBusiness
    {
        #region properties
        private IDepositLogRepository _depositLogResposiotory;
        private Utility _utility;
        private Common.Helper _helper;
        private ILogger _logger;
        #endregion properties

        #region constructor
        public DepositLogBusiness(IDepositLogRepository DepositLogResposiotory, Common.Helper helper, ILogger logger)
        {
            _depositLogResposiotory = DepositLogResposiotory;
            _utility = new Utility();
            _helper = helper;
            _logger = logger;
        }
        #endregion constructor

        #region public methods
        /// <summary>
        /// To get ClientDeposit logs depending upon ClientCode
        /// </summary>
        /// <param name="ClientCode"></param>
        /// <returns></returns>
        public DepositLogClientDataAmountViewModel GetClientDepositLogData(string ClientCode, int month, int year)
        {
            try
            {
                DepositLogClientDataAmountViewModel depositLogClientDataAmountViewModel = null;
                BusinessModel.BusinessModels.DepositLogClientDataAmount clientDepositLogDTO = _depositLogResposiotory.GetDepositLogs(ClientCode, month, year);
                if (clientDepositLogDTO != null)
                {
                    depositLogClientDataAmountViewModel = new DepositLogClientDataAmountViewModel();
                    depositLogClientDataAmountViewModel.DepositLogData = clientDepositLogDTO.depositLogData;
                    depositLogClientDataAmountViewModel.DepositLogPayers = clientDepositLogDTO.depositLogPayers;
                    return depositLogClientDataAmountViewModel;
                }
                else
                {
                    return depositLogClientDataAmountViewModel;
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// To Save Deposite logs into Database
        /// </summary>
        /// <param name="clientDepositLog"></param>
        public ValidationViewModel SaveClientDepositLogData(DepositLogClientDataViewModel clientDepositLog)
        {
            ValidationViewModel response = new ValidationViewModel();
            BusinessResponse businessResponse = new BusinessResponse();
            try
            {
                List<BusinessModel.BusinessModels.ClientPayer> clientPayers = new List<ClientPayer>();
                BusinessModel.BusinessModels.DepositLog depositLogDTO = new BusinessModel.BusinessModels.DepositLog();
                foreach (PayerDataViewModel payer in clientDepositLog.Payers)
                {
                    BusinessModel.BusinessModels.ClientPayer clientPayer = new ClientPayer();
                    clientPayer.Payer = new BusinessModel.BusinessModels.Payer();
                    clientPayer.Payer.PayerCode = payer.PayerCode;
                    clientPayer.Payer.PayerName = payer.PayerName;

                    clientPayer.DepositLog = new List<BusinessModel.BusinessModels.DepositLog>();
                    clientPayer.DepositLog.Add(new BusinessModel.BusinessModels.DepositLog { DepositDate = clientDepositLog.Date, Amount = payer.Amount });
                    clientPayers.Add(clientPayer);
                }
                businessResponse.IsSuccess = _depositLogResposiotory.SaveDepositLogs(clientPayers, clientDepositLog.ClientCode, clientDepositLog.SavedLastNumberOfBusinessDays);
                if (businessResponse.IsSuccess)
                {
                    businessResponse.Messages.Add(new MessageDTO() { Message = BusinessConstants.DEPOSIT_LOG_SAVE_SUCCESS, MessageType = Infrastructure.Enums.MessageType.Info });
                }
                else
                {
                    businessResponse.Messages.Add(new MessageDTO() { Message = BusinessConstants.DEPOSIT_LOG_SAVE_FAIL, MessageType = Infrastructure.Enums.MessageType.Error });
                }
                return businessResponse.ToValidationViewModel();

            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                businessResponse.IsSuccess = false;
                businessResponse.IsExceptionOccured = true;
                businessResponse.Messages.Add(new MessageDTO() { Message = BusinessConstants.DEPOSIT_LOG_SAVE_FAIL, MessageType = Infrastructure.Enums.MessageType.Error });
                return businessResponse.ToValidationViewModel();
            }
        }

        /// <summary>
        /// To get ClientPayers associated to Client
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public DepositLogClientDataViewModel GetPayerDetailsForClient(string clientCode)
        {
            DepositLogClientDataViewModel clientPayers = new DepositLogClientDataViewModel();
            try
            {
                clientPayers.ClientCode = clientCode;
                clientPayers.Payers = GetPayersForClient(clientCode);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
            }
            return clientPayers;
        }

        /// <summary>
        /// To Get DepositLogs depending on DepositeDate and ClientCode
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="depositDate"></param>
        /// <returns></returns>
        public DepositLogClientDataViewModel GetDepositLogsOfClientForDate(string clientCode, DateTime depositDate)
        {
            DepositLogClientDataViewModel depositLogClientData = new DepositLogClientDataViewModel();
            try
            {
                List<BusinessModel.BusinessModels.ClientPayer> payerDetails = _depositLogResposiotory.GetDepositLogsForClientDate(clientCode, depositDate);
                depositLogClientData.ClientCode = clientCode;
                depositLogClientData.Date = depositDate.Date;
                depositLogClientData.Payers = new List<PayerDataViewModel>();
                if (payerDetails?.Count > 0)
                {
                    foreach (BusinessModel.BusinessModels.ClientPayer payer in payerDetails)
                    {
                        PayerDataViewModel clientPayer = new PayerDataViewModel();
                        clientPayer.PayerCode = payer.Payer?.PayerCode;
                        clientPayer.PayerName = payer.Payer?.PayerName;
                        clientPayer.Amount = payer.DepositLog?.First().Amount;
                        depositLogClientData.Payers.Add(clientPayer);
                    }
                }
                else
                {
                    depositLogClientData.Payers = GetPayersForClient(clientCode);
                }
                depositLogClientData.Total = depositLogClientData.Payers.Sum(c => c.Amount);
                depositLogClientData.IsMonthClosed = _depositLogResposiotory.IsMonthOfAYearClosedForAClient(clientCode, depositDate.Month, depositDate.Year);
                return depositLogClientData;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return depositLogClientData;
            }
        }

        /// <summary>
        /// To get the Deposit Log Month to date amount.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public DepositLogMTDViewModel GetDepositLogMTD(string clientCode, int month, int year)
        {
            try
            {
                DepositLogMTDViewModel depositLogMTDViewModel = null;
                DepositLogMTD depositLogMTD = _depositLogResposiotory.GetDepositLogMTD(clientCode, month, year);
                if (depositLogMTD != null)
                {
                    depositLogMTDViewModel = new DepositLogMTDViewModel();
                    if (depositLogMTD.BusinessDays > 0)
                    {
                        decimal? expectedOneMonthPayments = depositLogMTD.Payments;
                        depositLogMTDViewModel.MaxMTDValueTillDate = (_depositLogResposiotory.GetNumberOfDepositDaysOfClientForGivenMonth(clientCode, month, year) * expectedOneMonthPayments) / depositLogMTD.BusinessDays;
                    }
                    depositLogMTDViewModel.CoveredMTDValueTillDate = depositLogMTD.TotalDepositAmount != null ? depositLogMTD.TotalDepositAmount : (decimal)0.0;
                }
                return depositLogMTDViewModel;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// To get the Deposit Log Monthly Targets when month and year are given.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public DepositLogMonthlyTargetViewModel GetDepositLogMonthlyTargets(string clientCode, int month, int year)
        {
            DepositLogMonthlyTargetViewModel depositLogMonthlyTargetViewModel = new DepositLogMonthlyTargetViewModel();
            try
            {
                DepositLogMonthlyTarget depositLogMonthlyTarget = _depositLogResposiotory.GetDepositLogMonthlyTargets(clientCode, month, year);
                depositLogMonthlyTargetViewModel.Payments = depositLogMonthlyTarget.Payments;
                depositLogMonthlyTargetViewModel.EstimatedDepositAmountForCompleteMonth = (depositLogMonthlyTarget.DepositLogPaymentsTillDate * depositLogMonthlyTarget.BusinessDays) / GetNumberOfWorkingDaysforGivenMonthAndYear(month, year);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
            }
            return depositLogMonthlyTargetViewModel;
        }

        /// <summary>
        /// Fetches total no of business days in a Month of an Year
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public int? GetTotalBusinessDaysInaMonthOfAnYear(int year, int month)
        {
            try
            {
                return _depositLogResposiotory.GetTotalBusinessDaysInaMonthOfAnYear(year, month);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// To get the Projected Cash From Deposit Log when number of last working days are given.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="numberOfLastWorkingDays"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="savedLastNumberOfBusinessDays"></param>
        /// <returns></returns>
        public DepositLogProjectionViewModel GetProjectedCashFromSimpleBusinessDays(string clientCode, int numberOfLastWorkingDays, int month, int year , int savedLastNumberOfBusinessDays)
        {
            try
            {
                DepositLogProjectionViewModel depositLogSimpleBusinessDaysViewModel = null;
                DepositLogSimpleBusinessDays depositLogSimpleBusinessDays = null;
                DateTime endDate = DateTime.Today;
                decimal? depositLogAmountForMonth = 0;
                long? payments = null;
                if (new DateTime(year, month, 1) < new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1))
                {
                    endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                }
                ClientDepositLogInfo clientDepositLogInfo = new ClientDepositLogInfo();
                clientDepositLogInfo = _depositLogResposiotory.GetDepositLogStartDateAndNumberOfDepositDates(clientCode, numberOfLastWorkingDays, endDate);
                if (clientDepositLogInfo.NumberOfDepositDates > 0)
                {
                    depositLogSimpleBusinessDays = _depositLogResposiotory.GetProjectedCashFromSimpleBusinessDays(clientCode, clientDepositLogInfo.NumberOfDepositDates, month, year, clientDepositLogInfo.DepositStartDate, endDate);
                    if (depositLogSimpleBusinessDays != null)
                    {
                        depositLogSimpleBusinessDaysViewModel = new DepositLogProjectionViewModel();
                        depositLogSimpleBusinessDaysViewModel.Payments = depositLogSimpleBusinessDays.Payments;
                        depositLogSimpleBusinessDays.DepositLogPaymentsOnWeekEnds = depositLogSimpleBusinessDays.DepositLogPaymentsOnWeekEnds != null ? (decimal)depositLogSimpleBusinessDays.DepositLogPaymentsOnWeekEnds : (decimal)0.0;
                        depositLogSimpleBusinessDaysViewModel.DepositLogAmount = depositLogSimpleBusinessDays.DepositLogPaymentsOnWeekDays != null ? (decimal)depositLogSimpleBusinessDays.DepositLogPaymentsOnWeekDays : (decimal)0.0;
                        depositLogSimpleBusinessDaysViewModel.DepositLogAmount = ((depositLogSimpleBusinessDaysViewModel.DepositLogAmount / clientDepositLogInfo.NumberOfDepositDates) * depositLogSimpleBusinessDays.BusinessDays) + depositLogSimpleBusinessDays.DepositLogPaymentsOnWeekEnds;
                        depositLogSimpleBusinessDaysViewModel.NumberOfLastWorkingDaysOrWeeks = clientDepositLogInfo.NumberOfDepositDates;
                        return depositLogSimpleBusinessDaysViewModel;
                    }
                    else
                    {
                        return depositLogSimpleBusinessDaysViewModel;
                    }
                }
                else
                {
                    payments = _depositLogResposiotory.GetClientPaymentForMonth(clientCode, month, year);
                    depositLogSimpleBusinessDaysViewModel = GetDepositLogProjectionViewModel(depositLogSimpleBusinessDaysViewModel, payments, depositLogAmountForMonth, clientDepositLogInfo.NumberOfDepositDates);
                    return depositLogSimpleBusinessDaysViewModel;
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }

        }

        /// <summary>
        /// To get the Projected Cash From Deposit Log when number of weeks are given.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="numberOfWeeks"></param>
        /// <returns></returns>
        public DepositLogProjectionViewModel GetProjectedCashOfPreviousWeeks(string clientCode, int month, int year, int numberOfWeeks)
        {
            DepositLogProjectionViewModel depositLogProjectionViewModel = new DepositLogProjectionViewModel();
            try
            {
                decimal totalForecast = 0;
                decimal? totalDepositAmount = 0;
                DateTime startDate = new DateTime(year, month, 1);
                DateTime endDate = DateTime.Today;
                depositLogProjectionViewModel.Payments = _depositLogResposiotory.GetClientPaymentForMonth(clientCode, month, year);
                if (new DateTime(year, month, 1) < new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1))
                {
                    endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                }

                totalDepositAmount = _depositLogResposiotory.GetDepositLogAmountFromGivenDatesIncludingWeekEnds(clientCode, month, year, startDate, endDate);
                List<DepositLogWeekDays> depositLogWeekDaysList = _depositLogResposiotory.GetProjectedCashOfPreviousWeeks(clientCode, month, year, numberOfWeeks, endDate);
                if (depositLogWeekDaysList != null && depositLogWeekDaysList.Count == 5)
                {
                    foreach (DepositLogWeekDays depositLogWeekDays in depositLogWeekDaysList)
                    {
                        decimal averageDepositLogAmountOfWeekDay = depositLogWeekDays.DepositAmount / depositLogWeekDays.WeekDaysCompleted;
                        decimal forecastDepositLogAmountOfWeekDay = averageDepositLogAmountOfWeekDay * depositLogWeekDays.WeekDaysLeft;
                        totalForecast += forecastDepositLogAmountOfWeekDay;
                        depositLogProjectionViewModel.NumberOfLastWorkingDaysOrWeeks = depositLogWeekDays.WeekDaysCompleted;
                    }
                    depositLogProjectionViewModel.DepositLogAmount = totalDepositAmount != null ? (decimal)totalDepositAmount : (decimal)0.0;
                    depositLogProjectionViewModel.DepositLogAmount += totalForecast;
                    return depositLogProjectionViewModel;
                }
                else
                {
                    depositLogProjectionViewModel.NumberOfLastWorkingDaysOrWeeks = 0;
                    depositLogProjectionViewModel.DepositLogAmount = totalForecast;
                    return depositLogProjectionViewModel;
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return depositLogProjectionViewModel;
            }

        }

        /// <summary>
        /// To get the projected cash of the last working days and the target payments for the given month.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="numberOfLastWorkingDays"></param>
        /// <returns></returns>
        public DepositLogProjectedCashAmountWithPayment GetProjectedCashOfLastWorkingDays(string clientCode, int month, int year, int numberOfLastWorkingDays)
        {
            try
            {
                DepositLogProjectedCashAmountWithPayment depositLogProjectedCashAmountWithPayment = null;
                List<DepositLogProjectedCashViewModel> projectedCashViewModel = null;
                List<DepositLogProjectedCashViewModel> previousDateViewModel = null;
                List<DepositLogProjectedCash> depositLogProjectedCashList = null;
                DateTime endDate = DateTime.Today;
                ClientDepositLogInfo clientDepositLogInfo = new ClientDepositLogInfo();
                clientDepositLogInfo = _depositLogResposiotory.GetDepositLogStartDateAndNumberOfDepositDates(clientCode, numberOfLastWorkingDays, DateTime.Today);
                long? payments = _depositLogResposiotory.GetClientPaymentForMonth(clientCode, month, year);
                if (clientDepositLogInfo.NumberOfDepositDates > 0)
                {
                    depositLogProjectedCashList = _depositLogResposiotory.GetProjectedCashOfLastWorkingDays(clientCode, month, year, numberOfLastWorkingDays, clientDepositLogInfo.DepositStartDate, endDate);
                    if (depositLogProjectedCashList != null)
                    {
                        depositLogProjectedCashAmountWithPayment = new DepositLogProjectedCashAmountWithPayment();
                        projectedCashViewModel = new List<DepositLogProjectedCashViewModel>();
                        previousDateViewModel = new List<DepositLogProjectedCashViewModel>();
                        foreach (DepositLogProjectedCash depositLogProjectedCash in depositLogProjectedCashList)
                        {
                            DepositLogProjectedCashViewModel projectedCash = new DepositLogProjectedCashViewModel();
                            DepositLogProjectedCashViewModel previousDate = new DepositLogProjectedCashViewModel();
                            projectedCash.name = depositLogProjectedCash.LastWorkingDayNumber.ToString();
                            projectedCash.value = depositLogProjectedCash.ProjectedCash;
                            previousDate.name = depositLogProjectedCash.LastWorkingDayNumber.ToString();
                            previousDate.value = payments != null ? (decimal)payments : (decimal)0.0;
                            projectedCashViewModel.Add(projectedCash);
                            previousDateViewModel.Add(previousDate);
                        }
                        depositLogProjectedCashAmountWithPayment.DepositLogProjectedCashViewModelList = projectedCashViewModel;
                        depositLogProjectedCashAmountWithPayment.Payments = previousDateViewModel;
                    }
                }
                return depositLogProjectedCashAmountWithPayment;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// To get the number of deposit days and deposit weeks for a client.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public List<int> GetNumberOfDepositDaysOfClient(string clientCode, int month, int year)
        {
            try
            {
                DateTime endDate = DateTime.Today.Date;
                if (new DateTime(year, month, 1) < new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1))
                {
                    endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                }
                List<int> clientDepositDaysInfo = new List<int>();
                clientDepositDaysInfo.Add(_depositLogResposiotory.GetNumberOfDepositDaysOfClient(clientCode, endDate));
                clientDepositDaysInfo.Add(_depositLogResposiotory.GetNumberOfDepositWeeksForClient(clientCode, endDate));
                return clientDepositDaysInfo;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// To get the number of deposit days of a client for a given month.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public int GetNumberOfDepositDaysOfClientForGivenMonth(string clientCode, int month, int year)
        {
            try
            {
                return _depositLogResposiotory.GetNumberOfDepositDaysOfClientForGivenMonth(clientCode, month, year);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// To get holiday dates.
        /// </summary>
        /// <returns></returns>
        public List<DateTime> GetHolidayDates()
        {
            try
            {
                List<DateTime> holidayListOfYear = new List<DateTime>();
                holidayListOfYear = _depositLogResposiotory.GetHolidayDates(new DateTime(1900, 1, 1).Date, DateTime.Today.Date);
                return holidayListOfYear;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Get All Deposit Dates Submitted For Client
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public List<DateTime> GetEnteredDepositDatesForClient(string clientCode)
        {
            try
            {
                return _depositLogResposiotory.GetEnteredDepositDatesForClient(clientCode);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Save last entered business days or weeks for a client.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="lastEnteredBusiessDaysOrWeeks"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public ValidationViewModel SaveLastEnteredBusinessDaysOrWeeks(string clientCode, int lastEnteredBusiessDaysOrWeeks, string attributeName)
        {
            ValidationViewModel response = new ValidationViewModel();
            BusinessResponse businessResponse = new BusinessResponse();
            try
            {
                businessResponse.IsSuccess = _depositLogResposiotory.SaveLastEnteredBusinessDaysOrWeeks(clientCode, lastEnteredBusiessDaysOrWeeks, attributeName);
                if (businessResponse.IsSuccess)
                {
                    businessResponse.Messages.Add(new MessageDTO() { Message = BusinessConstants.DEPOSIT_LOG_SAVE_SUCCESS, MessageType = Infrastructure.Enums.MessageType.Info });
                }
                else
                {
                    businessResponse.Messages.Add(new MessageDTO() { Message = BusinessConstants.DEPOSIT_LOG_ATTRIBUTE_SAVE_FAIL, MessageType = Infrastructure.Enums.MessageType.Error });
                }
                return businessResponse.ToValidationViewModel();
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                businessResponse.IsSuccess = false;
                businessResponse.IsExceptionOccured = true;
                businessResponse.Messages.Add(new MessageDTO() { Message = BusinessConstants.DEPOSIT_LOG_ATTRIBUTE_SAVE_FAIL, MessageType = Infrastructure.Enums.MessageType.Error });
                return businessResponse.ToValidationViewModel();

            }
        }

        /// <summary>
        /// Get the saved last number of business days or weeks.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetSavedLastNumberOfBusinessDaysOrWeeks(string clientCode)
        {
            try
            {
                return _depositLogResposiotory.GetSavedLastNumberOfBusinessDaysOrWeeks(clientCode);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Close Or Reopen A Month Of A Year For Client
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="monthID"></param>
        /// <param name="year"></param>
        /// <param name="isCloseMonth"></param>
        /// <returns></returns>
        public ValidationViewModel CloseOrReopenAMonthOfAYearForClient(string clientCode, int monthID, int year, bool isCloseMonth)
        {
            ValidationViewModel validationViewModel = new ValidationViewModel();
            BusinessResponse businessResponse = new BusinessResponse();
            string message = string.Empty;
            try
            {
                businessResponse.IsSuccess = _depositLogResposiotory.OpenOrCloseMonthForAClient(clientCode, monthID, year, isCloseMonth);
                if (businessResponse.IsSuccess)
                {
                    message = isCloseMonth ? BusinessConstants.DEPOSIT_LOG_CLOSE_MONTH_SUCCESS : BusinessConstants.DEPOSIT_LOG_REOPEN_MONTH_SUCCESS;
                    businessResponse.Messages.Add(new MessageDTO() { Message = message, MessageType = Infrastructure.Enums.MessageType.Info });
                }
                else
                {
                    message = isCloseMonth ? BusinessConstants.DEPOSIT_LOG_CLOSE_MONTH_FAIL : BusinessConstants.DEPOSIT_LOG_REOPEN_MONTH_FAIL;
                    businessResponse.Messages.Add(new MessageDTO() { Message = message, MessageType = Infrastructure.Enums.MessageType.Error });
                }
                return businessResponse.ToValidationViewModel();
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                businessResponse.IsSuccess = false;
                businessResponse.IsExceptionOccured = true;
                message = isCloseMonth ? BusinessConstants.DEPOSIT_LOG_CLOSE_MONTH_FAIL : BusinessConstants.DEPOSIT_LOG_REOPEN_MONTH_FAIL;
                businessResponse.Messages.Add(new MessageDTO() { Message = message, MessageType = Infrastructure.Enums.MessageType.Error });
                return businessResponse.ToValidationViewModel();
            }
        }

        /// <summary>
        /// Get Client Month Wise DepositLog For A Year
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<MonthDepositViewModel> GetClientMonthWiseDepositLogForAYear(string clientCode, int year)
        {
            List<MonthDepositViewModel> monthDepositsViewModel = new List<MonthDepositViewModel>();
            try
            {
                List<MonthDeposit> monthDeposits = _depositLogResposiotory.GetMonthWiseDepositsInAYearForClient(clientCode, year);
                if (monthDeposits != null && monthDeposits.Count > 0)
                {
                    monthDepositsViewModel = ConvertViewModelFromMonthDepositBusinessModel(monthDeposits);
                }
                return monthDepositsViewModel;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return monthDepositsViewModel;
            }
        }

        /// <summary>
        /// To export deposit log data
        /// </summary>
        /// <param name="months"></param>
        /// <returns></returns>
        public MemoryStream GetDepositLogsForSelectedMonths(string clientCode, List<DateTime> months)
        {
            try
            {
                string sWebRootFolder = "";
                string sFileName = @"demo.xlsx";
                FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                var memory = new MemoryStream();
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();

                    foreach (DateTime month in months)
                    {
                        string monthName = DateHelper.ToMonthName(month);
                        ISheet excelSheet = workbook.CreateSheet(monthName);
                        IRow row = excelSheet.CreateRow(0);
                        BusinessModel.BusinessModels.DepositLogClientDataAmount clientDepositLogDTO = _depositLogResposiotory.GetDepositLogs(clientCode, month.Month, month.Year);

                        int i = 0;
                        foreach (var payer in clientDepositLogDTO.depositLogPayers)
                        {
                            row.CreateCell(i).SetCellValue(payer);
                            i++;
                        }

                        int j = 1;
                        foreach (var deposit in clientDepositLogDTO.depositLogData)
                        {
                            row = excelSheet.CreateRow(j);
                            var list = deposit as Dictionary<string, Object>;
                            i = 0;
                            foreach (var payer in clientDepositLogDTO.depositLogPayers)
                            {
                                var value = list.FirstOrDefault(c => c.Key == payer).Value;
                                row.CreateCell(i).SetCellValue(value.ToString());
                                i++;
                            }
                            j++;
                        }
                    }
                    workbook.Write(fs);
                }
                using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
                {
                    stream.CopyToAsync(memory).Wait();
                }
                memory.Position = 0;
                return memory;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// To get the minimum year for the client deposits
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public int GetMinimumDepositYear(string clientCode)
        {
            try
            {
                return _depositLogResposiotory.GetMinimumDepositYear(clientCode);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return 0;
            }

        }
        #endregion public methods

        #region private methods
        private List<PayerDataViewModel> GetPayersForClient(string clientCode)
        {
            List<PayerDataViewModel> clientPayers = new List<PayerDataViewModel>();
            List<BusinessModel.BusinessModels.Payer> payerDetails = _depositLogResposiotory.GetPayerDetailsForClient(clientCode);
            foreach (BusinessModel.BusinessModels.Payer payer in payerDetails)
            {
                PayerDataViewModel clientPayer = new PayerDataViewModel();
                clientPayer.PayerCode = payer.PayerCode;
                clientPayer.PayerName = payer.PayerName;
                clientPayers.Add(clientPayer);
            }
            return clientPayers;
        }

        /// <summary>
        /// Helper method that forms the DepositLogProjectionViewModel instance.
        /// </summary>
        /// <param name="depositLogSimpleBusinessDaysViewModel"></param>
        /// <param name="payments"></param>
        /// <param name="depositLogAmountForMonth"></param>
        /// <param name="numberOfLastWorkingDays"></param>
        /// <returns></returns>
        private DepositLogProjectionViewModel GetDepositLogProjectionViewModel(DepositLogProjectionViewModel depositLogSimpleBusinessDaysViewModel, long? payments, decimal? depositLogAmountForMonth, int numberOfLastWorkingDays)
        {
            if (payments != null)
            {
                depositLogSimpleBusinessDaysViewModel = new DepositLogProjectionViewModel();
                depositLogSimpleBusinessDaysViewModel.Payments = payments;
                depositLogSimpleBusinessDaysViewModel.DepositLogAmount = depositLogAmountForMonth;
                depositLogSimpleBusinessDaysViewModel.NumberOfLastWorkingDaysOrWeeks = numberOfLastWorkingDays;
                return depositLogSimpleBusinessDaysViewModel;
            }
            else
            {
                return depositLogSimpleBusinessDaysViewModel;
            }
        }

        /// <summary>
        /// To get the number of working days for a month when month and year are given.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private int GetNumberOfWorkingDaysforGivenMonthAndYear(int month, int year)
        {
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = DateTime.Today;
            return _depositLogResposiotory.GetNumberOfBusinessDaysFromStartDateAndEndDate(startDate, endDate);
        }

        #endregion private methods

        #region Mapper Methods
        /// <summary>
        /// Convert MonthDeposit Business Model to MonthDepositViewModel View Model
        /// </summary>
        /// <param name="monthDeposits"></param>
        /// <returns></returns>
        private List<MonthDepositViewModel> ConvertViewModelFromMonthDepositBusinessModel(List<MonthDeposit> monthDeposits)
        {
            List<MonthDepositViewModel> monthDepositViewModelList = new List<MonthDepositViewModel>();
            foreach (MonthDeposit monthDeposit in monthDeposits)
            {
                MonthDepositViewModel monthDepositViewModel = new MonthDepositViewModel();
                monthDepositViewModel.MonthID = monthDeposit.MonthID;
                monthDepositViewModel.MonthName = monthDeposit.MonthName;
                monthDepositViewModel.MonthStatus = monthDeposit.MonthStatus;
                monthDepositViewModel.Target = monthDeposit.Target;
                monthDepositViewModel.ActualDeposit = monthDeposit.ActualDeposit;
                if (monthDeposit.MetPercent != null)
                {
                    monthDepositViewModel.MetPercent = monthDeposit.MetPercent.ToString() + " %";
                    monthDepositViewModel.MetPercentStatus = monthDeposit.MetPercent >= BusinessConstants.CENT_VALUE ? BusinessConstants.STATUS_UP : BusinessConstants.STATUS_DOWN;
                }
                monthDepositViewModelList.Add(monthDepositViewModel);
            }
            return monthDepositViewModelList;
        }
        #endregion Mapper Methods
    }
}
