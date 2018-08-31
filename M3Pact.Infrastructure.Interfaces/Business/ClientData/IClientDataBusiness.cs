using M3Pact.Infrastructure.Common;
using M3Pact.ViewModel;
using M3Pact.ViewModel.Client;
using System;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Business.ClientData
{
    public interface IClientDataBusiness
    {
        ClientViewModel GetClientData(string clientCode);
        ValidationViewModel SaveClientData(ClientViewModel clientData);
        byte[] GetClientDocument(string path);
        List<ClientTargetViewModel> SaveClientTargetData(ClientTargetViewModel clientTargetData);
        List<ClientTargetViewModel> GetClientTargetData(string clientCode, int year);
        ValidationViewModel SaveManuallyEditedTargetData(ManuallyEditedTargets manuallyEditedTargets);
        List<ClientStepDetailViewModel> GetClientStepStatusDetails(string clientCode);
        bool SaveClientStepStatusDetail(ClientStepDetailViewModel stepDetail);
        List<ClientsDataViewModel> GetAllClientsData();
        List<ClientsDataViewModel> GetActiveClientsForAUser();
        List<ClientViewModel> GetClientsByUser();
        bool ActivateClient(string clientCode);
        bool CheckForExistingClientCode(string clientCode);
        List<KeyValueModel> GetAllSites();
        List<ClientHistoryViewModel> GetClientHistory(string clientCode, DateTime startDate, DateTime endDate);
        Dictionary<string, object> GetClientCreationDetails(string clientCode);
    }
}
