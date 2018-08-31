using M3Pact.Infrastructure.Interfaces.Business.Admin;
using M3Pact.Infrastructure.Interfaces.Repository.Admin;
using M3Pact.LoggerUtility;
using M3Pact.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Linq;

namespace M3Pact.Business.Admin
{
    public class SystemBusiness : ISystemBusiness
    {

        #region private variables
        private ISystemRepository _systemRepository;
        private ILogger _logger;
        #endregion private variables


        #region constructor
        public SystemBusiness(ISystemRepository systemRepository, ILogger logger)
        {
            _systemRepository = systemRepository;
            _logger = logger;

        }
        #endregion constructor


        #region public Methods

        /// <summary>
        /// To get all the Active Systems
        /// </summary>
        /// <param name="fromClient"></param>
        /// <returns></returns>
        public List<SystemViewModel> GetAllSystems(bool fromClient)
        {
            try
            {
                List<BusinessModel.Admin.System> systemDTO = _systemRepository.GetAllSystems(fromClient);

                if (systemDTO != null && systemDTO.Count > 0)
                {
                    List<SystemViewModel> systemViewModel = new List<SystemViewModel>();
                    foreach (BusinessModel.Admin.System system in systemDTO)
                    {
                        systemViewModel.Add(new SystemViewModel()
                        {
                            SystemCode = system.SystemCode,
                            SystemName = system.SystemName,
                            SystemDescription = system.SystemDescription,
                            RecordStatus = system.RecordStatus,
                            Clients = GetClientsAssociatedWithSystem(system.ID, false),
                            ID = system.ID
                        });
                    }

                    return fromClient ? systemViewModel?.OrderBy(s => s.SystemName).ToList() : systemViewModel;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Saves the Systems
        /// </summary>
        /// <param name="Systems"></param>
        /// <returns></returns>
        public bool SaveSystems(List<SystemViewModel> Systems)
        {
            try
            {
                List<BusinessModel.Admin.System> SystemsDTO = new List<BusinessModel.Admin.System>();
                foreach (SystemViewModel system in Systems)
                {
                    BusinessModel.Admin.System systemDTO = new BusinessModel.Admin.System();
                    systemDTO.SystemCode = system.SystemCode;
                    systemDTO.SystemDescription = system.SystemDescription;
                    systemDTO.SystemName = system.SystemName;
                    systemDTO.RecordStatus = system.RecordStatus;
                    systemDTO.ID = system.ID;
                    SystemsDTO.Add(systemDTO);
                }
                return _systemRepository.SaveSystems(SystemsDTO);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// To get the clients asociated with the system
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="isRecordStatus"></param>
        /// <returns></returns>
        public List<string> GetClientsAssociatedWithSystem(int systemId, bool isRecordStatus)
        {
            try
            {
                return _systemRepository.GetClientsAssociatedWithSystem(systemId, isRecordStatus);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Activating or InActivating systems
        /// </summary>
        /// <param name="systemViewModel"></param>
        /// <returns></returns>
        public bool ActivateOrInactivateSystem(SystemViewModel systemViewModel)
        {
            try
            {
                BusinessModel.Admin.System system = new BusinessModel.Admin.System();
                system.SystemCode = systemViewModel.SystemCode;
                system.ID = systemViewModel.ID;
                system.SystemDescription = systemViewModel.SystemDescription;
                system.SystemName = systemViewModel.SystemName;

                return _systemRepository.ActivateOrInactivateSystems(system);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return false;
            }
        }
        #endregion public constructor
    }
}
