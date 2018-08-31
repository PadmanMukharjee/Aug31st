using M3Pact.ViewModel;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Business.Admin
{
    public interface ISiteBusiness
    {
        List<SiteViewModel> GetSites(bool isActiveSites);
        bool SaveSites(List<SiteViewModel> sites);
        
        List<string> GetClientsAssociatedWithSite(int siteId, bool isRecordStatus = true);
        bool ActivateOrInactivateSites(SiteViewModel site);
    }
}
