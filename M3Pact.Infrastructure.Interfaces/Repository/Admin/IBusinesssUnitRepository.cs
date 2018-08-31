using M3Pact.BusinessModel.BusinessModels;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Repository.Admin
{
    public interface IBusinesssUnitRepository
    {
        List<BusinessUnit> GetBusinessUnits();
        bool SaveBusinessUnits(List<BusinessUnit> businessUnits);
        bool ActiveOrInactiveBusinessUnit(BusinessUnit businessUnit);
        List<string> GetClientsAssociatedWithBusinessUnit(int businessUnitId,bool isRecordStatus = true);
    }
}
