using M3Pact.BusinessModel.Admin;
using M3Pact.Infrastructure.Interfaces.Business.Admin;
using M3Pact.Infrastructure.Interfaces.Repository.Admin;
using M3Pact.Infrastructure.Interfaces.Repository.ClientData;
using M3Pact.LoggerUtility;
using M3Pact.ViewModel;
using M3Pact.ViewModel.Admin;
using System;
using System.Collections.Generic;

namespace M3Pact.Business.Admin
{
    public class BusinessDaysBusiness : IBusinessDaysBusiness
    {
        #region Private Properties

        private IBusinessDaysRepository _businessDaysRepository;
        private IClientDataRepository _clientDataRepository;
        ILogger _logger;

        #endregion Private Properties

        #region Constructor 

        public BusinessDaysBusiness(IBusinessDaysRepository businessDaysRepository, IClientDataRepository clientDataRepository, ILogger logger)
        {
            _businessDaysRepository = businessDaysRepository;
            _clientDataRepository = clientDataRepository;
            _logger = logger;
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Returns the Business Days
        /// </summary>
        /// <returns></returns>
        public List<BusinessDaysViewModel> GetBusinessDays()
        {
            try
            {
                List<BusinessDaysViewModel> businessDays = new List<BusinessDaysViewModel>();
                List<BusinessModel.BusinessModels.BusinessDays> businessDaysDTO = _businessDaysRepository.GetBusinessDays();
                foreach (BusinessModel.BusinessModels.BusinessDays businessDay in businessDaysDTO)
                {
                    BusinessDaysViewModel businessDaysModel = new BusinessDaysViewModel();
                    businessDaysModel.Year = businessDay.Year;
                    businessDaysModel.MonthID = businessDay.MonthID;
                    businessDaysModel.Month = businessDay.Month;
                    businessDaysModel.BusinessDays = businessDay.NumberOfBusinessDays;
                    businessDaysModel.RecordStatus = businessDay.RecordStatus;
                    businessDaysModel.ID = businessDay.ID;

                    businessDays.Add(businessDaysModel);
                }
                return businessDays;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Saves the Business Days 
        /// </summary>
        /// <param name="businessDays"></param>
        /// <returns></returns>
        public bool SaveBusinessDays(List<BusinessDaysViewModel> businessDays)
        {
            try
            {
                List<BusinessModel.BusinessModels.BusinessDays> businessDaysDTO = new List<BusinessModel.BusinessModels.BusinessDays>();
                foreach (BusinessDaysViewModel businessDayModel in businessDays)
                {
                    BusinessModel.BusinessModels.BusinessDays businessDayDTO = new BusinessModel.BusinessModels.BusinessDays();
                    businessDayDTO.Year = businessDayModel.Year;
                    businessDayDTO.MonthID = businessDayModel.MonthID;
                    businessDayDTO.NumberOfBusinessDays = businessDayModel.BusinessDays;
                    businessDayDTO.RecordStatus = businessDayModel.RecordStatus;
                    businessDaysDTO.Add(businessDayDTO);
                }
                return _businessDaysRepository.SaveBusinessDays(businessDaysDTO);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return false;
            }
        }

        public List<HolidayViewModel> GetHolidaysOfYear(int year)
        {
            try
            {
                List<HolidayViewModel> listOfholidays = null;
                IEnumerable<Holiday> holidayList = _businessDaysRepository.GetHolidaysOfYear(year);
                if (holidayList != null)
                {
                    listOfholidays = new List<ViewModel.Admin.HolidayViewModel>();
                    foreach (Holiday holiday in holidayList)
                    {
                        HolidayViewModel holidayViewModel = new HolidayViewModel();
                        holidayViewModel.HolidayDate = holiday.HolidayDate;
                        holidayViewModel.HolidayName = holiday.HolidayName;
                        holidayViewModel.DateKey = holiday.DateKey;
                        listOfholidays.Add(holidayViewModel);
                    }
                }
                return listOfholidays;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        public Dictionary<string, int> GetHolidaysByMonth(int year)
        {
            try
            {
                return _businessDaysRepository.GetHolidaysByMonth(year);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Edits Exixting Holiday
        /// </summary>
        /// <param name="holiday"></param>
        /// <returns></returns>
        public bool AddOrEditHoliday(HolidayViewModel holiday)
        {
            try
            {
                Holiday editHoliday = BusinessMapper.MapHolidayViewModelToBusinessModel(holiday);
                return _businessDaysRepository.AddOrEditHoliday(editHoliday);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Deletes Holiday
        /// </summary>
        /// <param name="holiday"></param>
        /// <returns></returns>
        public bool DeleteHoliday(HolidayViewModel holiday)
        {
            try
            {
                Holiday deleteHoliday = BusinessMapper.MapHolidayViewModelToBusinessModel(holiday);
                return _businessDaysRepository.DeleteHoliday(deleteHoliday);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// updates cliet targets.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public bool UpdateClientTargets(int year)
        {
            try
            {
                return _clientDataRepository.UpdateClientTargetData(year);
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
