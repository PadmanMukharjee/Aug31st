using M3Pact.BusinessModel.BusinessModels;
using M3Pact.BusinessModel.DepositLog;
using M3Pact.DomainModel.DomainModels;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Common;
using M3Pact.Infrastructure.Interfaces.Repository;
using M3Pact.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace M3Pact.Repository.DepositLog
{
    public class DepositLogRepository : IDepositLogRepository
    {
        #region private Properties
        private M3PactContext _m3pactContext;
        private IConfiguration _Configuration { get; }
        private UserContext userContext;
        #endregion

        #region Constructor
        public DepositLogRepository(M3PactContext m3PactContext, IConfiguration configuration)
        {
            _m3pactContext = m3PactContext;
            _Configuration = configuration;
            userContext = UserHelper.getUserContext();
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// To get Depositlogs of a Client
        /// </summary>
        /// <param name="ClientCode"></param>
        /// <returns></returns>
        public DepositLogClientDataAmount GetDepositLogs(string clientCode, int month, int year)
        {
            try
            {
                DepositLogClientDataAmount depositLogClientDataAmount = new DepositLogClientDataAmount();
                depositLogClientDataAmount.depositLogData = new List<Object>();
                List<string> clientPayers = new List<string>();
                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, "M3PactConnection");
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    string sql = DomainConstants.GetDepositLog;
                    using (SqlCommand sqlCmd = new SqlCommand(sql, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@ClientCode", clientCode);
                        sqlCmd.Parameters.AddWithValue("@Month", month);
                        sqlCmd.Parameters.AddWithValue("@Year", year);
                        sqlConn.Open();
                        SqlDataReader reader = sqlCmd.ExecuteReader();
                        depositLogClientDataAmount.depositLogPayers = new List<string>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            clientPayers.Add(reader.GetName(i));
                            depositLogClientDataAmount.depositLogPayers.Add(reader.GetName(i).Trim());
                        }
                        while (reader.Read())
                        {
                            Dictionary<string, Object> rowList = new Dictionary<string, Object>();
                            rowList.Add("Deposit Date", reader[depositLogClientDataAmount.depositLogPayers[0]]);
                            for (int i = 1; i < clientPayers.Count; i++)
                            {
                                rowList.Add(depositLogClientDataAmount.depositLogPayers[i], reader[clientPayers[i]]);
                            }
                            depositLogClientDataAmount.depositLogData.Add(rowList);
                        }
                    }
                    sqlConn.Close();
                }
                return depositLogClientDataAmount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To Save DepositLogs of a Client
        /// </summary>
        /// <param name="clientDepositLogs"></param>
        public bool SaveDepositLogs(List<BusinessModel.BusinessModels.ClientPayer> clientDepositLogs, string clientCode, int savedLastNumberOfBusinessDays)
        {
            try
            {
                UserContext userContext = UserHelper.getUserContext();
                List<DomainModel.DomainModels.ClientPayer> data = (from c in _m3pactContext.Client
                                                                   join cp in _m3pactContext.ClientPayer
                                                                   on c.ClientId equals cp.ClientId
                                                                   join p in _m3pactContext.Payer
                                                                   on cp.PayerId equals p.PayerId
                                                                   join d in _m3pactContext.DepositLog
                                                                   on cp.ClientPayerId equals d.ClientPayerId
                                                                   join dd in _m3pactContext.DateDimension
                                                                   on d.DepositDateId equals dd.DateKey
                                                                   where c.ClientCode == clientCode
                                                                     && c.IsActive == DomainConstants.RecordStatusActive
                                                                     && cp.RecordStatus != DomainConstants.RecordStatusDelete
                                                                     && cp.EndDate >= clientDepositLogs.First().DepositLog.First().DepositDate.Value
                                                                     && p.RecordStatus != DomainConstants.RecordStatusDelete
                                                                     && p.EndDate >= clientDepositLogs.First().DepositLog.First().DepositDate.Value
                                                                     && d.RecordStatus == DomainConstants.RecordStatusActive
                                                                     && dd.RecordStatus == DomainConstants.RecordStatusActive
                                                                     && dd.Date == clientDepositLogs.First().DepositLog.First().DepositDate.Value
                                                                   select new DomainModel.DomainModels.ClientPayer()
                                                                   {
                                                                       ClientPayerId = cp.ClientPayerId,
                                                                       Payer = new DomainModel.DomainModels.Payer() { PayerCode = p.PayerCode, PayerName = p.PayerName },
                                                                       DepositLog = new List<DomainModel.DomainModels.DepositLog>()
                                                                       { new DomainModel.DomainModels.DepositLog()
                                                                       {
                                                                         DepositLogId = d.DepositLogId,
                                                                         Amount = d.Amount,RecordStatus=d.RecordStatus,
                                                                         CreatedBy = d.CreatedBy,ModifiedBy=d.ModifiedBy,CreatedDate=d.CreatedDate,
                                                                         ModifiedDate = d.ModifiedDate,
                                                                         ClientPayerId=d.ClientPayerId,
                                                                         DepositDateId=d.DepositDateId
                                                                       }
                                                                       }

                                                                   })?.ToListAsync().Result;

                DateTime depositDate = clientDepositLogs.First().DepositLog.First().DepositDate.Value;
                foreach (BusinessModel.BusinessModels.ClientPayer clientPayer in clientDepositLogs)
                {
                    DomainModel.DomainModels.DepositLog depositLog = data?.Where(c => c.Payer.PayerCode == clientPayer.Payer.PayerCode)?.FirstOrDefault()?.DepositLog.FirstOrDefault();
                    if (depositLog != null)
                    {
                        if (depositLog.Amount != clientPayer?.DepositLog?.First()?.Amount)
                        {
                            depositLog.RecordStatus = DomainConstants.RecordStatusInactive;
                            depositLog.ModifiedBy = userContext.UserId;
                            depositLog.ModifiedDate = DateTime.UtcNow;
                            _m3pactContext.DepositLog.Update(depositLog);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    DomainModel.DomainModels.DepositLog newDepositLog = new DomainModel.DomainModels.DepositLog();
                    newDepositLog.Amount = clientPayer.DepositLog.First().Amount;
                    newDepositLog.DepositDateId = _m3pactContext.DateDimension.Where(c => c.Date == depositDate).FirstOrDefault()?.DateKey;
                    newDepositLog.CreatedBy = depositLog != null ? depositLog.CreatedBy : userContext.UserId;
                    newDepositLog.ModifiedBy = userContext.UserId;
                    newDepositLog.RecordStatus = DomainConstants.RecordStatusActive;
                    newDepositLog.CreatedDate = depositLog != null ? depositLog.CreatedDate : DateTime.Now;
                    newDepositLog.ModifiedDate = DateTime.Now;
                    newDepositLog.ClientPayerId = depositLog != null ? depositLog.ClientPayerId : (from c in _m3pactContext.Client
                                                                                                   join cp in _m3pactContext.ClientPayer
                                                                                                   on c.ClientId equals cp.ClientId
                                                                                                   join p in _m3pactContext.Payer
                                                                                                   on cp.PayerId equals p.PayerId
                                                                                                   where c.ClientCode == clientCode
                                                                                                   && p.PayerCode == clientPayer.Payer.PayerCode
                                                                                                   && c.IsActive == DomainConstants.RecordStatusActive
                                                                                                   && cp.RecordStatus != DomainConstants.RecordStatusDelete
                                                                                                   && p.RecordStatus != DomainConstants.RecordStatusDelete
                                                                                                   select new { clientPayerId = cp.ClientPayerId }
                                                                                                   )?.FirstOrDefault()?.clientPayerId;
                    _m3pactContext.DepositLog.Add(newDepositLog);
                }
                int saveId = _m3pactContext.SaveChanges();
                if (saveId >= 0)
                {
                    int clientId = _m3pactContext.Client.Where(c => c.ClientCode == clientCode).Select(c => c.ClientId).FirstOrDefault();
                    SaveDepositLogSumOfaClient(clientCode, depositDate);
                    DepositLogProjectionViewModel depositLogProjectionViewModel = GetProjectedCashForAClient(clientId , savedLastNumberOfBusinessDays , DateTime.Today.Month , DateTime.Today.Year);
                    if (depositLogProjectionViewModel != null)
                    {
                        SaveProjectedCashOfAClient(clientId, depositLogProjectionViewModel);
                        _m3pactContext.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To Get ClientPayers of a Client
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public List<BusinessModel.BusinessModels.Payer> GetPayerDetailsForClient(string clientCode)
        {
            try
            {
                List<BusinessModel.BusinessModels.Payer> payerDetails = (from c in _m3pactContext.Client
                                                                         join cp in _m3pactContext.ClientPayer
                                                                         on c.ClientId equals cp.ClientId
                                                                         join p in _m3pactContext.Payer
                                                                         on cp.PayerId equals p.PayerId
                                                                         where c.ClientCode == clientCode
                                                                            && c.IsActive == DomainConstants.RecordStatusActive
                                                                            && cp.RecordStatus == DomainConstants.RecordStatusActive
                                                                            && p.RecordStatus == DomainConstants.RecordStatusActive
                                                                         select new BusinessModel.BusinessModels.Payer()
                                                                         {
                                                                             PayerName = p.PayerName,
                                                                             PayerCode = p.PayerCode
                                                                         })?.OrderBy(c => c.PayerName).ToList();

                return payerDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To Get DepositLogs depending upon Date abd ClientCode
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="depositDate"></param>
        /// <returns></returns>
        public List<BusinessModel.BusinessModels.ClientPayer> GetDepositLogsForClientDate(string clientCode, DateTime depositDate)
        {
            try
            {
                List<BusinessModel.BusinessModels.ClientPayer> payerDetails = new List<BusinessModel.BusinessModels.ClientPayer>();
                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, "M3PactConnection");
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    string sql = DomainConstants.usp_GetDepositLogsForClientDate;
                    using (SqlCommand sqlCmd = new SqlCommand(sql, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter sinceDateTimeParam = new SqlParameter("@DepositDate", SqlDbType.DateTime);
                        sinceDateTimeParam.Value = depositDate;
                        sqlCmd.Parameters.Add(sinceDateTimeParam);
                        sqlCmd.Parameters.AddWithValue("@ClientCode", clientCode);

                        sqlConn.Open();
                        SqlDataReader dr = sqlCmd.ExecuteReader();
                        while (dr.Read())
                        {
                            BusinessModel.BusinessModels.ClientPayer clientPayer = new BusinessModel.BusinessModels.ClientPayer();
                            clientPayer.Payer = new BusinessModel.BusinessModels.Payer();
                            BusinessModel.BusinessModels.DepositLog deposit = new BusinessModel.BusinessModels.DepositLog();
                            if (dr["PayerCode"] != DBNull.Value)
                            {
                                clientPayer.Payer.PayerCode = dr["PayerCode"].ToString();
                            }
                            if (dr["PayerName"] != DBNull.Value)
                            {
                                clientPayer.Payer.PayerName = dr["PayerName"].ToString();
                            }
                            if (dr["Amount"] != DBNull.Value)
                            {
                                deposit.Amount = Convert.ToDecimal(dr["Amount"]);
                            }
                            else
                            {
                                deposit.Amount = null;
                            }
                            clientPayer.DepositLog.Add(deposit);
                            payerDetails.Add(clientPayer);
                        }
                    }
                    sqlConn.Close();
                }
                return payerDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// To Get the Deposit Log MTD Values. 
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public DepositLogMTD GetDepositLogMTD(string clientCode, int month, int year)
        {
            try
            {
                DepositLogMTD depositMtd = (from c in _m3pactContext.Client
                                            join ct in _m3pactContext.ClientTarget
                                            on c.ClientId equals ct.ClientId
                                            join bd in _m3pactContext.BusinessDays
                                            on ct.MonthId equals bd.MonthId
                                            join mo in _m3pactContext.Month
                                            on ct.MonthId equals mo.MonthId
                                            where c.ClientCode == clientCode
                                            && ct.MonthId == month
                                            && ct.CalendarYear == year
                                            && bd.Year == year
                                            && c.IsActive == DomainConstants.RecordStatusActive
                                            && bd.RecordStatus == DomainConstants.RecordStatusActive && ct.RecordStatus == DomainConstants.RecordStatusActive
                                            && mo.RecordStatus == DomainConstants.RecordStatusActive
                                            select new BusinessModel.BusinessModels.DepositLogMTD()
                                            {
                                                ClientId = ct.ClientId,
                                                MonthCode = mo.MonthCode,
                                                AnnualCharges = ct.AnnualCharges,
                                                GrossCollectionRate = ct.GrossCollectionRate,
                                                Payments = ct.Payments,
                                                BusinessDays = bd.BusinessDays1,
                                                TotalDepositAmount = GetDepositLogOfCurrentMonth(clientCode, month, year)
                                            }).FirstOrDefault();
                return depositMtd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To Get the total number of business days.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public int GetBusinessTotalDays(int year)
        {
            try
            {
                int totalBusinessDays = (from bd in _m3pactContext.BusinessDays
                                         where bd.Year == year
                                         group bd by bd.Year into g
                                         select g.Sum(x => x.BusinessDays1)
                                         ).FirstOrDefault();
                return totalBusinessDays;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get the deposit log amount of a client till current date.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public decimal? GetDepositLogOfCurrentMonth(string clientCode, int month, int year)
        {
            try
            {
                DateTime startDate = new DateTime(year, month, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);
                decimal? totalDepositsTillDate = (from c in _m3pactContext.Client
                                                  join cp in _m3pactContext.ClientPayer
                                                  on c.ClientId equals cp.ClientId
                                                  join dl in _m3pactContext.DepositLog
                                                  on cp.ClientPayerId equals dl.ClientPayerId
                                                  join dd in _m3pactContext.DateDimension
                                                  on dl.DepositDateId equals dd.DateKey
                                                  where c.ClientCode == clientCode
                                                  && dd.Date >= startDate
                                                  && dd.Date <= endDate
                                                  && c.IsActive == DomainConstants.RecordStatusActive
                                                  //&& cp.RecordStatus == DomainConstants.RecordStatusActive
                                                  && dl.RecordStatus == DomainConstants.RecordStatusActive
                                                  select dl.Amount
                                             ).Sum();
                return totalDepositsTillDate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// To get the Monthly Targets of the Deposit Log.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public DepositLogMonthlyTarget GetDepositLogMonthlyTargets(string clientCode, int month, int year)
        {
            try
            {
                DepositLogMonthlyTarget monthlyTargets = (from c in _m3pactContext.Client
                                                          join ct in _m3pactContext.ClientTarget
                                                          on c.ClientId equals ct.ClientId
                                                          join m in _m3pactContext.Month
                                                          on ct.MonthId equals m.MonthId
                                                          join bd in _m3pactContext.BusinessDays
                                                          on m.MonthId equals bd.MonthId
                                                          where c.ClientCode == clientCode
                                                          && ct.MonthId == month
                                                          && ct.CalendarYear == year
                                                          && bd.Year == year
                                                          && c.IsActive == DomainConstants.RecordStatusActive
                                                          && ct.RecordStatus == DomainConstants.RecordStatusActive
                                                          && m.RecordStatus == DomainConstants.RecordStatusActive
                                                          && bd.RecordStatus == DomainConstants.RecordStatusActive
                                                          select new BusinessModel.BusinessModels.DepositLogMonthlyTarget()
                                                          {
                                                              BusinessDays = bd.BusinessDays1,
                                                              Payments = ct.Payments,
                                                              DepositLogPaymentsTillDate = GetDepositLogOfCurrentMonth(clientCode, month, year)
                                                          }).FirstOrDefault();
                return monthlyTargets;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Fetches total no of business days in a Month of an Year
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public int? GetTotalBusinessDaysInaMonthOfAnYear(int year, int month)
        {
            int? no_of_days = null;
            try
            {
                no_of_days = _m3pactContext.BusinessDays.FirstOrDefault(x => x.MonthId == month && x.Year == year)?.BusinessDays1;
                return no_of_days;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get deposit log amount when from date and to date is given excluding weekends deposit amount.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="weekEndList"></param>
        /// <returns></returns>
        public decimal? GetDepositLogAmountFromGivenDatesExcludingWeekEnds(string clientCode, DateTime? startDate, DateTime endDate)
        {
            try
            {
                decimal? totalDepositsGivenDates = (from c in _m3pactContext.Client
                                                    join cp in _m3pactContext.ClientPayer
                                                    on c.ClientId equals cp.ClientId
                                                    join dl in _m3pactContext.DepositLog
                                                    on cp.ClientPayerId equals dl.ClientPayerId
                                                    join dd in _m3pactContext.DateDimension
                                                    on dl.DepositDateId equals dd.DateKey
                                                    where c.ClientCode == clientCode
                                                    && c.IsActive == DomainConstants.RecordStatusActive
                                                    //&& cp.RecordStatus != DomainConstants.RecordStatusDelete
                                                    && dl.RecordStatus == DomainConstants.RecordStatusActive
                                                    && dd.Date >= startDate
                                                    && dd.Date <= endDate
                                                    && dd.IsWeekend == false
                                                    && dd.IsHoliday == false
                                                    select dl.Amount
                                                   ).Sum();
                return totalDepositsGivenDates;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// To get the projected cash of deposit log amount when number of last business working days are given.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="numberOfDays"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="weekEndList"></param>
        /// <returns></returns>
        public DepositLogSimpleBusinessDays GetProjectedCashFromSimpleBusinessDays(string clientCode, int numberOfDays, int month, int year, DateTime? startDate, DateTime endDate)
        {
            try
            {
                DateTime? weekEndDepositStartDate = startDate;
                if (startDate < new DateTime(year, month, 1))
                {
                    weekEndDepositStartDate = new DateTime(year, month, 1);
                }
                DepositLogSimpleBusinessDays monthlyTargets = (from c in _m3pactContext.Client
                                                               join ct in _m3pactContext.ClientTarget
                                                               on c.ClientId equals ct.ClientId
                                                               join m in _m3pactContext.Month
                                                               on ct.MonthId equals m.MonthId
                                                               join bd in _m3pactContext.BusinessDays
                                                               on m.MonthId equals bd.MonthId
                                                               where c.ClientCode == clientCode
                                                               && ct.MonthId == month
                                                               && ct.CalendarYear == year
                                                               && bd.Year == year
                                                               && c.IsActive == DomainConstants.RecordStatusActive
                                                               && ct.RecordStatus == DomainConstants.RecordStatusActive
                                                               && m.RecordStatus == DomainConstants.RecordStatusActive
                                                               && bd.RecordStatus == DomainConstants.RecordStatusActive

                                                               select new BusinessModel.BusinessModels.DepositLogSimpleBusinessDays()
                                                               {
                                                                   BusinessDays = bd.BusinessDays1,
                                                                   Payments = ct.Payments,
                                                                   DepositLogPaymentsOnWeekDays = GetDepositLogAmountFromGivenDatesExcludingWeekEnds(clientCode, startDate, endDate),
                                                                   DepositLogPaymentsOnWeekEnds = GetDepositLogAmountOnWeekEnds(clientCode, weekEndDepositStartDate, endDate)
                                                               }).FirstOrDefault();
                return monthlyTargets;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// To get the holiday list when start date and end date are given.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<DateTime> GetHolidayDates(DateTime startDate, DateTime endDate)
        {
            try
            {
                List<DateTime> listOfHolidays = new List<DateTime>();
                listOfHolidays = (from dd in _m3pactContext.DateDimension
                                  where dd.Date >= startDate
                                  && dd.Date <= endDate
                                  && dd.IsHoliday == true
                                  select dd.Date.Date
                                  ).ToList();
                return listOfHolidays;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get All Entered DepositDates For Client
        /// </summary>
        /// <param name="clienCode"></param>
        /// <returns></returns>
        public List<DateTime> GetEnteredDepositDatesForClient(string clienCode)
        {
            try
            {
                List<DateTime> depositDates = new List<DateTime>();
                int clientID = _m3pactContext.Client.Where(x => x.ClientCode == clienCode).Select(x => x.ClientId)?.First() ?? 0;
                depositDates = (from dl in _m3pactContext.DepositLog
                                join dd in _m3pactContext.DateDimension.AsNoTracking() on dl.DepositDateId equals dd.DateKey
                                join cp in _m3pactContext.ClientPayer on dl.ClientPayerId equals cp.ClientPayerId
                                where cp.ClientId == clientID && dl.RecordStatus == DomainConstants.RecordStatusActive
                                orderby dd.DateKey descending
                                select dd.Date.Date).Distinct().ToList();
                return depositDates;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get the deposit log amount on week ends.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="weekEndList"></param>
        /// <returns></returns>
        public decimal? GetDepositLogAmountOnWeekEnds(string clientCode, DateTime? startDate, DateTime endDate)
        {
            try
            {
                decimal? totalDepositsOnWeekEnds = (from c in _m3pactContext.Client
                                                    join cp in _m3pactContext.ClientPayer
                                                    on c.ClientId equals cp.ClientId
                                                    join dl in _m3pactContext.DepositLog
                                                    on cp.ClientPayerId equals dl.ClientPayerId
                                                    join dd in _m3pactContext.DateDimension
                                                    on dl.DepositDateId equals dd.DateKey
                                                    where c.ClientCode == clientCode
                                                    && c.IsActive == DomainConstants.RecordStatusActive
                                                    //&& cp.RecordStatus == DomainConstants.RecordStatusDelete
                                                    && dl.RecordStatus == DomainConstants.RecordStatusActive
                                                    && dd.Date >= startDate
                                                    && dd.Date <= endDate
                                                    && dd.IsWeekend == true
                                                    && dd.IsHoliday == false
                                                    select dl.Amount
                                                  ).Sum();
                return totalDepositsOnWeekEnds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get the projected cash of deposit log amount when number of weeks are given.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="numberOfWeeks"></param>
        /// <returns></returns>

        public List<DepositLogWeekDays> GetProjectedCashOfPreviousWeeks(string clientCode, int month, int year, int numberOfWeeks, DateTime endDate)
        {
            try
            {
                List<DepositLogWeekDays> depositLogWeekDayList = null;
                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, "M3PactConnection");
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    string sql = DomainConstants.GetProjectedCashOfPreviousWeek;
                    using (SqlCommand sqlCmd = new SqlCommand(sql, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@ClientCode", clientCode);
                        sqlCmd.Parameters.AddWithValue("@Month", month);
                        sqlCmd.Parameters.AddWithValue("@Year", year);
                        sqlCmd.Parameters.AddWithValue("@LastNumberOfWeeks", numberOfWeeks);
                        sqlCmd.Parameters.AddWithValue("@EndDate", endDate.Date);
                        sqlConn.Open();
                        SqlDataReader reader = sqlCmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            depositLogWeekDayList = new List<DepositLogWeekDays>();
                            while (reader.Read())
                            {
                                DepositLogWeekDays depositLogWeekDays = new DepositLogWeekDays();
                                depositLogWeekDays.WeekName = (string)reader["WeekName"];
                                depositLogWeekDays.DepositAmount = (decimal)reader["DepositAmount"];
                                depositLogWeekDays.WeekDaysCompleted = (int)reader["WeeksDaysCompleted"];
                                depositLogWeekDays.WeekDaysLeft = (int)reader["WeekDaysLeft"];
                                depositLogWeekDays.DepositStartDate = (DateTime)reader["DepositStartDate"];

                                depositLogWeekDayList.Add(depositLogWeekDays);
                            }
                        }
                    }
                    sqlConn.Close();
                }
                return depositLogWeekDayList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get the Client Payment for a specific month.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public long? GetClientPaymentForMonth(string clientCode, int month, int year)
        {
            try
            {
                long? payments = (from c in _m3pactContext.Client
                                  join ct in _m3pactContext.ClientTarget
                                  on c.ClientId equals ct.ClientId
                                  join m in _m3pactContext.Month
                                  on ct.MonthId equals m.MonthId
                                  join bd in _m3pactContext.BusinessDays
                                  on m.MonthId equals bd.MonthId
                                  where c.ClientCode == clientCode
                                  && m.MonthId == month
                                  && ct.CalendarYear == year
                                  && bd.Year == year
                                  && c.IsActive == DomainConstants.RecordStatusActive
                                  && ct.RecordStatus == DomainConstants.RecordStatusActive
                                  && m.RecordStatus == DomainConstants.RecordStatusActive
                                  && bd.RecordStatus == DomainConstants.RecordStatusActive
                                  select ct.Payments
                                 ).FirstOrDefault();
                return payments;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get the deposit log amount of a client including week ends when start and end date are given.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public decimal? GetDepositLogAmountFromGivenDatesIncludingWeekEnds(string clientCode, int year, int month, DateTime startDate, DateTime endDate)
        {
            try
            {
                decimal? totalDepositsGivenDates = (from c in _m3pactContext.Client
                                                    join cp in _m3pactContext.ClientPayer
                                                    on c.ClientId equals cp.ClientId
                                                    join dl in _m3pactContext.DepositLog
                                                    on cp.ClientPayerId equals dl.ClientPayerId
                                                    join dd in _m3pactContext.DateDimension
                                                    on dl.DepositDateId equals dd.DateKey
                                                    where c.ClientCode == clientCode
                                                    && c.IsActive == DomainConstants.RecordStatusActive
                                                    //&& cp.RecordStatus != DomainConstants.RecordStatusDelete
                                                    && dl.RecordStatus == DomainConstants.RecordStatusActive
                                                    && dd.Date >= startDate
                                                    && dd.Date <= endDate
                                                    select dl.Amount
                                                   ).Sum();
                return totalDepositsGivenDates;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get the projected cash of the last working days.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="numberOfLastWorkingDays"></param>
        /// <returns></returns>
        public List<DepositLogProjectedCash> GetProjectedCashOfLastWorkingDays(string clientCode, int month, int year, int numberOfLastWorkingDays, DateTime? startDate, DateTime endDate)
        {
            try
            {
                List<DepositLogProjectedCash> depositLogProjectedCashList = null;
                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, "M3PactConnection");
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    string sql = DomainConstants.GetProjectedCashOfLastWorkingDays;
                    using (SqlCommand sqlCmd = new SqlCommand(sql, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@ClientCode", clientCode);
                        sqlCmd.Parameters.AddWithValue("@Year", year);
                        sqlCmd.Parameters.AddWithValue("@Month", month);
                        sqlCmd.Parameters.AddWithValue("@NumberOfLastWorkingDays", numberOfLastWorkingDays);
                        sqlCmd.Parameters.AddWithValue("@StartDate", startDate);
                        sqlCmd.Parameters.AddWithValue("@EndDate", endDate);
                        sqlConn.Open();
                        SqlDataReader reader = sqlCmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            depositLogProjectedCashList = new List<DepositLogProjectedCash>();
                            while (reader.Read())
                            {
                                DepositLogProjectedCash depositLogProjectedCash = new DepositLogProjectedCash();
                                depositLogProjectedCash.LastWorkingDayNumber = (int)reader["LastWorkingDayNumber"];
                                depositLogProjectedCash.ProjectedCash = (decimal)reader["ProjectedCash"];
                                depositLogProjectedCashList.Add(depositLogProjectedCash);
                            }
                        }
                        else
                        {
                            sqlConn.Close();
                            return depositLogProjectedCashList;
                        }

                    }
                    sqlConn.Close();
                }
                return depositLogProjectedCashList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get the number of business days between start date and end date.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int GetNumberOfBusinessDaysFromStartDateAndEndDate(DateTime? startDate, DateTime endDate)
        {
            try
            {
                int numberOfBusinessDays = (from dd in _m3pactContext.DateDimension
                                            where dd.Date >= startDate
                                            && dd.Date <= endDate
                                            && dd.IsWeekend != true
                                            && dd.IsHoliday != true
                                            select dd.Date).Count();
                return numberOfBusinessDays;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// To get the deposit log start date and number of deposit dates from the deposit log table.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="lastNumberOfDays"></param>
        /// <returns></returns>
        public ClientDepositLogInfo GetDepositLogStartDateAndNumberOfDepositDates(string clientCode, int lastNumberOfDays, DateTime endDate)
        {
            try
            {
                ClientDepositLogInfo clientDepositLogInfo = new ClientDepositLogInfo();
                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, "M3PactConnection");
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    string sql = DomainConstants.GetDepositLogStartDateAndNumberOfDepositDates;
                    using (SqlCommand sqlCmd = new SqlCommand(sql, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@ClientCode", clientCode);
                        sqlCmd.Parameters.AddWithValue("@LastNumberOfDays", lastNumberOfDays);
                        sqlCmd.Parameters.AddWithValue("@EndDate", endDate.Date);
                        sqlConn.Open();
                        SqlDataReader reader = sqlCmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["DepositStartDate"] != DBNull.Value)
                            {
                                clientDepositLogInfo.DepositStartDate = Convert.ToDateTime(reader["DepositStartDate"]);
                            }
                            clientDepositLogInfo.NumberOfDepositDates = Convert.ToInt32(reader["NumberOfDepositDates"]);
                        }
                    }
                    sqlConn.Close();
                }
                return clientDepositLogInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get the number of deposit days for a client.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public int GetNumberOfDepositDaysOfClient(string clientCode, DateTime endDate)
        {
            try
            {
                int numberOfDepositDays = (from c in _m3pactContext.Client
                                           join cp in _m3pactContext.ClientPayer
                                           on c.ClientId equals cp.ClientId
                                           join dl in _m3pactContext.DepositLog
                                           on cp.ClientPayerId equals dl.ClientPayerId
                                           join dd in _m3pactContext.DateDimension
                                           on dl.DepositDateId equals dd.DateKey
                                           where c.ClientCode == clientCode
                                           && dd.IsHoliday == false
                                           && dd.IsWeekend == false
                                           && c.IsActive == DomainConstants.RecordStatusActive
                                           //&& cp.RecordStatus != DomainConstants.RecordStatusDelete
                                           && dl.RecordStatus == DomainConstants.RecordStatusActive
                                           && dd.Date <= endDate
                                           select dd.Date).Distinct().Count();
                return numberOfDepositDays;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get the number of deposit weeks for client
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public int GetNumberOfDepositWeeksForClient(string clientCode, DateTime endDate)
        {
            try
            {
                int numberOfDepositWeeksForClient = 0;
                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, "M3PactConnection");
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    string sql = DomainConstants.GetNumberOfDepositWeeksForClient; ;
                    using (SqlCommand sqlCmd = new SqlCommand(sql, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@ClientCode", clientCode);
                        sqlCmd.Parameters.AddWithValue("@EndDate", endDate.Date);
                        sqlConn.Open();
                        SqlDataReader reader = sqlCmd.ExecuteReader();
                        while (reader.Read())
                        {
                            numberOfDepositWeeksForClient = (int)(reader["NumberOfDepositWeeksForClient"]);
                        }
                    }
                    sqlConn.Close();
                }
                return numberOfDepositWeeksForClient;
            }
            catch (Exception ex)
            {
                throw ex;
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
                DateTime endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                DateTime startDate = new DateTime(year, month, 1);
                int numberOfDepositDaysForMonth = (from c in _m3pactContext.Client
                                                   join cp in _m3pactContext.ClientPayer
                                                   on c.ClientId equals cp.ClientId
                                                   join dl in _m3pactContext.DepositLog
                                                   on cp.ClientPayerId equals dl.ClientPayerId
                                                   join dd in _m3pactContext.DateDimension
                                                   on dl.DepositDateId equals dd.DateKey
                                                   where c.ClientCode == clientCode
                                                   && dd.IsHoliday == false
                                                   && dd.IsWeekend == false
                                                   && dd.Date >= startDate
                                                   && dd.Date <= endDate
                                                   && c.IsActive == DomainConstants.RecordStatusActive
                                                   //&& cp.RecordStatus == DomainConstants.RecordStatusActive
                                                   && dl.RecordStatus == DomainConstants.RecordStatusActive
                                                   select dd.Date).Distinct().Count();
                return numberOfDepositDaysForMonth;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To save the last entered business days or weeks for a client.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="lastEnteredBusiessDaysOrWeeks"></param>
        /// <param name="attributeCode"></param>
        /// <returns></returns>
        public bool SaveLastEnteredBusinessDaysOrWeeks(string clientCode, int lastEnteredBusiessDaysOrWeeks, string attributeCode)
        {
            try
            {
                UserContext user = UserHelper.getUserContext();
                int clientId = _m3pactContext.Client.Where(p => p.ClientCode == clientCode && p.IsActive == DomainConstants.RecordStatusActive).Select(p => p.ClientId).FirstOrDefault();
                int attributeId = _m3pactContext.Attribute.Where(p => p.AttributeCode == attributeCode && p.RecordStatus == DomainConstants.RecordStatusActive).Select(p => p.AttributeId).FirstOrDefault();
                ClientConfig clientConfig = _m3pactContext.ClientConfig.Where(p => p.ClientId == clientId && p.AttributeId == attributeId && p.RecordStatus == DomainConstants.RecordStatusActive).FirstOrDefault();
                ClientConfig clientConfigObject = new ClientConfig();
                if (clientConfig == null)
                {
                    clientConfigObject = FormClientConfig(clientId, attributeId, lastEnteredBusiessDaysOrWeeks, user);
                    _m3pactContext.ClientConfig.Add(clientConfigObject);
                }
                else
                {
                    clientConfig.RecordStatus = DomainConstants.RecordStatusInactive;
                    clientConfig.ModifiedBy = user.UserId;
                    clientConfig.ModifiedDate = DateTime.Now;
                    _m3pactContext.ClientConfig.Update(clientConfig);
                    clientConfigObject = FormClientConfig(clientId, attributeId, lastEnteredBusiessDaysOrWeeks, user);
                    _m3pactContext.ClientConfig.Add(clientConfigObject);
                }
                int lastBusinessDaysAttributeId = _m3pactContext.Attribute.Where(a => a.AttributeCode == DomainConstants.LastEnteredBusinessDays).Select(a => a.AttributeId).FirstOrDefault();
                if (lastBusinessDaysAttributeId == attributeId)
                {
                    DepositLogProjectionViewModel depositLogProjectionViewModel = GetProjectedCashForAClient(clientId, lastEnteredBusiessDaysOrWeeks, DateTime.Today.Month, DateTime.Today.Year);
                    if (depositLogProjectionViewModel != null)
                    {
                        SaveProjectedCashOfAClient(clientId, depositLogProjectionViewModel);
                    }
                }
                _m3pactContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Form client config object.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="attributeId"></param>
        /// <param name="lastEnteredBusiessDaysOrWeeks"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public ClientConfig FormClientConfig(int clientId, int attributeId, int lastEnteredBusiessDaysOrWeeks, UserContext user)
        {
            try
            {
                ClientConfig clientConfigObject = new ClientConfig();
                clientConfigObject.ClientId = clientId;
                clientConfigObject.AttributeId = attributeId;
                clientConfigObject.AttributeValue = lastEnteredBusiessDaysOrWeeks.ToString();
                clientConfigObject.RecordStatus = DomainConstants.RecordStatusActive;
                clientConfigObject.CreatedBy = user.UserId;
                clientConfigObject.CreatedDate = DateTime.Now;
                clientConfigObject.ModifiedBy = user.UserId;
                clientConfigObject.ModifiedDate = DateTime.Now;
                return clientConfigObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get the saved last number of business days or weeks for a client.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetSavedLastNumberOfBusinessDaysOrWeeks(string clientCode)
        {
            try
            {
                Dictionary<string, string> attributeValueList = new Dictionary<string, string>();
                int clientId = _m3pactContext.Client.Where(p => p.ClientCode == clientCode && p.IsActive == DomainConstants.RecordStatusActive).Select(p => p.ClientId).FirstOrDefault();
                int busiessDaysAttributeId = _m3pactContext.Attribute.Where(p => p.AttributeCode == DomainConstants.LastEnteredBusinessDays && p.RecordStatus == DomainConstants.RecordStatusActive).Select(p => p.AttributeId).FirstOrDefault();
                string businessDaysAttributeValue = _m3pactContext.ClientConfig.Where(p => p.ClientId == clientId && p.AttributeId == busiessDaysAttributeId && p.RecordStatus == DomainConstants.RecordStatusActive).Select(p => p.AttributeValue).FirstOrDefault();
                if (businessDaysAttributeValue == null)
                {
                    // GET THE VALUE FROM ADMIN CONFIG TABLE.
                    businessDaysAttributeValue = _m3pactContext.AdminConfigValues.Where(p => p.AttributeId == busiessDaysAttributeId && p.RecordStatus == DomainConstants.RecordStatusActive).Select(p => p.AttributeValue).FirstOrDefault();
                }
                int weeksAttributeId = _m3pactContext.Attribute.Where(p => p.AttributeCode == DomainConstants.LastEnteredWeeks && p.RecordStatus == DomainConstants.RecordStatusActive).Select(p => p.AttributeId).FirstOrDefault();
                string weeksAttributeValue = _m3pactContext.ClientConfig.Where(p => p.ClientId == clientId && p.AttributeId == weeksAttributeId && p.RecordStatus == DomainConstants.RecordStatusActive).Select(p => p.AttributeValue).FirstOrDefault();
                if (weeksAttributeValue == null)
                {
                    // GET THE VALUE FROM ADMIN CONFIG TABLE.
                    weeksAttributeValue = _m3pactContext.AdminConfigValues.Where(p => p.AttributeId == weeksAttributeId && p.RecordStatus == DomainConstants.RecordStatusActive).Select(p => p.AttributeValue).FirstOrDefault();
                }
                attributeValueList.Add(DomainConstants.LastEnteredBusinessDays, businessDaysAttributeValue);
                attributeValueList.Add(DomainConstants.LastEnteredWeeks, weeksAttributeValue);
                return attributeValueList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To save deposit log sum of a client.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="depositDate"></param>
        public void SaveDepositLogSumOfaClient(string clientCode, DateTime depositDate)
        {
            try
            {
                int clientId = _m3pactContext.Client.Where(c => c.ClientCode == clientCode && c.IsActive == DomainConstants.RecordStatusActive).Select(c => c.ClientId)?.FirstOrDefault() ?? 0;
                if (clientId != 0)
                {
                    decimal? depositsTotal = GetDepositLogOfCurrentMonth(clientCode, depositDate.Month, depositDate.Year);
                    string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(depositDate.Month);
                    int monthId = _m3pactContext.Month.Where(m => m.MonthName.ToLower() == month.ToLower()).Select(m => m.MonthId).FirstOrDefault();
                    DepositLogMonthlyDetails depositLogMonthlyDetails = _m3pactContext.DepositLogMonthlyDetails.Where(cm => cm.ClientId == clientId && cm.MonthId == monthId && cm.Year == depositDate.Year)?.FirstOrDefault() ?? null;
                    if (depositLogMonthlyDetails != null)
                    {
                        depositLogMonthlyDetails.TotalDepositAmount = depositsTotal;
                        depositLogMonthlyDetails.ModifiedBy = userContext.UserId;
                        depositLogMonthlyDetails.ModifiedDate = DateTime.Now;
                        _m3pactContext.DepositLogMonthlyDetails.Update(depositLogMonthlyDetails);
                    }
                    else
                    {
                        depositLogMonthlyDetails = new DepositLogMonthlyDetails();
                        depositLogMonthlyDetails.ClientId = clientId;
                        depositLogMonthlyDetails.CreatedBy = userContext.UserId;
                        depositLogMonthlyDetails.CreatedDate = DateTime.Now;
                        depositLogMonthlyDetails.ModifiedBy = userContext.UserId;
                        depositLogMonthlyDetails.ModifiedDate = DateTime.Now;
                        depositLogMonthlyDetails.MonthId = depositDate.Month;
                        depositLogMonthlyDetails.MonthStatus = DomainConstants.Pending;
                        depositLogMonthlyDetails.RecordStatus = DomainConstants.RecordStatusActive;
                        depositLogMonthlyDetails.TotalDepositAmount = depositsTotal;
                        depositLogMonthlyDetails.Year = depositDate.Year;
                        _m3pactContext.DepositLogMonthlyDetails.Add(depositLogMonthlyDetails);
                    }
                    _m3pactContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To insert deposit log sum for all client.
        /// </summary>
        /// <param name="year"></param>
        public void InsertDepositlogSumForAllClient(int year)
        {
            try
            {
                List<Client> clients = _m3pactContext.Client.ToList();
                foreach (Client client in clients)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        decimal? depositsTotal = GetDepositLogOfCurrentMonth(client.ClientCode, i, year);
                        string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i);
                        int monthId = _m3pactContext.Month.Where(m => m.MonthName.ToLower() == month.ToLower()).Select(m => m.MonthId).FirstOrDefault();
                        DepositLogMonthlyDetails depositLogMonthlyDetails = _m3pactContext.DepositLogMonthlyDetails.Where(cm => cm.ClientId == client.ClientId && cm.MonthId == monthId && cm.Year == year)?.FirstOrDefault() ?? null;
                        if (depositLogMonthlyDetails != null)
                        {
                            depositLogMonthlyDetails.TotalDepositAmount = depositsTotal;
                            depositLogMonthlyDetails.ModifiedBy = userContext.UserId;
                            depositLogMonthlyDetails.ModifiedDate = DateTime.Now;
                            _m3pactContext.DepositLogMonthlyDetails.Update(depositLogMonthlyDetails);
                        }
                        else
                        {
                            depositLogMonthlyDetails = new DepositLogMonthlyDetails();
                            depositLogMonthlyDetails.ClientId = client.ClientId;
                            depositLogMonthlyDetails.CreatedBy = userContext.UserId;
                            depositLogMonthlyDetails.CreatedDate = DateTime.Now;
                            depositLogMonthlyDetails.ModifiedBy = userContext.UserId;
                            depositLogMonthlyDetails.ModifiedDate = DateTime.Now;
                            depositLogMonthlyDetails.MonthId = i;
                            depositLogMonthlyDetails.MonthStatus = DomainConstants.Pending;
                            depositLogMonthlyDetails.RecordStatus = DomainConstants.RecordStatusActive;
                            depositLogMonthlyDetails.TotalDepositAmount = depositsTotal;
                            depositLogMonthlyDetails.Year = year;
                            _m3pactContext.DepositLogMonthlyDetails.Add(depositLogMonthlyDetails);
                        }
                        _m3pactContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Close or Reopen a Month in a year for a client
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="monthID"></param>
        /// <param name="year"></param>
        /// <param name="isCloseMonth"></param>
        /// <returns></returns>
        public bool OpenOrCloseMonthForAClient(string clientCode, int monthID, int year, bool isCloseMonth)
        {
            try
            {
                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, "M3PactConnection");
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    string sql = DomainConstants.OpenOrCloseMonth;
                    using (SqlCommand sqlCmd = new SqlCommand(sql, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@ClientCode", clientCode);
                        sqlCmd.Parameters.AddWithValue("@MonthID", monthID);
                        sqlCmd.Parameters.AddWithValue("@Year", year);
                        sqlCmd.Parameters.AddWithValue("@IsCloseMonth", isCloseMonth);
                        sqlCmd.Parameters.AddWithValue("@UserID", userContext.UserId);
                        sqlConn.Open();
                        SqlDataReader reader = sqlCmd.ExecuteReader();
                    }
                    sqlConn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get month wise deposits in a year for a client
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<MonthDeposit> GetMonthWiseDepositsInAYearForClient(string clientCode, int year)
        {
            List<MonthDeposit> monthwiseDeposits = new List<MonthDeposit>();
            try
            {
                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, "M3PactConnection");
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    string sql = DomainConstants.GetMonthDepositsOfAYear;
                    using (SqlCommand sqlCmd = new SqlCommand(sql, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@ClientCode", clientCode);
                        sqlCmd.Parameters.AddWithValue("@Year", year);
                        sqlConn.Open();
                        SqlDataReader reader = sqlCmd.ExecuteReader();
                        while (reader.Read())
                        {
                            MonthDeposit monthDeposit = new MonthDeposit();
                            if (reader["MonthID"] != DBNull.Value)
                            {
                                monthDeposit.MonthID = Convert.ToInt32(reader["MonthID"]);
                            }
                            if (reader["MonthName"] != DBNull.Value)
                            {
                                monthDeposit.MonthName = Convert.ToString(reader["MonthName"]);
                            }
                            if (reader["MonthStatus"] != DBNull.Value)
                            {
                                monthDeposit.MonthStatus = Convert.ToString(reader["MonthStatus"]);
                            }
                            if (reader["Payments"] != DBNull.Value)
                            {
                                monthDeposit.Target = Convert.ToInt64(reader["Payments"]);
                            }
                            if (reader["TotalDepositAmount"] != DBNull.Value)
                            {
                                monthDeposit.ActualDeposit = Convert.ToDecimal(reader["TotalDepositAmount"]);
                            }
                            if (reader["MetPercent"] != DBNull.Value)
                            {
                                monthDeposit.MetPercent = Convert.ToDecimal(reader["MetPercent"]);
                            }
                            monthwiseDeposits.Add(monthDeposit);
                        }
                    }
                    sqlConn.Close();
                }
                return monthwiseDeposits;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Check if month is closed or not
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="monthID"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public bool IsMonthOfAYearClosedForAClient(string clientCode, int monthID, int year)
        {
            try
            {
                bool isMonthClosed = (_m3pactContext.DepositLogMonthlyDetails.Include(x => x.Client)
                                        .Where(x => x.Client.ClientCode == clientCode && x.MonthId == monthID && x.Year == year && x.MonthStatus == DomainConstants.Completed))
                                        .Any();
                return isMonthClosed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To save the projected cash of a client for a selected month.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="projectedCash"></param>
        public void SaveProjectedCashOfClientForAGivenMonth(string clientCode , int month , int year , decimal? projectedCash)
        {
            try
            {
                int clientId = _m3pactContext.Client.Where(c => c.ClientCode == clientCode && c.IsActive == DomainConstants.RecordStatusActive).Select(c => c.ClientId)?.FirstOrDefault() ?? 0;
                if (clientId != 0)
                {
                    DepositLogMonthlyDetails depositLogMonthlyDetails = _m3pactContext.DepositLogMonthlyDetails.Where(cm => cm.ClientId == clientId && cm.MonthId == month && cm.Year == year)?.FirstOrDefault() ?? null;
                    if (depositLogMonthlyDetails != null)
                    {
                        depositLogMonthlyDetails.ProjectedCash = projectedCash;
                        depositLogMonthlyDetails.ModifiedBy = userContext.UserId;
                        depositLogMonthlyDetails.ModifiedDate = DateTime.Now;
                        _m3pactContext.DepositLogMonthlyDetails.Update(depositLogMonthlyDetails);
                    }
                    else
                    {
                        depositLogMonthlyDetails = new DepositLogMonthlyDetails();
                        depositLogMonthlyDetails.ClientId = clientId;
                        depositLogMonthlyDetails.CreatedBy = userContext.UserId;
                        depositLogMonthlyDetails.CreatedDate = DateTime.Now;
                        depositLogMonthlyDetails.ModifiedBy = userContext.UserId;
                        depositLogMonthlyDetails.ModifiedDate = DateTime.Now;
                        depositLogMonthlyDetails.MonthId = month;
                        depositLogMonthlyDetails.MonthStatus = DomainConstants.Pending;
                        depositLogMonthlyDetails.RecordStatus = DomainConstants.RecordStatusActive;
                        depositLogMonthlyDetails.Year = year;
                        depositLogMonthlyDetails.ProjectedCash = projectedCash;
                        _m3pactContext.DepositLogMonthlyDetails.Add(depositLogMonthlyDetails);
                    }
                    _m3pactContext.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get minimum deposit year for the client
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public int GetMinimumDepositYear(string clientCode)
        {
            try
            {
                return (from c in _m3pactContext.Client
                        join cp in _m3pactContext.ClientPayer on c.ClientId equals cp.ClientId
                        join dp in _m3pactContext.DepositLog on cp.ClientPayerId equals dp.ClientPayerId
                        join d in _m3pactContext.DateDimension on dp.DepositDateId equals d.DateKey
                        select d.Year
                        ).Min();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get the projected cash of a client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="savedLastNumberOfBusinessDays"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public DepositLogProjectionViewModel GetProjectedCashForAClient(int clientId, int savedLastNumberOfBusinessDays, int month, int year)
        {
            string clientCode = _m3pactContext.Client.Where(c => c.ClientId == clientId).Select(c => c.ClientCode).FirstOrDefault();
            DepositLogSimpleBusinessDays depositLogSimpleBusinessDays = null;
            DepositLogProjectionViewModel depositLogSimpleBusinessDaysViewModel = null;
            ClientDepositLogInfo clientDepositLogInfo = new ClientDepositLogInfo();
            DateTime endDate = DateTime.Today;
            clientDepositLogInfo = GetDepositLogStartDateAndNumberOfDepositDates(clientCode, savedLastNumberOfBusinessDays, endDate);
            if (clientDepositLogInfo.NumberOfDepositDates > 0)
            {
                depositLogSimpleBusinessDays = GetProjectedCashFromSimpleBusinessDays(clientCode, clientDepositLogInfo.NumberOfDepositDates, month, year, clientDepositLogInfo.DepositStartDate, endDate);
                if (depositLogSimpleBusinessDays != null)
                {
                    depositLogSimpleBusinessDaysViewModel = new DepositLogProjectionViewModel();
                    depositLogSimpleBusinessDaysViewModel.Payments = depositLogSimpleBusinessDays.Payments;
                    depositLogSimpleBusinessDays.DepositLogPaymentsOnWeekEnds = depositLogSimpleBusinessDays.DepositLogPaymentsOnWeekEnds != null ? (decimal)depositLogSimpleBusinessDays.DepositLogPaymentsOnWeekEnds : (decimal)0.0;
                    depositLogSimpleBusinessDaysViewModel.DepositLogAmount = depositLogSimpleBusinessDays.DepositLogPaymentsOnWeekDays != null ? (decimal)depositLogSimpleBusinessDays.DepositLogPaymentsOnWeekDays : (decimal)0.0;
                    depositLogSimpleBusinessDaysViewModel.DepositLogAmount = ((depositLogSimpleBusinessDaysViewModel.DepositLogAmount / clientDepositLogInfo.NumberOfDepositDates) * depositLogSimpleBusinessDays.BusinessDays) + depositLogSimpleBusinessDays.DepositLogPaymentsOnWeekEnds;
                    depositLogSimpleBusinessDaysViewModel.NumberOfLastWorkingDaysOrWeeks = clientDepositLogInfo.NumberOfDepositDates;
                }
            }
            return depositLogSimpleBusinessDaysViewModel;
        }

        /// <summary>
        /// To save the projected cash of a client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="depositLogSimpleBusinessDaysViewModel"></param>
        public void SaveProjectedCashOfAClient(int clientId , DepositLogProjectionViewModel depositLogSimpleBusinessDaysViewModel)
        {
            DepositLogMonthlyDetails depositLogMonthlyDetails = _m3pactContext.DepositLogMonthlyDetails.Where(d => d.ClientId == clientId && d.MonthId == DateTime.Today.Month && d.Year == DateTime.Today.Year && d.RecordStatus == DomainConstants.RecordStatusActive).FirstOrDefault();
            if (depositLogMonthlyDetails != null)
            {
                depositLogMonthlyDetails.ProjectedCash = depositLogSimpleBusinessDaysViewModel.DepositLogAmount;
                depositLogMonthlyDetails.ModifiedBy = userContext.UserId;
                depositLogMonthlyDetails.ModifiedDate = DateTime.Now;
                _m3pactContext.UpdateRange(depositLogMonthlyDetails);
            }
            else
            {
                depositLogMonthlyDetails = new DepositLogMonthlyDetails();
                depositLogMonthlyDetails.ClientId = clientId;
                depositLogMonthlyDetails.CreatedBy = userContext.UserId;
                depositLogMonthlyDetails.CreatedDate = DateTime.Now;
                depositLogMonthlyDetails.ModifiedBy = userContext.UserId;
                depositLogMonthlyDetails.ModifiedDate = DateTime.Now;
                depositLogMonthlyDetails.MonthId = DateTime.Today.Month;
                depositLogMonthlyDetails.MonthStatus = DomainConstants.Pending;
                depositLogMonthlyDetails.RecordStatus = DomainConstants.RecordStatusActive;
                depositLogMonthlyDetails.Year = DateTime.Today.Year;
                depositLogMonthlyDetails.ProjectedCash = depositLogSimpleBusinessDaysViewModel.DepositLogAmount;
                _m3pactContext.DepositLogMonthlyDetails.AddRange(depositLogMonthlyDetails);
            }
        }
        #endregion

    }
}