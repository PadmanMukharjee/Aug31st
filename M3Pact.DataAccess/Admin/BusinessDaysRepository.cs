using M3Pact.DomainModel.DomainModels;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Common;
using M3Pact.Infrastructure.Interfaces.Repository.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using businessAdminModel = M3Pact.BusinessModel.Admin;
using businessModels = M3Pact.BusinessModel.BusinessModels;

namespace M3Pact.Repository.Admin
{
    public class BusinessDaysRepository : IBusinessDaysRepository
    {
        #region private Properties

        private M3PactContext _m3pactContext;
        private UserContext userContext;

        #endregion private Properties

        #region Constructor

        public BusinessDaysRepository(M3PactContext m3PactContext)
        {
            _m3pactContext = m3PactContext;
            userContext = UserHelper.getUserContext();
        }

        #endregion Constructor

        #region public Methods

        /// <summary>
        /// Get the month name based on Id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string GetMonthName(int ID)
        {
            try
            {
                Month month = (from m in _m3pactContext.Month
                               where m.MonthId == ID
                               && m.RecordStatus == DomainConstants.RecordStatusActive
                               select m
                                 ).ToList().FirstOrDefault();
                return month.MonthName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Returns the  BusinessDays
        /// </summary>
        /// <returns></returns>
        public List<BusinessModel.BusinessModels.BusinessDays> GetBusinessDays()
        {
            try
            {
                List<BusinessModel.BusinessModels.BusinessDays> businessDaysDTO = new List<BusinessModel.BusinessModels.BusinessDays>();
                IEnumerable<BusinessDays> businessDays = _m3pactContext.BusinessDays;
                if (businessDays != null && businessDays.Count() > 0)
                {
                    foreach (BusinessDays businessDaysModel in businessDays)
                    {
                        BusinessModel.BusinessModels.BusinessDays businessDayDTO = new BusinessModel.BusinessModels.BusinessDays();
                        businessDayDTO.Year = businessDaysModel.Year;
                        businessDayDTO.MonthID = businessDaysModel.MonthId.Value;
                        businessDayDTO.Month = GetMonthName(businessDayDTO.MonthID);
                        businessDayDTO.NumberOfBusinessDays = businessDaysModel.BusinessDays1;
                        businessDayDTO.RecordStatus = businessDaysModel.RecordStatus;
                        businessDayDTO.ID = businessDaysModel.BusinessDaysId;
                        businessDaysDTO.Add(businessDayDTO);
                    }
                }
                return businessDaysDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Saves the BusinessDays
        /// </summary>
        /// <param name="businessDaysDTO"></param>
        /// <returns></returns>
        public bool SaveBusinessDays(List<BusinessModel.BusinessModels.BusinessDays> businessDaysDTO)
        {
            try
            {
                List<BusinessDays> businessDays = new List<DomainModel.DomainModels.BusinessDays>();
                List<BusinessDays> businessDaysUpdated = new List<DomainModel.DomainModels.BusinessDays>();

                foreach (BusinessModel.BusinessModels.BusinessDays businessDayDTO in businessDaysDTO)
                {
                    BusinessDays businessDay = _m3pactContext.BusinessDays.FirstOrDefault(x => x.MonthId == businessDayDTO.MonthID && x.Year == businessDayDTO.Year);
                    BusinessDays businessDayModel;
                    if (businessDay != null)
                    {
                        businessDayModel = businessDay;
                        businessDaysUpdated.Add(businessDayModel);
                    }
                    else
                    {
                        businessDayModel = new BusinessDays();
                        businessDayModel.CreatedDate = DateTime.UtcNow;
                        businessDayModel.CreatedBy = userContext.UserId;
                        businessDays.Add(businessDayModel);
                    }
                    businessDayModel.BusinessDays1 = businessDayDTO.NumberOfBusinessDays;
                    businessDayModel.Year = businessDayDTO.Year;
                    businessDayModel.MonthId = businessDayDTO.MonthID;
                    businessDayModel.RecordStatus = businessDayDTO.RecordStatus;
                    businessDayModel.ModifiedBy = userContext.UserId;
                    businessDayModel.ModifiedDate = DateTime.UtcNow; ;
                }
                if (businessDaysUpdated.Count > 0)
                {
                    _m3pactContext.BusinessDays.UpdateRange(businessDaysUpdated);
                }
                if (businessDays.Count > 0)
                {
                    _m3pactContext.AddRange(businessDays);
                }
                if (businessDaysUpdated.Count > 0 || businessDays.Count > 0)
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
        /// updates business days of a particular month
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public bool UpdateBusinessDays(int year)
        {
            try
            {
                Dictionary<int, int> businessDays = GetBusinessDaysOfMonth(year);
                List<businessModels.BusinessDays> businessDaysDTOList = new List<businessModels.BusinessDays>();
                if (businessDays != null)
                {
                    foreach (KeyValuePair<int, int> businessDay in businessDays)
                    {
                        businessModels.BusinessDays businessDayDTO = new businessModels.BusinessDays();
                        businessDayDTO.Year = year;
                        businessDayDTO.MonthID = businessDay.Key;
                        businessDayDTO.NumberOfBusinessDays = businessDay.Value;
                        businessDayDTO.RecordStatus = BusinessConstants.RecordStatusActive;
                        businessDaysDTOList.Add(businessDayDTO);
                    }
                }
                return SaveBusinessDays(businessDaysDTOList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// gets business days of a particular month
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public Dictionary<int, int> GetBusinessDaysOfMonth(int year)
        {
            try
            {
                Dictionary<int, int> groupByMonth = (from p in _m3pactContext.DateDimension
                                                     join od in _m3pactContext.Month on p.Month equals od.MonthId
                                                     where p.IsHoliday == false && p.IsWeekend == false && p.Year == year
                                                     group p by new { month = od.MonthId } into d
                                                     select new
                                                     {
                                                         Month = d.Key.month,
                                                         count = d.Count(),
                                                     }).ToDictionary(p => Convert.ToInt32(p.Month), p => p.count);

                return groupByMonth?.Count > 0 ? groupByMonth : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get holidays of the year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public IEnumerable<businessAdminModel.Holiday> GetHolidaysOfYear(int year)
        {
            try
            {
                IEnumerable<businessAdminModel.Holiday> holidayList = (from h in _m3pactContext.DateDimension
                                                                       where h.IsHoliday == true
                                                                       && h.Year == year
                                                                       select new M3Pact.BusinessModel.Admin.Holiday()
                                                                       {
                                                                           HolidayDate = h.Date,
                                                                           HolidayName = h.HolidayText,
                                                                           DateKey = h.DateKey
                                                                       }).ToList();
                return holidayList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get holidays for each month of the year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public Dictionary<string, int> GetHolidaysByMonth(int year)
        {
            try
            {
                if (!(_m3pactContext.BusinessDays.Any(x => x.Year == year)))
                {
                    UpdateBusinessDays(year);
                }
                Dictionary<string, int> groupByMonth = (from p in _m3pactContext.BusinessDays
                                                        join od in _m3pactContext.Month on p.MonthId equals od.MonthId
                                                        where p.Year == year
                                                        orderby p.MonthId
                                                        select new { Month = od.MonthName, count = p.BusinessDays1 }).ToDictionary(p => p.Month, p => p.count);
                return groupByMonth;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Edits the Selected Holiday
        /// </summary>
        /// <param name="holiday"></param>
        /// <returns></returns>
        public bool AddOrEditHoliday(businessAdminModel.Holiday holiday)
        {
            try
            {
                bool isEditedSuccesfully = false;
                bool isUpdatedBusinessDays = false;
                if (holiday != null && holiday.HolidayDate != null)
                {
                    DateDimension dayInQuestion = _m3pactContext.DateDimension.FirstOrDefault(day => day.DateKey == holiday.DateKey);
                    if (dayInQuestion != null)
                    {
                        if (dayInQuestion.Date.Day == holiday.HolidayDate.Day &&
                            dayInQuestion.Date.Month == holiday.HolidayDate.Month &&
                            dayInQuestion.Date.Year == holiday.HolidayDate.Year)
                        {
                            dayInQuestion.HolidayText = holiday.HolidayName;
                        }
                        else
                        {
                            dayInQuestion.IsHoliday = false;
                            dayInQuestion.HolidayText = string.Empty;
                            DateDimension dayToUpdate = _m3pactContext.DateDimension.FirstOrDefault(day => day.Day == holiday.HolidayDate.Day
                                                                                                               && day.Month == holiday.HolidayDate.Month
                                                                                                               && day.Year == holiday.HolidayDate.Year);

                            if (dayToUpdate != null)
                            {
                                dayToUpdate.HolidayText = holiday.HolidayName;
                                dayToUpdate.IsHoliday = true;
                            }
                        }
                    }
                    else
                    {
                        DateDimension dayToUpdate = _m3pactContext.DateDimension.FirstOrDefault(day => day.Day == holiday.HolidayDate.Day
                                                                                                              && day.Month == holiday.HolidayDate.Month
                                                                                                              && day.Year == holiday.HolidayDate.Year);

                        if (dayToUpdate != null)
                        {
                            dayToUpdate.HolidayText = holiday.HolidayName;
                            dayToUpdate.IsHoliday = true;
                        }
                    }
                    isEditedSuccesfully = _m3pactContext.SaveChanges() > 0;

                    if (dayInQuestion != null && string.IsNullOrEmpty(dayInQuestion.HolidayText))
                    {
                        isUpdatedBusinessDays = UpdateBusinessDays(dayInQuestion.Date.Year);
                    }
                    else
                    {
                        isUpdatedBusinessDays = UpdateBusinessDays(holiday.HolidayDate.Year);
                    }
                }

                return isEditedSuccesfully && isUpdatedBusinessDays;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Deletes Existing Holiday
        /// </summary>
        /// <param name="holiday"></param>
        /// <returns></returns>
        public bool DeleteHoliday(businessAdminModel.Holiday holiday)
        {
            try
            {
                bool isDeletedSuccesfully = false;
                bool isUpdatedBusinessDays = false;
                DateDimension dayInQuestion = _m3pactContext.DateDimension.FirstOrDefault(day => day.DateKey == holiday.DateKey);
                if (dayInQuestion != null)
                {
                    if (dayInQuestion.Date.Day == holiday.HolidayDate.Day &&
                    dayInQuestion.Date.Month == holiday.HolidayDate.Month &&
                    dayInQuestion.Date.Year == holiday.HolidayDate.Year)
                    {
                        dayInQuestion.IsHoliday = false;
                    }
                    isDeletedSuccesfully = _m3pactContext.SaveChanges() > 0;
                    isUpdatedBusinessDays = UpdateBusinessDays(holiday.HolidayDate.Year);
                }
                return isDeletedSuccesfully && isUpdatedBusinessDays;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion public Methods
    }
}
