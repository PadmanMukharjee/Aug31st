using M3Pact.BusinessModel.Admin;
using M3Pact.Infrastructure.Interfaces.Business.Admin;
using M3Pact.Infrastructure.Interfaces.Repository.Admin;
using M3Pact.LoggerUtility;
using M3Pact.ViewModel.Admin;
using System;
using System.Collections.Generic;

namespace M3Pact.Business.Admin
{
    public class ConfigurationsBusiness: IConfigurationsBusiness
    {
        #region properties

        private IConfigurationsRepository _configurationsRepository;
        private ILogger _logger;

        #endregion properties

        #region constructor
        public ConfigurationsBusiness(IConfigurationsRepository configurationsRepository,ILogger logger)
        {
            _configurationsRepository = configurationsRepository;
            _logger = logger;
        }
        #endregion constructor

        #region public methods
        /// <summary>
        /// Business method to call repository To get the attributes
        /// </summary>
        /// <returns></returns>
        public List<AttributesViewModel> GetAttributesForConfig()
        {
            try
            {
                List<AttributesBusinessModel> attributes = _configurationsRepository.GetAttributesForConfig();
                return BusinessMapper.AttributesBusinessModelToAttributesViewModel(attributes);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Business Method to call repository to save the configuration
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public bool SaveAttributeValue( AttributesViewModel attribute)
        {
            try
            {
                AttributesBusinessModel attributesBusinessModel = new AttributesBusinessModel()
                {
                    AttributeCode = attribute.AttributeCode,
                    AttributeDescription = attribute.AttributeDescription,
                    AttributeId = attribute.AttributeId,
                    AttributeName = attribute.AttributeName,
                    AttributeType = attribute.AttributeType,
                    AttributeValue = attribute.AttributeValue,
                    Control = attribute.Control,
                    RecordStatus = attribute.RecordStatus,
                };
                _configurationsRepository.SaveAttributeValue(attributesBusinessModel);
                return true;
            }
            catch(Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return false;
            }
        }

        #endregion public methods       
    }
}
