using M3Pact.ViewModel.Admin;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Business.Admin
{
    public interface IHeatMapBusiness
    {
        List<HeatMapViewModel> GetKpisforHeatMap();
        List<HeatMapViewModel> GetHeatMapItems();
        bool SaveHeatMapItems(List<HeatMapViewModel> heatMapViewModels);
    }
}
