using M3Pact.ViewModel.Admin;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Business.Admin
{
    public interface ISystemBusiness
    {
        List<SystemViewModel> GetAllSystems(bool fromClient);
        bool SaveSystems(List<SystemViewModel> systems);
        List<string> GetClientsAssociatedWithSystem(int systemId, bool isRecordStatus = true);
        bool ActivateOrInactivateSystem(SystemViewModel system);
    }
}
