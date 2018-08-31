using M3Pact.Infrastructure.Interfaces.Business.Admin;
using M3Pact.Infrastructure.Interfaces.Repository.Admin;
using M3Pact.LoggerUtility;
using M3Pact.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace M3Pact.Business.Admin
{
    public class BusinessUnitBusines : IBusinessUnitBusiness
    {
        #region Private Properties
        private IBusinesssUnitRepository _businesssUnitRepository;
        private ILogger _logger;
        #endregion Private Properties

        #region Constructor 
        public BusinessUnitBusines(IBusinesssUnitRepository businesssUnitRepository, ILogger logger)
        {
            _businesssUnitRepository = businesssUnitRepository;
            _logger = logger;
        }
        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// To activate or inactivate business days
        /// </summary>
        /// <param name="businessUnit"></param>
        /// <returns></returns>
        public bool ActiveOrInactiveBusinessUnit(BusinessUnitViewModel businessUnit)
        {
            try
            {
                if (businessUnit != null)
                {
                    BusinessModel.BusinessModels.BusinessUnit businessUnitBusines = new BusinessModel.BusinessModels.BusinessUnit()
                    {
                        BusinessUnitCode = businessUnit.BusinessUnitCode,
                        BusinessUnitName = businessUnit.BusinessUnitName,
                        BusinessUnitDescription = businessUnit.BusinessUnitDescription,
                        Site = new BusinessModel.BusinessModels.Site()
                        {
                            SiteCode = businessUnit.Site.SiteCode,
                            SiteDescription = businessUnit.Site.SiteDescription,
                            SiteId = businessUnit.Site.SiteId,
                            SiteName = businessUnit.Site.SiteName,
                            RecordStatus = businessUnit.Site.RecordStatus
                        }
                    };

                    return _businesssUnitRepository.ActiveOrInactiveBusinessUnit(businessUnitBusines);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return false;
            }

        }

        /// <summary>
        /// To get all the clients associated with bussiness unit
        /// </summary>
        /// <param name="businessUnitId"></param>
        /// <param name="isRecordStatus"></param>
        /// <returns></returns>
        public List<string> GetClientsAssociatedWithBusinessUnit(int businessUnitId, bool isRecordStatus)
        {
            try
            {
                return _businesssUnitRepository.GetClientsAssociatedWithBusinessUnit(businessUnitId, isRecordStatus);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }
        /// <summary>
        /// Returns the Business Units
        /// </summary>
        /// <returns></returns>
        public List<BusinessUnitViewModel> GetBusinessUnits(bool fromClient)
        {
            try
            {
                List<BusinessUnitViewModel> businessUnits = new List<BusinessUnitViewModel>();
                IEnumerable<BusinessModel.BusinessModels.BusinessUnit> businessUnitsDTO = _businesssUnitRepository.GetBusinessUnits();
                foreach (BusinessModel.BusinessModels.BusinessUnit businessUnitDTO in businessUnitsDTO)
                {
                    BusinessUnitViewModel businessUnit = new BusinessUnitViewModel();
                    businessUnit.BusinessUnitCode = businessUnitDTO.BusinessUnitCode;
                    businessUnit.BusinessUnitName = businessUnitDTO.BusinessUnitName;
                    businessUnit.BusinessUnitDescription = businessUnitDTO.BusinessUnitDescription;
                    businessUnit.RecordStatus = businessUnitDTO.RecordStatus;
                    businessUnit.ID = businessUnitDTO.ID;
                    businessUnit.Clients = GetClientsAssociatedWithBusinessUnit(businessUnit.ID, false);
                    businessUnits.Add(businessUnit);
                }
                return fromClient ? businessUnits.OrderBy(b => b.BusinessUnitName).ToList() : businessUnits;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }


        /// <summary>
        /// Saves the Business Units
        /// </summary>
        /// <param name="businessUnits"></param>
        /// <returns></returns>
        public bool SaveBusinessUnits(List<BusinessUnitViewModel> businessUnits)
        {
            try
            {
                List<BusinessModel.BusinessModels.BusinessUnit> businessUnitsDTO = new List<BusinessModel.BusinessModels.BusinessUnit>();
                foreach (BusinessUnitViewModel businessUnit in businessUnits)
                {
                    BusinessModel.BusinessModels.BusinessUnit businessUnitDTO = new BusinessModel.BusinessModels.BusinessUnit();
                    businessUnitDTO.BusinessUnitCode = businessUnit.BusinessUnitCode;
                    businessUnitDTO.BusinessUnitName = businessUnit.BusinessUnitName;
                    businessUnitDTO.BusinessUnitDescription = businessUnit.BusinessUnitDescription;
                    businessUnitDTO.RecordStatus = businessUnit.RecordStatus;
                    businessUnitDTO.ID = businessUnit.ID;
                    businessUnitsDTO.Add(businessUnitDTO);
                }
                return _businesssUnitRepository.SaveBusinessUnits(businessUnitsDTO);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return false;
            }
        }
        #endregion Public Methods
    }
}
