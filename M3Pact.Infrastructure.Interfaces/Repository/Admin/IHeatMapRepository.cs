using M3Pact.BusinessModel.Admin;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Repository.Admin
{
    public interface IHeatMapRepository
    {
        List<HeatMapBusinessModel> GetKpisforHeatMap();
        List<HeatMapBusinessModel> GetHeatMapItems();
        bool SaveHeatMapItems(List<HeatMapBusinessModel> heatMapBusinessModels);

    }
}
