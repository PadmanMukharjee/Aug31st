using M3Pact.DomainModel.DomainModels;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Common;
using M3Pact.Infrastructure.Interfaces.Repository;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace M3Pact.Repository.Payer
{
    public class PayerRepository : IPayerRepository
    {
        #region private Properties
        private M3PactContext _m3pactContext;
        private UserContext userContext;
        private IConfiguration _Configuration { get; }
        #endregion


        #region Constructor
        public PayerRepository(M3PactContext m3PactContext, IConfiguration configuration)
        {
            _m3pactContext = m3PactContext;
            _Configuration = configuration;
            userContext = UserHelper.getUserContext();
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the Payers
        /// </summary>
        /// <returns></returns>
        public List<BusinessModel.BusinessModels.Payer> GetPayers()
        {
            try
            {
                List<BusinessModel.BusinessModels.Payer> payersDTO = new List<BusinessModel.BusinessModels.Payer>();
                IEnumerable<DomainModel.DomainModels.Payer> payers = _m3pactContext.Payer;
                if (payers != null && payers.Count() > 0)
                {
                    payersDTO = ConstructPayerDTO(payers);
                }
                return payersDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Active & UnAssigned Payers to a client
        /// </summary>
        /// <returns></returns>
        public List<BusinessModel.BusinessModels.Payer> GetActivePayersToAssignForClient(string clientCode)
        {
            try
            {
                List<BusinessModel.BusinessModels.Payer> payers = new List<BusinessModel.BusinessModels.Payer>();
                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, "M3PactConnection");
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(DomainConstants.GetActivePayersToAssign, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@ClientCode", clientCode);
                        sqlConn.Open();
                        SqlDataReader dr = sqlCmd.ExecuteReader();
                        while (dr.Read())
                        {
                            BusinessModel.BusinessModels.Payer payer = new BusinessModel.BusinessModels.Payer();
                            if (dr["PayerName"] != DBNull.Value)
                            {
                                payer.PayerCode = dr["PayerCode"].ToString();
                            }
                            if (dr["PayerCode"] != DBNull.Value)
                            {
                                payer.PayerName = dr["PayerName"].ToString();
                            }
                            payers.Add(payer);
                        }
                    }
                    sqlConn.Close();
                }
                return payers?.OrderBy(p => p.PayerName)?.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To construct payers dto.
        /// </summary>
        /// <param name="payers"></param>
        /// <returns></returns>
        public List<BusinessModel.BusinessModels.Payer> ConstructPayerDTO(IEnumerable<DomainModel.DomainModels.Payer> payers)
        {
            try
            {
                List<BusinessModel.BusinessModels.Payer> payersDTO = new List<BusinessModel.BusinessModels.Payer>();
                BusinessModel.BusinessModels.Payer payer;
                foreach (DomainModel.DomainModels.Payer item in payers)
                {
                    payer = new BusinessModel.BusinessModels.Payer();
                    payer.PayerCode = item.PayerCode;
                    payer.PayerName = item.PayerName;
                    payer.PayerDescription = item.PayerDescription;
                    payer.RecordStatus = item.RecordStatus;
                    payer.ID = item.PayerId;
                    payersDTO.Add(payer);
                }
                return payersDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Saves the Payers
        /// </summary>
        /// <param name="payersDTO"></param>
        /// <returns></returns>
        public bool SavePayers(List<BusinessModel.BusinessModels.Payer> payersDTO)
        {
            try
            {
                List<DomainModel.DomainModels.Payer> payers = new List<DomainModel.DomainModels.Payer>();
                List<DomainModel.DomainModels.Payer> payersUpdated = new List<DomainModel.DomainModels.Payer>();
                foreach (BusinessModel.BusinessModels.Payer payerDTO in payersDTO)
                {
                    DomainModel.DomainModels.Payer payerModel;
                    DomainModel.DomainModels.Payer payer = _m3pactContext.Payer.FirstOrDefault(x => x.PayerId == payerDTO.ID);
                    if (payer != null)
                    {
                        payerModel = payer;
                        payersUpdated.Add(payerModel);
                    }
                    else
                    {
                        payerModel = new DomainModel.DomainModels.Payer();
                        payerModel.StartDate = DomainConstants.MinDate;
                        payerModel.EndDate = DateTime.MaxValue.Date;
                        payerModel.CreatedBy = userContext.UserId;
                        payerModel.CreatedDate = DateTime.UtcNow; ;
                        payers.Add(payerModel);
                    }
                    payerModel.PayerCode = payerDTO.PayerCode;
                    payerModel.PayerName = payerDTO.PayerName;
                    payerModel.PayerDescription = payerDTO.PayerDescription;
                    payerModel.RecordStatus = payerDTO.RecordStatus;
                    payerModel.ModifiedBy = userContext.UserId;
                    payerModel.ModifiedDate = DateTime.UtcNow;
                }
                if (payersUpdated.Count > 0)
                {
                    _m3pactContext.UpdateRange(payersUpdated);
                }
                if (payers.Count > 0)
                {
                    _m3pactContext.Payer.AddRange(payers);
                }

                if (payersUpdated.Count > 0 || payers.Count > 0)
                {
                    _m3pactContext.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// To get top payers data.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<BusinessModel.BusinessModels.ClientPayer> GetTopPayersData(string clientCode, int month, int year)
        {
            try
            {
                DateTime startOfMonth = new DateTime(year, month, 1);
                DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
                List<BusinessModel.BusinessModels.ClientPayer> payerDetails = (from c in _m3pactContext.Client
                                                                               join cp in _m3pactContext.ClientPayer
                                                                               on c.ClientId equals cp.ClientId
                                                                               join p in _m3pactContext.Payer
                                                                               on cp.PayerId equals p.PayerId
                                                                               join d in _m3pactContext.DepositLog
                                                                               on cp.ClientPayerId equals d.ClientPayerId
                                                                               join dd in _m3pactContext.DateDimension
                                                                               on d.DepositDateId equals dd.DateKey
                                                                               where c.ClientCode == clientCode
                                                                                  && dd.Date >= startOfMonth
                                                                                  && dd.Date <= endOfMonth
                                                                                  && c.IsActive == DomainConstants.RecordStatusActive
                                                                                 // && cp.RecordStatus == DomainConstants.RecordStatusActive
                                                                                 // && p.RecordStatus == DomainConstants.RecordStatusActive
                                                                                  && d.RecordStatus == DomainConstants.RecordStatusActive
                                                                               group d by new { d.ClientPayer.Payer.PayerCode, d.ClientPayer.Payer.PayerName } into g
                                                                               select new BusinessModel.BusinessModels.ClientPayer()
                                                                               {
                                                                                   Payer = new BusinessModel.BusinessModels.Payer() { PayerCode = g.Key.PayerCode, PayerName = g.Key.PayerName },
                                                                                   DepositLog = new List<BusinessModel.BusinessModels.DepositLog>()
                                    { new BusinessModel.BusinessModels.DepositLog() {Amount=g.Sum(c=>c.Amount) } }

                                                                               }
                                    )?.OrderByDescending(c => c.DepositLog.First().Amount).Take(10).ToList();
                return payerDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save(Insert/Update) Client Payer(s)
        /// </summary>
        /// <param name="clientPayersDTO"></param>
        /// <returns></returns>
        public bool SaveClientPayers(List<BusinessModel.BusinessModels.ClientPayer> clientPayersDTO)
        {
            try
            {
                List<ClientPayer> clientPayers = new List<ClientPayer>();
                List<ClientPayer> clientPayersUpdated = new List<ClientPayer>();
                ClientPayer clientPayerModel;
                UserContext userContext = UserHelper.getUserContext();
                foreach (BusinessModel.BusinessModels.ClientPayer clientPayerDTO in clientPayersDTO)
                {
                    ClientPayer clientPayer = _m3pactContext.ClientPayer.FirstOrDefault(x => x.ClientPayerId == clientPayerDTO.ID);
                    if (clientPayer != null)
                    {
                        clientPayerModel = clientPayer;
                        if (clientPayerDTO.RecordStatus != DomainConstants.RecordStatusDelete || (clientPayerDTO.RecordStatus == DomainConstants.RecordStatusDelete && _m3pactContext.DepositLog.Where(d => d.ClientPayerId == clientPayer.ClientPayerId).Count() == 0))
                        {
                            clientPayersUpdated.Add(clientPayerModel);
                        }
                    }
                    else
                    {
                        clientPayerModel = new ClientPayer();
                        clientPayerModel.StartDate = DomainConstants.MinDate;
                        clientPayerModel.EndDate = DateTime.MaxValue.Date;
                        clientPayerModel.CreatedBy = userContext.UserId;
                        clientPayerModel.CreatedDate = DateTime.Now;
                        clientPayerModel.ClientId = _m3pactContext.Client.Where(c => c.ClientCode == clientPayerDTO.ClientCode).FirstOrDefault().ClientId;
                        clientPayers.Add(clientPayerModel);
                    }

                    clientPayerModel.IsM3feeExempt = clientPayerDTO.IsM3feeExempt;
                    clientPayerModel.PayerId = _m3pactContext.Payer.Where(c => c.PayerCode == clientPayerDTO.Payer.PayerCode).FirstOrDefault().PayerId;
                    clientPayerModel.RecordStatus = clientPayerDTO.RecordStatus;
                    clientPayerModel.ModifiedBy = userContext.UserId;
                    clientPayerModel.ModifiedDate = DateTime.Now;
                }
                if (clientPayersUpdated.Count > 0)
                {
                    _m3pactContext.UpdateRange(clientPayersUpdated);
                }
                if (clientPayers.Count > 0)
                {
                    _m3pactContext.ClientPayer.AddRange(clientPayers);
                }

                if (clientPayersUpdated.Count > 0 || clientPayers.Count > 0)
                {
                    _m3pactContext.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Get Assigned Payers of a client
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public List<BusinessModel.BusinessModels.ClientPayer> GetClientPayers(string clientCode)
        {
            try
            {
                List<BusinessModel.BusinessModels.ClientPayer> clientPayersDTO = new List<BusinessModel.BusinessModels.ClientPayer>();
                int? Flatfee = 0;
                if (clientCode != null && clientCode != string.Empty)
                {
                    Flatfee = _m3pactContext.Client.Where(c => c.ClientCode == clientCode).FirstOrDefault()?.FlatFee ?? null;
                }
                bool Isflatfee = (Flatfee == null || Flatfee == 0) ? false : true;
                List<ClientPayer> clientPayers = (from CP in _m3pactContext.ClientPayer
                                                  join P in _m3pactContext.Payer on CP.PayerId equals P.PayerId
                                                  join C in _m3pactContext.Client on CP.ClientId equals C.ClientId
                                                  where C.ClientCode == clientCode
                                                  && (CP.RecordStatus == DomainConstants.RecordStatusActive ||
                                                  CP.RecordStatus == DomainConstants.RecordStatusInactive)
                                                  orderby CP.CreatedDate descending
                                                  select new ClientPayer()
                                                  {
                                                      ClientPayerId = CP.ClientPayerId,
                                                      IsM3feeExempt = CP.IsM3feeExempt,
                                                      RecordStatus = CP.RecordStatus,
                                                      Payer = P,
                                                  })?.OrderBy(p => p.Payer.PayerName)?.ToList();
                if (clientPayers != null && clientPayers.Count() > 0)
                {
                    if (Isflatfee)
                    {
                        foreach (ClientPayer item in clientPayers)
                        {
                            item.IsM3feeExempt = null;
                        }
                    }
                    clientPayersDTO = ConstructClientPayerDTO(clientPayers);
                }
                return clientPayersDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To activate or inactivate payer and clientpayer
        /// </summary>
        /// <param name="payer"></param>
        public void ActivateOrDeactivatePayer(BusinessModel.BusinessModels.Payer payer)
        {
            try
            {
                UserContext userContext = UserHelper.getUserContext();
                DomainModel.DomainModels.Payer payerModel = (from p in _m3pactContext.Payer
                                                             where p.PayerCode == payer.PayerCode
                                                             select p
                                                        ).ToList().FirstOrDefault();
                if (payerModel != null)
                {
                    if (payerModel.RecordStatus == DomainConstants.RecordStatusActive)
                    {
                        List<ClientPayer> clientPayers = _m3pactContext.ClientPayer.Where(cp => cp.PayerId == payerModel.PayerId && cp.RecordStatus == DomainConstants.RecordStatusActive)?.ToList();
                        foreach (ClientPayer cp in clientPayers)
                        {
                            cp.RecordStatus = DomainConstants.RecordStatusInactive;
                            cp.EndDate = DateTime.Now.Date;
                        }
                        payerModel.RecordStatus = DomainConstants.RecordStatusInactive;
                        payerModel.EndDate = DateTime.Now.Date;
                        payerModel.ModifiedDate = DateTime.Now;
                        payerModel.ModifiedBy = userContext.UserId;
                        _m3pactContext.Update(payerModel);
                        _m3pactContext.UpdateRange(clientPayers);
                    }
                    else
                    {
                        payerModel.RecordStatus = DomainConstants.RecordStatusActive;
                        payerModel.EndDate = DateTime.MaxValue.Date;
                        payerModel.ModifiedDate = DateTime.Now;
                        payerModel.ModifiedBy = userContext.UserId;
                        _m3pactContext.Update(payerModel);
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
        /// To activate or inactivate client payer
        /// </summary>
        /// <param name="clientPayer"></param>
        public void ActivateOrDeactivateClientPayer(BusinessModel.BusinessModels.ClientPayer clientPayerDTO)
        {
            try
            {
                UserContext userContext = UserHelper.getUserContext();
                ClientPayer clientPayer = _m3pactContext.ClientPayer.FirstOrDefault(x => x.ClientPayerId == clientPayerDTO.ID);
                if (clientPayer != null)
                {
                    if (clientPayer.RecordStatus == DomainConstants.RecordStatusActive && clientPayerDTO.RecordStatus == DomainConstants.RecordStatusInactive)
                    {
                        clientPayer.EndDate = DateTime.Now.Date;
                    }
                    else if (clientPayer.RecordStatus == DomainConstants.RecordStatusInactive && clientPayerDTO.RecordStatus == DomainConstants.RecordStatusActive)
                    {
                        clientPayer.EndDate = DateTime.MaxValue.Date;
                    }
                    clientPayer.RecordStatus = clientPayerDTO.RecordStatus;
                    clientPayer.ModifiedDate = DateTime.Now;
                    clientPayer.ModifiedBy = userContext.UserId;

                    _m3pactContext.ClientPayer.Update(clientPayer);
                    _m3pactContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Mapper Methods

        /// <summary>
        /// To get ClientPayer DTO & also frame edit permission for a client payer
        /// </summary>
        /// <param name="clientPayers"></param>
        /// <returns></returns>
        public List<BusinessModel.BusinessModels.ClientPayer> ConstructClientPayerDTO(List<ClientPayer> clientPayers)
        {
            try
            {
                List<BusinessModel.BusinessModels.ClientPayer> clientPayersDTO = new List<BusinessModel.BusinessModels.ClientPayer>();                
                foreach (ClientPayer item in clientPayers)
                {
                    BusinessModel.BusinessModels.ClientPayer clientPayer = new BusinessModel.BusinessModels.ClientPayer();
                    clientPayer.Payer = new BusinessModel.BusinessModels.Payer();
                    clientPayer.Payer.PayerName = item.Payer.PayerName;
                    clientPayer.Payer.PayerCode = item.Payer.PayerCode;
                    clientPayer.ID = item.ClientPayerId;
                    clientPayer.IsM3feeExempt = item.IsM3feeExempt;
                    clientPayer.RecordStatus = item.RecordStatus;
                    clientPayer.IsEditable = item.Payer.RecordStatus == DomainConstants.RecordStatusActive ? true : false;
                    clientPayer.CanDelete = _m3pactContext.DepositLog.Where(dl => dl.ClientPayerId == item.ClientPayerId).Count() > 0 ? false : true;
                    clientPayersDTO.Add(clientPayer);
                }
                return clientPayersDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get the clients assigned to Payer
        /// </summary>
        /// <param name="payerCode"></param>
        /// <param name="isRecordStatus"></param>
        /// <returns></returns>
        public List<string> GetClientsAssignedtoPayer(string payerCode, bool isRecordStatus)
        {
            try
            {
                if (isRecordStatus)
                {
                    return (from c in _m3pactContext.Client
                            join cp in _m3pactContext.ClientPayer
                            on c.ClientId equals cp.ClientId
                            join p in _m3pactContext.Payer
                            on cp.PayerId equals p.PayerId
                            where c.IsActive == DomainConstants.RecordStatusActive
                            && p.RecordStatus == DomainConstants.RecordStatusActive
                            && cp.RecordStatus == DomainConstants.RecordStatusActive
                            && p.PayerCode == payerCode
                            orderby c.Name
                            select (c.ClientCode + '-' + c.Name))?.ToList();
                }
                else
                {
                    return (from c in _m3pactContext.Client
                            join cp in _m3pactContext.ClientPayer
                            on c.ClientId equals cp.ClientId
                            join p in _m3pactContext.Payer
                            on cp.PayerId equals p.PayerId
                            where p.PayerCode == payerCode
                            orderby c.Name
                            select (c.ClientCode + '-' + c.Name))?.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Mapper Methods
    }
}
