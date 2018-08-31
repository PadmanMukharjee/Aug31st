using M3Pact.BusinessModel.HeatMap;
using M3Pact.DomainModel.DomainModels;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Common;
using M3Pact.Infrastructure.Enums;
using M3Pact.Infrastructure.Interfaces.Repository.HeatMap;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace M3Pact.Repository.HeatMap
{
    public class ClientsHeatMapRepository : IClientsHeatMapRepository
    {
        #region private Properties
        private M3PactContext _m3pactContext;
        private IConfiguration _Configuration { get; }
        #endregion

        #region Constructor
        public ClientsHeatMapRepository(M3PactContext m3PactContext, IConfiguration configuration)
        {
            _m3pactContext = m3PactContext;
            _Configuration = configuration;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Get HeatMap ForClients & HeatMap Questions 
        /// </summary>
        /// <param name="heatMapRequest"></param>
        /// <returns></returns>
        public HeatMapDetails GetHeatMapForClients(ClientsHeatMapRequest heatMapRequest)
        {
            try
            {
                HeatMapDetails heatMapDetails = new HeatMapDetails();
                List<ClientsHeatMapResponse> heatmaps = new List<ClientsHeatMapResponse>();
                List<HeatMapItemTypeDetail> heatmapItems = new List<HeatMapItemTypeDetail>();
                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, "M3PactConnection");
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(DomainConstants.GetHeatMapForApplicableClients, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@EmployeeID", heatMapRequest.EmployeeID);
                        sqlCmd.Parameters.AddWithValue("@BusinessUnitCode", heatMapRequest.BusinessUnitCode);
                        sqlCmd.Parameters.AddWithValue("@SystemCode", heatMapRequest.SystemCode);
                        sqlCmd.Parameters.AddWithValue("@SpecialtyCode", heatMapRequest.SpecialtyCode);

                        sqlConn.Open();
                        SqlDataReader reader = sqlCmd.ExecuteReader();
                        while (reader.Read())
                        {
                            ClientsHeatMapResponse clientsHeatMapResponse = new ClientsHeatMapResponse();
                            if (reader["ClientName"] != DBNull.Value)
                            {
                                clientsHeatMapResponse.ClientName = Convert.ToString(reader["ClientName"]);
                            }
                            if (reader["ClientCode"] != DBNull.Value)
                            {
                                clientsHeatMapResponse.ClientCode = Convert.ToString(reader["ClientCode"]);
                            }
                            if (reader["SpecialityName"] != DBNull.Value)
                            {
                                clientsHeatMapResponse.Specialty = Convert.ToString(reader["SpecialityName"]);
                            }
                            if (reader["SiteAcronym"] != DBNull.Value)
                            {
                                clientsHeatMapResponse.SiteAcronym = Convert.ToString(reader["SiteAcronym"]);
                            }
                            if (reader["BusinessUnitCode"] != DBNull.Value)
                            {
                                clientsHeatMapResponse.BusinessUnitCode = Convert.ToString(reader["BusinessUnitCode"]);
                            }
                            if (reader["SystemCode"] != DBNull.Value)
                            {
                                clientsHeatMapResponse.SystemCode = Convert.ToString(reader["SystemCode"]);
                            }
                            if (reader["LTM"] != DBNull.Value)
                            {
                                clientsHeatMapResponse.Ltm = Convert.ToDecimal(reader["LTM"]);
                            }
                            if (reader["HeatMapItemName"] != DBNull.Value)
                            {
                                clientsHeatMapResponse.HeatMapItemName = Convert.ToString(reader["HeatMapItemName"]);
                            }
                            if (reader["HeatMapItemNameScore"] != DBNull.Value)
                            {
                                clientsHeatMapResponse.HeatMapItemNameDetail.Score = Convert.ToInt32(reader["HeatMapItemNameScore"]);
                            }
                            if (reader["HeatMapItemType"] != DBNull.Value)
                            {
                                clientsHeatMapResponse.HeatMapItemNameDetail.Type = Convert.ToString(reader["HeatMapItemType"]);
                            }
                            if (reader["Risk"] != DBNull.Value)
                            {
                                clientsHeatMapResponse.Risk = Convert.ToInt32(reader["Risk"]);
                            }
                            if (reader["RiskPercentage"] != DBNull.Value)
                            {
                                clientsHeatMapResponse.RiskPercentage = Convert.ToInt32(reader["RiskPercentage"]);
                            }
                            if (reader["Trend"] != DBNull.Value)
                            {
                                clientsHeatMapResponse.Trend = Convert.ToInt32(reader["Trend"]);
                            }
                            if (reader["ChecklistWeeklyDate"] != DBNull.Value)
                            {
                                clientsHeatMapResponse.ChecklistWeeklyDate = Convert.ToDateTime(reader["ChecklistWeeklyDate"]);
                            }
                            if (reader["ChecklistMonthlyDate"] != DBNull.Value)
                            {
                                clientsHeatMapResponse.ChecklistMonthlyDate = Convert.ToDateTime(reader["ChecklistMonthlyDate"]);
                            }
                            if (reader["AlertLevel"] != DBNull.Value)
                            {
                                clientsHeatMapResponse.HeatMapItemNameDetail.AlertLevel = Convert.ToString(reader["AlertLevel"]);
                            }
                            if (reader["ActualValue"] != DBNull.Value)
                            {
                                clientsHeatMapResponse.HeatMapItemNameDetail.ActualValue = Convert.ToString(reader["ActualValue"]);
                            }
                            heatmaps.Add(clientsHeatMapResponse);
                        }
                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                HeatMapItemTypeDetail heatmapItem = new HeatMapItemTypeDetail();
                                if (reader["HeatMapItemName"] != DBNull.Value)
                                {
                                    heatmapItem.HeatMapItemName = Convert.ToString(reader["HeatMapItemName"]);
                                }
                                if (reader["HeatMapItemType"] != DBNull.Value)
                                {
                                    heatmapItem.HeatMapItemType = Convert.ToString(reader["HeatMapItemType"]);
                                }
                                if (reader["HeatMapItemMeasureType"] != DBNull.Value)
                                {
                                    heatmapItem.HeatMapItemMeasureType = Convert.ToString(reader["HeatMapItemMeasureType"]);
                                }
                                heatmapItems.Add(heatmapItem);
                            }
                        }
                        heatMapDetails.ClientsHeatMapList = heatmaps;
                        heatMapDetails.HeatMapItemTypeDetail = heatmapItems;
                    }
                    sqlConn.Close();
                }
                return heatMapDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get client systems of a user.
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public List<DomainModel.DomainModels.System> GetClientSystemsOfUser(List<string> userIds)
        {
            try
            {
                List<DomainModel.DomainModels.System> systemList = new List<DomainModel.DomainModels.System>();
                UserContext userContext = UserHelper.getUserContext();
                if (userContext.Role == RoleType.Admin.ToString())
                {
                    systemList = (from c in _m3pactContext.Client
                                  join s in _m3pactContext.System
                                  on c.SystemId equals s.SystemId
                                  where
                                  c.RecordStatus == DomainConstants.RecordStatusActive
                                  && s.RecordStatus == DomainConstants.RecordStatusActive
                                  select new DomainModel.DomainModels.System()
                                  {
                                      SystemId = s.SystemId,
                                      SystemCode = s.SystemCode,
                                      SystemName = s.SystemName,
                                      SystemDescription = s.SystemDescription
                                  }).Distinct().ToList();
                }
                else
                {
                    systemList = (from u in _m3pactContext.UserLogin
                                  join uc in _m3pactContext.UserClientMap
                                  on u.Id equals uc.UserId
                                  join c in _m3pactContext.Client
                                  on uc.ClientId equals c.ClientId
                                  join s in _m3pactContext.System
                                  on c.SystemId equals s.SystemId
                                  where
                                  userIds.Contains(u.UserId)
                                  && u.RecordStatus == DomainConstants.RecordStatusActive
                                  && uc.RecordStatus == DomainConstants.RecordStatusActive
                                  && c.RecordStatus == DomainConstants.RecordStatusActive
                                  && s.RecordStatus == DomainConstants.RecordStatusActive
                                  select new DomainModel.DomainModels.System()
                                  {
                                      SystemId = s.SystemId,
                                      SystemCode = s.SystemCode,
                                      SystemName = s.SystemName,
                                      SystemDescription = s.SystemDescription
                                  }).Distinct().ToList();
                }
                return systemList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Get client business units of a user.
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public List<BusinessUnit> GetClientBusinessUnitsOfUser(List<string> userIds)
        {
            try
            {
                List<BusinessUnit> businessUnitList = new List<BusinessUnit>();
                UserContext userContext = UserHelper.getUserContext();
                if (userContext.Role == RoleType.Admin.ToString())
                {
                    businessUnitList = (from c in _m3pactContext.Client
                                        join b in _m3pactContext.BusinessUnit
                                        on c.BusinessUnitId equals b.BusinessUnitId
                                        where
                                        c.RecordStatus == DomainConstants.RecordStatusActive
                                        && b.RecordStatus == DomainConstants.RecordStatusActive
                                        select new BusinessUnit()
                                        {
                                            BusinessUnitId = b.BusinessUnitId,
                                            BusinessUnitCode = b.BusinessUnitCode,
                                            BusinessUnitName = b.BusinessUnitName,
                                            BusinessUnitDescription = b.BusinessUnitDescription
                                        }).Distinct().ToList();
                }
                else
                {
                    businessUnitList = (from u in _m3pactContext.UserLogin
                                        join uc in _m3pactContext.UserClientMap
                                        on u.Id equals uc.UserId
                                        join c in _m3pactContext.Client
                                        on uc.ClientId equals c.ClientId
                                        join b in _m3pactContext.BusinessUnit
                                        on c.BusinessUnitId equals b.BusinessUnitId
                                        where
                                        userIds.Contains(u.UserId)
                                        && u.RecordStatus == DomainConstants.RecordStatusActive
                                        && uc.RecordStatus == DomainConstants.RecordStatusActive
                                        && c.RecordStatus == DomainConstants.RecordStatusActive
                                        && b.RecordStatus == DomainConstants.RecordStatusActive
                                        select new BusinessUnit()
                                        {
                                            BusinessUnitId = b.BusinessUnitId,
                                            BusinessUnitCode = b.BusinessUnitCode,
                                            BusinessUnitName = b.BusinessUnitName,
                                            BusinessUnitDescription = b.BusinessUnitDescription
                                        }).Distinct().ToList();
                }
                return businessUnitList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Get client specialities of a user.
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public List<Speciality> GetClientSpecialitiesOfUser(List<string> userIds)
        {
            try
            {
                List<Speciality> specialtyList = new List<Speciality>();
                UserContext userContext = UserHelper.getUserContext();
                if (userContext.Role == RoleType.Admin.ToString())
                {
                    specialtyList = (from c in _m3pactContext.Client
                                     join s in _m3pactContext.Speciality
                                     on c.SpecialityId equals s.SpecialityId
                                     where
                                     c.RecordStatus == DomainConstants.RecordStatusActive
                                     && s.RecordStatus == DomainConstants.RecordStatusActive
                                     select new Speciality()
                                     {
                                         SpecialityId = s.SpecialityId,
                                         SpecialityCode = s.SpecialityCode,
                                         SpecialityName = s.SpecialityName,
                                         SpecialityDescription = s.SpecialityDescription

                                     }).Distinct().ToList();
                }
                else
                {
                    specialtyList = (from u in _m3pactContext.UserLogin
                                     join uc in _m3pactContext.UserClientMap
                                     on u.Id equals uc.UserId
                                     join c in _m3pactContext.Client
                                     on uc.ClientId equals c.ClientId
                                     join s in _m3pactContext.Speciality
                                     on c.SpecialityId equals s.SpecialityId
                                     where
                                     userIds.Contains(u.UserId)
                                     && u.RecordStatus == DomainConstants.RecordStatusActive
                                     && uc.RecordStatus == DomainConstants.RecordStatusActive
                                     && c.RecordStatus == DomainConstants.RecordStatusActive
                                     && s.RecordStatus == DomainConstants.RecordStatusActive
                                     select new Speciality()
                                     {
                                         SpecialityId = s.SpecialityId,
                                         SpecialityCode = s.SpecialityCode,
                                         SpecialityName = s.SpecialityName,
                                         SpecialityDescription = s.SpecialityDescription

                                     }).Distinct().ToList();
                }
                return specialtyList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
