using M3Pact.BusinessModel.Admin;
using M3Pact.DomainModel.DomainModels;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Common;
using M3Pact.Infrastructure.Interfaces.Repository;
using M3Pact.Infrastructure.Interfaces.Repository.Admin;
using M3Pact.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace M3Pact.Repository.Admin
{
    public class ConfigurationRepository : IConfigurationsRepository
    {
        #region  Properties

        private M3PactContext _m3pactContext;
        private UserContext userContext;
        private IDepositLogRepository _depositLogRepository;

        #endregion Properties

        #region Constructor

        public ConfigurationRepository(M3PactContext m3PactContext, IDepositLogRepository depositLogRepository)
        {
            _m3pactContext = m3PactContext;
            userContext = UserHelper.getUserContext();
            _depositLogRepository = depositLogRepository;
        }

        #endregion Constructor

        #region public methods
        /// <summary>
        /// To get the attributes from Database
        /// </summary>
        /// <returns></returns>
        public List<AttributesBusinessModel> GetAttributesForConfig()
        {
            try
            {
                IEnumerable<DomainModel.DomainModels.Attribute> attributes = _m3pactContext.Attribute.Include(a => a.ControlType).Include(a => a.AdminConfigValues)
                                                                        .Where(a => a.RecordStatus == DomainConstants.RecordStatusActive).ToList();
                return RepoToBusinessMapper(attributes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To save the configuration in Database
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public bool SaveAttributeValue(AttributesBusinessModel attribute)
        {
            try
            {
                AdminConfigValues adminConfig = _m3pactContext.AdminConfigValues.Where(a => a.AttributeId == attribute.AttributeId && a.RecordStatus == DomainConstants.RecordStatusActive).FirstOrDefault();
                adminConfig.AttributeValue = attribute.AttributeValue;
                adminConfig.ModifiedBy = userContext.UserId;
                adminConfig.ModifiedDate = DateTime.Now;
                _m3pactContext.Update(adminConfig);

                List<ClientConfig> clientConfigs = _m3pactContext.ClientConfig.Where(cc => cc.AttributeId == attribute.AttributeId && cc.RecordStatus == DomainConstants.RecordStatusActive).ToList();
                clientConfigs.ForEach(cc => cc.RecordStatus = DomainConstants.RecordStatusInactive);
                _m3pactContext.UpdateRange(clientConfigs);

                if (clientConfigs.Count > 0)
                {
                    List<int> clients = _m3pactContext.Client.Select(c => c.ClientId).ToList();
                    List<ClientConfig> newClientConfigs = new List<ClientConfig>();
                    foreach (int client in clients)
                    {
                        ClientConfig clientConfig = new ClientConfig();
                        clientConfig.AttributeId = attribute.AttributeId;
                        clientConfig.AttributeValue = attribute.AttributeValue;
                        clientConfig.ClientId = client;
                        clientConfig.RecordStatus = DomainConstants.RecordStatusActive;
                        clientConfig.CreatedBy = userContext.UserId;
                        clientConfig.CreatedDate = DateTime.Now;
                        clientConfig.ModifiedDate = DateTime.Now;
                        clientConfig.ModifiedBy = userContext.UserId;
                        newClientConfigs.Add(clientConfig);
                    }
                    _m3pactContext.AddRange(newClientConfigs);
                }
                int attributeId = _m3pactContext.Attribute.Where(a => a.AttributeCode == DomainConstants.LastEnteredBusinessDays).Select(a => a.AttributeId).FirstOrDefault();
                if (attributeId == attribute.AttributeId)
                {
                    UpdateProjectedCashForAllClients(attribute);
                }
                _m3pactContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        #endregion

        #region private methods
        /// <summary>
        /// Respository to Business model Mapper
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        private List<AttributesBusinessModel> RepoToBusinessMapper(IEnumerable<DomainModel.DomainModels.Attribute> attributes)
        {
            List<AttributesBusinessModel> attributesBusinessModels = new List<AttributesBusinessModel>();
            foreach (DomainModel.DomainModels.Attribute attr in attributes)
            {
                AttributesBusinessModel attribute = new AttributesBusinessModel();
                attribute.AttributeId = attr.AttributeId;
                attribute.AttributeCode = attr.AttributeCode;
                attribute.AttributeName = attr.AttributeName;
                attribute.AttributeDescription = attr.AttributeDescription;
                attribute.Control = attr.ControlType.ControlName;
                attribute.RecordStatus = attr.RecordStatus;
                attribute.AttributeType = attr.AttributeType;
                attribute.AttributeValue = attr.AdminConfigValues.Where(a => a.AttributeId == attr.AttributeId).Select(a => a.AttributeValue).FirstOrDefault();
                attributesBusinessModels.Add(attribute);
            }
            return attributesBusinessModels;
        }

        /// <summary>
        /// To update the projected cash for all clients when attribute value is changed.
        /// </summary>
        /// <param name="attribute"></param>
        private void UpdateProjectedCashForAllClients(AttributesBusinessModel attribute)
        {
            IEnumerable<Client> clientList = _m3pactContext.Client.ToList();
            foreach (Client client in clientList)
            {
                if (client.IsActive == DomainConstants.RecordStatusActive)
                {
                    DepositLogProjectionViewModel depositLogSimpleBusinessDaysViewModel = new DepositLogProjectionViewModel();
                    depositLogSimpleBusinessDaysViewModel = _depositLogRepository.GetProjectedCashForAClient(client.ClientId, Convert.ToInt32(attribute.AttributeValue), DateTime.Today.Month, DateTime.Today.Year);
                    if (depositLogSimpleBusinessDaysViewModel != null)
                    {
                        if (depositLogSimpleBusinessDaysViewModel.Payments != null || depositLogSimpleBusinessDaysViewModel.NumberOfLastWorkingDaysOrWeeks > 0)
                        {
                            _depositLogRepository.SaveProjectedCashOfAClient(client.ClientId, depositLogSimpleBusinessDaysViewModel);
                        }
                    }
                }
            }
        }
        #endregion
    }
}
