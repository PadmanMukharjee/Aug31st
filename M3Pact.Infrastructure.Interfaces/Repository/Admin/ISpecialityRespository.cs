using M3Pact.BusinessModel.BusinessModels;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Repository.Admin
{
    public interface ISpecialityRespository
    {
        List<Speciality> GetSpecialities();
        bool SaveSpecialities(List<Speciality> specialities);
        bool ActiveOrInactiveSpecialities(Speciality speciality);
        List<string> GetClientsAssociatedWithSpecialty(int specialtyId, bool isRecordStatus = true);
    }
}
