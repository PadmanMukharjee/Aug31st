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
    public class SiteBusiness : ISiteBusiness
    {

        #region private variables

        private ISiteRepository _siteRepository;
        private ILogger _logger;

        #endregion private variables

        #region constructor

        public SiteBusiness(ISiteRepository siteRepository, ILogger logger)
        {
            _siteRepository = siteRepository;
            _logger = logger;
        }
        #endregion constructor


        #region public Methods

        /// <summary>
        /// Returns the Sites
        /// </summary>
        /// <returns></returns>
        public List<SiteViewModel> GetSites(bool isActiveSites)
        {
            try
            {
                List<SiteViewModel> sites = new List<SiteViewModel>();
                List<BusinessModel.BusinessModels.Site> sitesDTO = _siteRepository.GetSites(isActiveSites);
                foreach (BusinessModel.BusinessModels.Site siteDTO in sitesDTO)
                {
                    SiteViewModel site = new SiteViewModel();
                    site.SiteCode = siteDTO.SiteCode;
                    site.SiteName = siteDTO.SiteName;
                    site.SiteDescription = siteDTO.SiteDescription;
                    site.RecordStatus = siteDTO.RecordStatus;
                    site.SiteId = siteDTO.SiteId;
                    site.Clients = GetClientsAssociatedWithSite(site.SiteId, false);
                    sites.Add(site);
                }
                return isActiveSites ? sites.OrderBy(x => x.SiteName).ToList() : sites;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Saves the Sites
        /// </summary>
        /// <param name="sites"></param>
        /// <returns></returns>
        public bool SaveSites(List<SiteViewModel> sites)
        {
            try
            {
                List<BusinessModel.BusinessModels.Site> sitesDTO = new List<BusinessModel.BusinessModels.Site>();                
                foreach (SiteViewModel site in sites)
                {
                    Site siteDTO = new BusinessModel.BusinessModels.Site();
                    siteDTO.SiteCode = site.SiteCode;
                    siteDTO.SiteDescription = site.SiteDescription;
                    siteDTO.SiteName = site.SiteName;
                    siteDTO.RecordStatus = site.RecordStatus;
                    siteDTO.SiteId = site.SiteId;
                    sitesDTO.Add(siteDTO);
                }
                return _siteRepository.SaveSites(sitesDTO);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return false;
            }
        }
        

        /// <summary>
        /// To get clients associated with sites
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="isRecordStatus"></param>
        /// <returns></returns>
        public List<string> GetClientsAssociatedWithSite(int siteId, bool isRecordStatus = true)
        {
            try
            {
                return _siteRepository.GetClientsAssociatedWithSite(siteId, isRecordStatus);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// To activate or inactvate sites
        /// </summary>
        /// <param name="siteViewModel"></param>
        /// <returns></returns>
        public bool ActivateOrInactivateSites(SiteViewModel siteViewModel)
        {
            try
            {
                Site site = new Site();
                site.SiteCode = siteViewModel.SiteCode;
                site.SiteId = siteViewModel.SiteId;
                site.SiteName = siteViewModel.SiteName;

                return _siteRepository.ActivateOrInactivateSites(site);
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
