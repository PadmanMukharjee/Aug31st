using M3Pact.ViewModel;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Business.Admin
{
    public interface ISpecialityBusiness
    {
        List<SpecialityViewModel> GetSpecialities(bool fromClient);
        bool SaveSpecialities(List<SpecialityViewModel> specialities);
        bool ActiveOrInactiveSpecialities(SpecialityViewModel speciality);
        List<string> GetClientsAssociatedWithSpecialty(int specialtyId, bool isRecordStatus = true);
    }
}
