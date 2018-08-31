using M3Pact.BusinessModel.BusinessModels;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Repository.Admin
{
    public  interface ISiteRepository
    {
        List<Site> GetSites(bool isActiveSites);
        bool SaveSites(List<Site> sites);
        List<string> GetBusinessUnitsAssociatedWithSite(int site,bool isRecordStatus = true);
        List<string> GetClientsAssociatedWithSite(int siteId, bool isRecordStatus = true);
        bool ActivateOrInactivateSites(Site site);
    }
}
