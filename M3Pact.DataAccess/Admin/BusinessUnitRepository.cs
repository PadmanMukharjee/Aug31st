using M3Pact.DomainModel.DomainModels;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Common;
using M3Pact.Infrastructure.Interfaces.Repository.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
namespace M3Pact.Repository.Admin
{
    public class BusinessUnitRepository : IBusinesssUnitRepository
    {
        #region private Properties

        private M3PactContext _m3pactContext;
        private UserContext userContext;

        #endregion private Properties

        #region Constructor
        public BusinessUnitRepository(M3PactContext m3PactContext)
        {
            _m3pactContext = m3PactContext;
            userContext = UserHelper.getUserContext();
        }
        #endregion Constructor

        #region public Methods

        /// <summary>
        /// get clients associated with BusinessUnit
        /// </summary>
        /// <param name="businessUnitId"></param>
        /// <param name="isRecordStatus"></param>
        /// <returns></returns>
        public List<string> GetClientsAssociatedWithBusinessUnit(int businessUnitId, bool isRecordStatus = true)
        {
            try
            {

                if (isRecordStatus)
                {
                    return (from c in _m3pactContext.Client
                            where c.BusinessUnitId == businessUnitId
                            && c.IsActive == DomainConstants.RecordStatusActive
                            orderby c.Name
                            select (c.ClientCode + '-' + c.Name))?.ToList();
                }
                else
                {
                    return (from c in _m3pactContext.Client
                            where c.BusinessUnitId == businessUnitId
                            orderby c.Name
                            select (c.ClientCode + '-' + c.Name))?.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To activate or inactivate business unit
        /// </summary>
        /// <param name="businessUnit"></param>
        /// <returns></returns>
        public bool ActiveOrInactiveBusinessUnit(BusinessModel.BusinessModels.BusinessUnit businessUnit)
        {
            try
            {
                DomainModel.DomainModels.BusinessUnit business = _m3pactContext.BusinessUnit.FirstOrDefault(bu => bu.BusinessUnitCode == businessUnit.BusinessUnitCode);

                if (business != null)
                {
                    if (business.RecordStatus == DomainConstants.RecordStatusActive)
                    {
                        business.RecordStatus = DomainConstants.RecordStatusInactive;
                    }
                    else
                    {
                        business.RecordStatus = DomainConstants.RecordStatusActive;
                    }
                    _m3pactContext.Update(business);
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
        /// Returns the BusinessUnit
        /// </summary>
        /// <returns></returns>
        public List<BusinessModel.BusinessModels.BusinessUnit> GetBusinessUnits()
        {
            try
            {
                List<BusinessModel.BusinessModels.BusinessUnit> businessUnitsDTO = new List<BusinessModel.BusinessModels.BusinessUnit>();

                IEnumerable<DomainModel.DomainModels.BusinessUnit> businessUnits = _m3pactContext.BusinessUnit;
                if (businessUnits != null && businessUnits.Count() > 0)
                {
                    foreach (DomainModel.DomainModels.BusinessUnit businessUnit in businessUnits)
                    {
                        BusinessModel.BusinessModels.BusinessUnit businessUnitDTO = new BusinessModel.BusinessModels.BusinessUnit();
                        businessUnitDTO.ID = businessUnit.BusinessUnitId;
                        businessUnitDTO.BusinessUnitCode = businessUnit.BusinessUnitCode;
                        businessUnitDTO.BusinessUnitName = businessUnit.BusinessUnitName;
                        businessUnitDTO.BusinessUnitDescription = businessUnit.BusinessUnitDescription;
                        businessUnitDTO.RecordStatus = businessUnit.RecordStatus;
                        businessUnitsDTO.Add(businessUnitDTO);
                    }
                }
                return businessUnitsDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Saves the BusinessUnit
        /// </summary>
        /// <param name="businessUnitsDTO"></param>
        /// <returns></returns>
        public bool SaveBusinessUnits(List<BusinessModel.BusinessModels.BusinessUnit> businessUnitsDTO)
        {
            try
            {
                List<DomainModel.DomainModels.BusinessUnit> businessUnits = new List<DomainModel.DomainModels.BusinessUnit>();
                List<DomainModel.DomainModels.BusinessUnit> businessUnitsUpdated = new List<DomainModel.DomainModels.BusinessUnit>();
                
                foreach (BusinessModel.BusinessModels.BusinessUnit businessUnitDTO in businessUnitsDTO)
                {
                    DomainModel.DomainModels.BusinessUnit businessUnitModel;
                    DomainModel.DomainModels.BusinessUnit businessUnit = _m3pactContext.BusinessUnit.FirstOrDefault(x => x.BusinessUnitId == businessUnitDTO.ID);
                    if (businessUnit != null)
                    {
                        businessUnitModel = businessUnit;
                        businessUnitsUpdated.Add(businessUnitModel);
                    }
                    else
                    {
                        businessUnitModel = new DomainModel.DomainModels.BusinessUnit();
                        businessUnitModel.CreatedBy = userContext.UserId;
                        businessUnitModel.CreatedDate = DateTime.UtcNow;
                        businessUnits.Add(businessUnitModel);
                    }
                    businessUnitModel.BusinessUnitCode = businessUnitDTO.BusinessUnitCode;
                    businessUnitModel.BusinessUnitName = businessUnitDTO.BusinessUnitName;
                    businessUnitModel.BusinessUnitDescription = businessUnitDTO.BusinessUnitDescription;
                    businessUnitModel.RecordStatus = businessUnitDTO.RecordStatus;
                    businessUnitModel.ModifiedBy = userContext.UserId;
                    businessUnitModel.ModifiedDate = DateTime.UtcNow;
                }
                if (businessUnitsUpdated.Count > 0)
                {
                    _m3pactContext.BusinessUnit.UpdateRange(businessUnitsUpdated);
                }
                if (businessUnits.Count > 0)
                {
                    _m3pactContext.BusinessUnit.AddRange(businessUnits);
                }
                if (businessUnitsUpdated.Count > 0 || businessUnits.Count > 0)
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
        #endregion public Methods

    }
}
