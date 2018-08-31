using M3Pact.ViewModel;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Business.Admin
{
    public interface IBusinessUnitBusiness
    {
        List<BusinessUnitViewModel> GetBusinessUnits(bool fromClient);
        bool SaveBusinessUnits(List<BusinessUnitViewModel> businessUnits);
        bool ActiveOrInactiveBusinessUnit(BusinessUnitViewModel businessUnit);
        List<string> GetClientsAssociatedWithBusinessUnit(int businessUnitId,bool isRecordStatus = true);
    }
}
