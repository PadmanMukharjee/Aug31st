using M3Pact.DomainModel.DomainModels;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Common;
using M3Pact.Infrastructure.Interfaces.Repository.Admin;
using System;
using System.Collections.Generic;
using System.Linq;

namespace M3Pact.Repository.Admin
{
    public class SpecialityRepository : ISpecialityRespository
    {
        #region private Properties

        private M3PactContext _m3PactContext;
        private UserContext userContext;

        #endregion private Properties

        #region Constructor
        public SpecialityRepository(M3PactContext m3PactContext)
        {
            _m3PactContext = m3PactContext;
            userContext = UserHelper.getUserContext();
        }
        #endregion Constructor


        #region public Methods

        /// <summary>
        /// To activate or inactivate Speciality
        /// </summary>
        /// <param name="speciality"></param>
        public bool ActiveOrInactiveSpecialities(BusinessModel.BusinessModels.Speciality speciality)
        {
            try
            {
                DomainModel.DomainModels.Speciality sp = _m3PactContext.Speciality.Where(s => s.SpecialityCode == speciality.SpecialityCode).FirstOrDefault();
                if (sp != null)
                {
                    if (sp.RecordStatus == DomainConstants.RecordStatusActive)
                    {
                        sp.RecordStatus = DomainConstants.RecordStatusInactive;
                    }
                    else if (sp.RecordStatus == DomainConstants.RecordStatusInactive)
                    {
                        sp.RecordStatus = DomainConstants.RecordStatusActive;
                    }
                    _m3PactContext.Update(sp);
                    _m3PactContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// To get the clients associated with specialty
        /// </summary>
        /// <param name="specialtyId"></param>
        /// <param name="isRecordStatus"></param>
        /// <returns></returns>
        public List<string> GetClientsAssociatedWithSpecialty(int specialtyId, bool isRecordStatus)
        {
            try
            {
                if (isRecordStatus)
                {
                    return (from c in _m3PactContext.Client
                            where c.SpecialityId == specialtyId
                            && c.RecordStatus == DomainConstants.RecordStatusActive
                            orderby c.Name
                            select (c.ClientCode + '-' + c.Name))?.ToList();
                }
                else
                {
                    return (from c in _m3PactContext.Client
                            where c.SpecialityId == specialtyId
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
        /// Returns the Specialities
        /// </summary>
        /// <returns></returns>
        public List<BusinessModel.BusinessModels.Speciality> GetSpecialities()
        {
            try
            {
                List<BusinessModel.BusinessModels.Speciality> specialitiesDTO = new List<BusinessModel.BusinessModels.Speciality>();

                IEnumerable<DomainModel.DomainModels.Speciality> specialities = _m3PactContext.Speciality;
                if (specialities != null && specialities.Count() > 0)
                {
                    foreach (DomainModel.DomainModels.Speciality speciality in specialities)
                    {
                        BusinessModel.BusinessModels.Speciality specialityDTO = new BusinessModel.BusinessModels.Speciality();
                        specialityDTO.SpecialityCode = speciality.SpecialityCode;
                        specialityDTO.SpecialityCode = speciality.SpecialityCode;
                        specialityDTO.SpecialityName = speciality.SpecialityName;
                        specialityDTO.SpecialityDescription = speciality.SpecialityDescription;
                        specialityDTO.RecordStatus = speciality.RecordStatus;
                        specialityDTO.ID = speciality.SpecialityId;

                        specialitiesDTO.Add(specialityDTO);
                    }
                }
                return specialitiesDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Saves the Specialities
        /// </summary>
        /// <param name="specialitiesDTO"></param>
        /// <returns></returns>
        public bool SaveSpecialities(List<BusinessModel.BusinessModels.Speciality> specialitiesDTO)
        {
            try
            {
                List<DomainModel.DomainModels.Speciality> specialities = new List<DomainModel.DomainModels.Speciality>();
                List<DomainModel.DomainModels.Speciality> specialitiesUpdated = new List<DomainModel.DomainModels.Speciality>();
                DomainModel.DomainModels.Speciality specialityModel;

                foreach (BusinessModel.BusinessModels.Speciality specialityDTO in specialitiesDTO)
                {
                    DomainModel.DomainModels.Speciality speciality = _m3PactContext.Speciality.FirstOrDefault(x => x.SpecialityId == specialityDTO.ID);
                    if (speciality != null)
                    {
                        specialityModel = speciality;
                        speciality.ModifiedDate = DateTime.UtcNow;
                        speciality.ModifiedBy = userContext.UserId;
                        specialitiesUpdated.Add(specialityModel);
                    }
                    else
                    {
                        specialityModel = new DomainModel.DomainModels.Speciality();
                        specialityModel.CreatedBy = userContext.UserId;
                        specialityModel.ModifiedBy = userContext.UserId;
                        specialityModel.CreatedDate = DateTime.UtcNow; ;
                        specialityModel.ModifiedDate = DateTime.UtcNow;
                        specialities.Add(specialityModel);
                    }

                    specialityModel.SpecialityCode = specialityDTO.SpecialityCode;
                    specialityModel.SpecialityName = specialityDTO.SpecialityName;
                    specialityModel.SpecialityDescription = specialityDTO.SpecialityDescription;
                    specialityModel.RecordStatus = specialityDTO.RecordStatus;
                }

                if (specialitiesUpdated.Count > 0)
                {
                    _m3PactContext.Speciality.UpdateRange(specialitiesUpdated);
                }
                if (specialities.Count > 0)
                {
                    _m3PactContext.Speciality.AddRange(specialities);
                }
                if (specialitiesUpdated.Count > 0 || specialities.Count > 0)
                {
                    _m3PactContext.SaveChanges();
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
