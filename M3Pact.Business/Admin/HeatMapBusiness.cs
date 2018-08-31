using M3Pact.BusinessModel.Admin;
using M3Pact.Infrastructure.Interfaces.Business.Admin;
using M3Pact.Infrastructure.Interfaces.Repository.Admin;
using M3Pact.LoggerUtility;
using M3Pact.ViewModel.Admin;
using System;
using System.Collections.Generic;

namespace M3Pact.Business.Admin
{
    public class HeatMapBusiness : IHeatMapBusiness
    {

        #region Private Variables
        private IHeatMapRepository _heatMapRepository;
        private ILogger _logger;
        #endregion Private Varibles

        #region Constructor
        public HeatMapBusiness(IHeatMapRepository heatMapRepository, ILogger logger)
        {
            _heatMapRepository = heatMapRepository;
            _logger = logger;
        }
        #endregion Constructor


        #region Public Methods
        /// <summary>
        /// Business class to Get the heat map items
        /// </summary>
        /// <returns></returns>
        public List<HeatMapViewModel> GetHeatMapItems()
        {
            try
            {
                List<HeatMapBusinessModel> heatMapBusinessModels = _heatMapRepository.GetHeatMapItems();
                List<HeatMapViewModel> heatMapViewModels = new List<HeatMapViewModel>();
                heatMapBusinessModels.ForEach(h =>
                {
                    HeatMapViewModel heatMapViewModel = new HeatMapViewModel();
                    heatMapViewModel.KpiId = h.KpiId;
                    heatMapViewModel.KpiDescription = h.KpiDescription;
                    heatMapViewModel.ChecklistType = h.ChecklistType;
                    heatMapViewModels.Add(heatMapViewModel);
                });
                return heatMapViewModels;

            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Get KPis which are eligible for heat map item
        /// </summary>
        /// <returns></returns>
        public List<HeatMapViewModel> GetKpisforHeatMap()
        {
            try
            {
                List<HeatMapBusinessModel> heatMapBusinessModels = _heatMapRepository.GetKpisforHeatMap();
                List<HeatMapViewModel> heatMapViewModels = new List<HeatMapViewModel>();
                heatMapBusinessModels.ForEach(h =>
                {
                    HeatMapViewModel heatMapViewModel = new HeatMapViewModel();
                    heatMapViewModel.KpiId = h.KpiId;
                    heatMapViewModel.KpiDescription = h.KpiDescription;
                    heatMapViewModel.ChecklistType = h.ChecklistType;
                    heatMapViewModels.Add(heatMapViewModel);
                });
                return heatMapViewModels;

            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Save Heatmap items
        /// </summary>
        /// <param name="heatMapViewModels"></param>
        /// <returns></returns>
        public bool SaveHeatMapItems(List<HeatMapViewModel> heatMapViewModels)
        {
            try
            {
                List<HeatMapBusinessModel> heatMapBusinessModels = new List<HeatMapBusinessModel>();
                heatMapViewModels.ForEach(hv =>
                {
                    HeatMapBusinessModel heatMapBusinessModel = new HeatMapBusinessModel();
                    heatMapBusinessModel.ChecklistType = hv.ChecklistType;
                    heatMapBusinessModel.KpiId = hv.KpiId;
                    heatMapBusinessModel.KpiDescription = hv.KpiDescription;
                    heatMapBusinessModels.Add(heatMapBusinessModel);
                });
                return _heatMapRepository.SaveHeatMapItems(heatMapBusinessModels);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return false;
            }
        }

        #endregion Public Methods
    }
}
