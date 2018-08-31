using M3Pact.DomainModel.DomainModels;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Common;
using M3Pact.Infrastructure.Interfaces.Repository.Admin;
using System;
using System.Collections.Generic;
using System.Linq;

namespace M3Pact.Repository.Admin
{
    public class SiteRepository : ISiteRepository
    {
        #region private Properties

        private M3PactContext _m3pactContext;
        private UserContext userContext;

        #endregion private Properties

        #region Constructor

        public SiteRepository(M3PactContext m3PactContext)
        {
            _m3pactContext = m3PactContext;
            userContext = UserHelper.getUserContext();
        }

        #endregion Constructor

        #region public Methods

        /// <summary>
        /// Returns the Sites
        /// </summary>
        /// <returns></returns>
        public List<BusinessModel.BusinessModels.Site> GetSites(bool isActiveSites)
        {
            try
            {
                List<BusinessModel.BusinessModels.Site> siteDto = new List<BusinessModel.BusinessModels.Site>();
                IEnumerable<DomainModel.DomainModels.Site> sites;
                
                if (isActiveSites)
                {
                    sites = _m3pactContext.Site.Where(x => x.RecordStatus == DomainConstants.RecordStatusActive);
                }
                else
                {
                    sites = _m3pactContext.Site;
                }

                if (sites != null && sites.Count() > 0)
                {
                    foreach (DomainModel.DomainModels.Site item in sites)
                    {
                        BusinessModel.BusinessModels.Site site = new BusinessModel.BusinessModels.Site();
                        site.SiteCode = item.SiteCode;
                        site.SiteName = item.SiteName;
                        site.SiteDescription = item.SiteDescription;
                        site.RecordStatus = item.RecordStatus;
                        site.SiteId = item.SiteId;
                        siteDto.Add(site);
                    }
                }
                return siteDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Saves the Sites
        /// </summary>
        /// <param name="sitesDto"></param>
        /// <returns></returns>
        public bool SaveSites(List<BusinessModel.BusinessModels.Site> sitesDto)
        {
            try
            {
                List<DomainModel.DomainModels.Site> sites = new List<DomainModel.DomainModels.Site>();
                List<DomainModel.DomainModels.Site> sitesUpdated = new List<DomainModel.DomainModels.Site>();
                DomainModel.DomainModels.Site siteModel;

                foreach (BusinessModel.BusinessModels.Site siteDto in sitesDto)
                {
                    DomainModel.DomainModels.Site site = _m3pactContext.Site.FirstOrDefault(x => x.SiteId == siteDto.SiteId);
                    if (site != null)
                    {
                        siteModel = site;
                        siteModel.ModifiedDate = DateTime.UtcNow;
                        siteModel.ModifiedBy = userContext.UserId;
                        sitesUpdated.Add(siteModel);
                    }
                    else
                    {
                        siteModel = new DomainModel.DomainModels.Site();
                        siteModel.CreatedDate = DateTime.UtcNow;
                        siteModel.ModifiedDate = DateTime.UtcNow;
                        siteModel.CreatedBy = userContext.UserId;
                        siteModel.ModifiedBy = userContext.UserId;
                        sites.Add(siteModel);
                    }
                    siteModel.SiteCode = siteDto.SiteCode;
                    siteModel.SiteName = siteDto.SiteName;
                    siteModel.SiteDescription = siteDto.SiteDescription;
                    siteModel.RecordStatus = siteDto.RecordStatus;
                    siteModel.SiteId = siteDto.SiteId;
                }
                if (sitesUpdated.Count > 0)
                {
                    _m3pactContext.Site.UpdateRange(sitesUpdated);
                }
                if (sites.Count > 0)
                {
                    _m3pactContext.Site.AddRange(sites);
                }
                if (sitesUpdated.Count > 0 || sites.Count > 0)
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
        /// Getting BusinessUnits associated with sites
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="isRecordStatus"></param>
        /// <returns></returns>
        public List<string> GetBusinessUnitsAssociatedWithSite(int siteId, bool isRecordStatus = true)
        {
            try
            {
                if (isRecordStatus)
                {
                    return (from bu in _m3pactContext.BusinessUnit
                            where bu.SiteId == siteId
                            && bu.RecordStatus == DomainConstants.RecordStatusActive
                            orderby bu.BusinessUnitName
                            select (bu.BusinessUnitCode + '-' + bu.BusinessUnitName))?.ToList();
                }
                else
                {
                    return (from bu in _m3pactContext.BusinessUnit
                            where bu.SiteId == siteId
                            orderby bu.BusinessUnitName
                            select (bu.BusinessUnitCode + '-' + bu.BusinessUnitName))?.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Getting Clients associated with Sites
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="isRecordStatus"></param>
        /// <returns></returns>

        public List<string> GetClientsAssociatedWithSite(int siteId, bool isRecordStatus = true)
        {
            try
            {
                if (isRecordStatus)
                {
                    return (from c in _m3pactContext.Client
                            join s in _m3pactContext.Site
                            on c.SiteId equals s.SiteId
                            where c.IsActive == DomainConstants.RecordStatusActive
                            && s.RecordStatus == DomainConstants.RecordStatusActive
                            && s.SiteId == siteId
                            orderby c.Name
                            select (c.ClientCode + '-' + c.Name))?.ToList();
                }
                else
                {
                    return (from c in _m3pactContext.Client
                            join s in _m3pactContext.Site
                            on c.SiteId equals s.SiteId
                            where s.SiteId == siteId
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
        /// To activate or inactivate sites
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public bool ActivateOrInactivateSites(BusinessModel.BusinessModels.Site site)
        {
            try
            {
                DomainModel.DomainModels.Site repoSite = _m3pactContext.Site.Where(s => s.SiteId == site.SiteId).ToList().FirstOrDefault();

                if (repoSite != null)
                {
                    if (repoSite.RecordStatus == DomainConstants.RecordStatusActive)
                    {
                        repoSite.RecordStatus = DomainConstants.RecordStatusInactive;

                        List<Client> clients = _m3pactContext.Client.Where(c => c.SiteId == repoSite.SiteId && c.IsActive == DomainConstants.RecordStatusActive)?.ToList();
                        if (clients != null && clients.Count > 0)
                        {
                            foreach (Client c in clients)
                            {
                                c.RecordStatus = DomainConstants.RecordStatusInactive;
                                c.IsActive = DomainConstants.RecordStatusInactive;
                            }
                            _m3pactContext.UpdateRange(clients);
                        }
                    }
                    else
                    {
                        repoSite.RecordStatus = DomainConstants.RecordStatusActive;
                    }

                    _m3pactContext.Update(repoSite);
                    _m3pactContext.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion public Methods
    }
}
