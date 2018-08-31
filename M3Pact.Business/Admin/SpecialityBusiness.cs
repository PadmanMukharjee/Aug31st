using M3Pact.BusinessModel.BusinessModels;
using M3Pact.Infrastructure.Interfaces.Business.Admin;
using M3Pact.Infrastructure.Interfaces.Repository.Admin;
using M3Pact.LoggerUtility;
using M3Pact.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace M3Pact.Business.Admin
{
    public class SpecialityBusiness : ISpecialityBusiness
    {

        #region private variables

        private ISpecialityRespository _specialityRespository;
        private ILogger _logger;

        #endregion private variables

        #region constructor
        public SpecialityBusiness(ISpecialityRespository specialityRespository, ILogger logger)
        {
            _specialityRespository = specialityRespository;
            _logger = logger;
        }

        #endregion constructor


        #region public Methods

        /// <summary>
        /// To activate or inactivate sprciality
        /// </summary>
        /// <param name="speciality"></param>
        public bool ActiveOrInactiveSpecialities(SpecialityViewModel speciality)
        {
            try
            {
                Speciality sp = new Speciality();
                sp.ID = speciality.ID;
                sp.SpecialityCode = speciality.SpecialityCode;
                return _specialityRespository.ActiveOrInactiveSpecialities(sp);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return false;
            }
        }

        /// <summary>
        ///  To get tha clients associalted with Specialty
        /// </summary>
        /// <param name="specialtyId"></param>
        /// <param name="isRecordStatus"></param>
        /// <returns></returns>
        public List<string> GetClientsAssociatedWithSpecialty(int specialtyId, bool isRecordStatus = true)
        {
            try
            {
                return _specialityRespository.GetClientsAssociatedWithSpecialty(specialtyId, isRecordStatus);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Returns the specialities
        /// </summary>
        /// <param name="fromClient"></param>
        /// <returns></returns>
        public List<SpecialityViewModel> GetSpecialities(bool fromClient)
        {
            try
            {
                List<SpecialityViewModel> specialities = new List<SpecialityViewModel>();
                List<BusinessModel.BusinessModels.Speciality> specialitiesDTO = _specialityRespository.GetSpecialities();
                foreach (BusinessModel.BusinessModels.Speciality specialityDTO in specialitiesDTO)
                {
                    SpecialityViewModel speciality = new SpecialityViewModel();
                    speciality.SpecialityCode = specialityDTO.SpecialityCode;
                    speciality.SpecialityName = specialityDTO.SpecialityName;
                    speciality.SpecialityDescription = specialityDTO.SpecialityDescription;
                    speciality.RecordStatus = specialityDTO.RecordStatus;
                    speciality.ID = specialityDTO.ID;
                    speciality.Clients = GetClientsAssociatedWithSpecialty(speciality.ID, false);

                    specialities.Add(speciality);
                }
                return fromClient ? specialities.OrderBy(s => s.SpecialityName).ToList() : specialities;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Saves the Specilaities
        /// </summary>
        /// <param name="specialities"></param>
        /// <returns></returns>
        public bool SaveSpecialities(List<SpecialityViewModel> specialities)
        {
            try
            {
                List<BusinessModel.BusinessModels.Speciality> specialitiesDTO = new List<BusinessModel.BusinessModels.Speciality>();
                foreach (SpecialityViewModel speciality in specialities)
                {
                    Speciality specialityDTO = new BusinessModel.BusinessModels.Speciality();
                    specialityDTO.SpecialityCode = speciality.SpecialityCode;
                    specialityDTO.SpecialityName = speciality.SpecialityName;
                    specialityDTO.SpecialityDescription = speciality.SpecialityDescription;
                    specialityDTO.RecordStatus = speciality.RecordStatus;
                    specialityDTO.ID = speciality.ID;
                    specialitiesDTO.Add(specialityDTO);
                }
                return _specialityRespository.SaveSpecialities(specialitiesDTO);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return false;
            }
        }

        #endregion public Methods
    }
}
