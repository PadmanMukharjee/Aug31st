using M3Pact.BusinessModel.HeatMap;
using M3Pact.DomainModel.DomainModels;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Repository.HeatMap
{
    public interface IClientsHeatMapRepository
    {
        HeatMapDetails GetHeatMapForClients(ClientsHeatMapRequest heatMapRequest);
        List<DomainModel.DomainModels.System> GetClientSystemsOfUser(List<string> userIds);
        List<BusinessUnit> GetClientBusinessUnitsOfUser(List<string> userIds);
        List<Speciality> GetClientSpecialitiesOfUser(List<string> userIds);

    }
}
