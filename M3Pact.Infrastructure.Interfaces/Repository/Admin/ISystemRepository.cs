using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Repository.Admin
{
    public interface ISystemRepository
    {
        List<BusinessModel.Admin.System> GetAllSystems(bool isActiveSystems);
        bool SaveSystems(List<BusinessModel.Admin.System> systems);
        List<string> GetClientsAssociatedWithSystem(int systemId, bool isRecordStatus = true);
        bool ActivateOrInactivateSystems(BusinessModel.Admin.System system);
    }
}
