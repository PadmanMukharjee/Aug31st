using M3Pact.ViewModel.HeatMap;

namespace M3Pact.Infrastructure.Interfaces.Business.HeatMap
{
    public interface IClientsHeatMapBusiness
    {
        HeatMapFiltersData GetHeatMapFilterData();
        string GetClientsHeatMap(ClientsHeatMapRequestViewModel heatMapRequest);
    }
}
