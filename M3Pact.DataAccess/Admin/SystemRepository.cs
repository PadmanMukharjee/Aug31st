using M3Pact.DomainModel.DomainModels;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Common;
using M3Pact.Infrastructure.Interfaces.Repository.Admin;
using System;
using System.Collections.Generic;
using System.Linq;

namespace M3Pact.Repository.Admin
{
    public class SystemRepository : ISystemRepository
    {
        #region private Properties

        private M3PactContext _m3pactContext;
        private UserContext userContext;

        #endregion private Properties

        #region Constructor
        public SystemRepository(M3PactContext m3PactContext)
        {
            _m3pactContext = m3PactContext;
            userContext = UserHelper.getUserContext();
        }
        #endregion Constructor


        #region public Methods
        /// <summary>
        /// To get all the System Data
        /// </summary>
        /// <param name="isActiveSystems"></param>
        /// <returns></returns>
        public List<BusinessModel.Admin.System> GetAllSystems(bool isActiveSystems)
        {
            try
            {
                if (isActiveSystems)
                {
                    return (from s in _m3pactContext.System
                            where s.RecordStatus == DomainConstants.RecordStatusActive
                            select new BusinessModel.Admin.System()
                            {
                                SystemCode = s.SystemCode,
                                SystemDescription = s.SystemDescription,
                                SystemName = s.SystemName,
                                RecordStatus = s.RecordStatus,
                                ID = s.SystemId
                            })?.ToList();
                }
                else
                {
                    return (from s in _m3pactContext.System
                            select new BusinessModel.Admin.System()
                            {
                                SystemCode = s.SystemCode,
                                SystemDescription = s.SystemDescription,
                                SystemName = s.SystemName,
                                RecordStatus = s.RecordStatus,
                                ID = s.SystemId
                            })?.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Saves the Systems
        /// </summary>
        /// <param name="systemsDto"></param>
        /// <returns></returns>
        public bool SaveSystems(List<BusinessModel.Admin.System> systemsDto)
        {
            try
            {
                List<DomainModel.DomainModels.System> systems = new List<DomainModel.DomainModels.System>();
                List<DomainModel.DomainModels.System> systemsUpdated = new List<DomainModel.DomainModels.System>();
                DomainModel.DomainModels.System SystemModel;

                foreach (BusinessModel.Admin.System SystemDto in systemsDto)
                {
                    DomainModel.DomainModels.System System = _m3pactContext.System.FirstOrDefault(x => x.SystemId == SystemDto.ID);
                    if (System != null)
                    {
                        SystemModel = System;
                        SystemModel.ModifiedDate = DateTime.UtcNow;
                        SystemModel.ModifiedBy = userContext.UserId;
                        systemsUpdated.Add(SystemModel);
                    }
                    else
                    {
                        SystemModel = new DomainModel.DomainModels.System();
                        SystemModel.CreatedDate = DateTime.UtcNow;
                        SystemModel.ModifiedDate = DateTime.UtcNow;
                        SystemModel.CreatedBy = userContext.UserId;
                        SystemModel.ModifiedBy = userContext.UserId;
                        systems.Add(SystemModel);
                    }
                    SystemModel.SystemCode = SystemDto.SystemCode;
                    SystemModel.SystemName = SystemDto.SystemName;
                    SystemModel.SystemDescription = SystemDto.SystemDescription;
                    SystemModel.RecordStatus = SystemDto.RecordStatus;
                    SystemModel.SystemId = SystemDto.ID;
                }
                if (systemsUpdated.Count > 0)
                {
                    _m3pactContext.System.UpdateRange(systemsUpdated);
                }
                if (systems.Count > 0)
                {
                    _m3pactContext.System.AddRange(systems);
                }
                if (systemsUpdated.Count > 0 || systems.Count > 0)
                {
                    _m3pactContext.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // <summary>
        /// To get the clients asociated with the system
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="isRecordStatus"></param>
        /// <returns></returns>
        public List<string> GetClientsAssociatedWithSystem(int systemId, bool isRecordStatus)
        {
            try
            {
                if (isRecordStatus)
                {
                    return (from c in _m3pactContext.Client
                            where c.SystemId == systemId
                            && c.RecordStatus == DomainConstants.RecordStatusActive
                            orderby c.Name
                            select (c.ClientCode + '-' + c.Name))?.ToList();
                }
                else
                {
                    return (from c in _m3pactContext.Client
                            where c.SystemId == systemId
                            orderby c.Name
                            select (c.ClientCode + '-' + c.Name))?.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Activating or InActivating systems
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        public bool ActivateOrInactivateSystems(BusinessModel.Admin.System system)
        {
            try
            {
                DomainModel.DomainModels.System systemDTO = _m3pactContext.System.FirstOrDefault(s => s.SystemCode == system.SystemCode);

                if (systemDTO != null)
                {
                    if (systemDTO.RecordStatus == DomainConstants.RecordStatusActive)
                    {
                        systemDTO.RecordStatus = DomainConstants.RecordStatusInactive;

                        List<Client> clients = _m3pactContext.Client.Where(c => c.SystemId == systemDTO.SystemId && c.IsActive == DomainConstants.RecordStatusActive).ToList();

                        if (clients != null && clients.Count > 0)
                        {
                            foreach (Client c in clients)
                            {
                                c.RecordStatus = DomainConstants.RecordStatusInactive;
                                c.IsActive = DomainConstants.RecordStatusInactive;
                            }
                            _m3pactContext.UpdateRange(clients);
                        }
                        _m3pactContext.Update(systemDTO);

                    }
                    else
                    {
                        systemDTO.RecordStatus = DomainConstants.RecordStatusActive;
                        _m3pactContext.Update(systemDTO);
                    }

                    _m3pactContext.SaveChanges();

                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        #endregion public Methods
    }
}
