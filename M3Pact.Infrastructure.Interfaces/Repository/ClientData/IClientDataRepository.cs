using M3Pact.BusinessModel;
using M3Pact.BusinessModel.BusinessModels;
using M3Pact.BusinessModel.Client;
using System;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Repository.ClientData
{
    public interface IClientDataRepository
    {
        ClientDetails GetClientData(string clientCode);
        bool SaveClientData(ClientDetails clientData);        
        List<ClientTargetModel> SaveClientTargetData(ClientTargetModel clientTargetModel);
        List<ClientTargetModel> GetClientTargetData(string clientCode, int year);
        void SaveManuallyEditedTargetData(ManuallyEditedTargetsBusinessModel manuallyEditedTargets);
        List<ClientsData> GetAllClientsData();
        List<ClientStepStatusDetail> GetClientStepStausDetails(string clientCode);
        bool SaveClientStepStatusDetail(ClientStepStatusDetail clientStepDetail);
        List<ClientDetails> GetClientsByUser(string userID,string role);
        List<ClientCloseMonth> GetClientCloseMonthData(string clientCode, int year);
        bool CheckForExistingClientCode(string clientCode);
        bool ActivateClient(string clientCode, DateTime checklistEffectiveWeek, DateTime checklistEffectiveMonth);
        bool UpdateClientTargetData(int year);
        List<ClientDetails> GetUserClientsToShowForLoggedinMeridianUser(List<int> clientsIDsOfLoggedInUser, string userID);
        DateTime GetClientContractStartDate(string clientCode);
        List<ClientHistory> GetClientHistory(string clientCode, DateTime startDate, DateTime endDate);
        Dictionary<string, object> GetClientCreationDetails(string clientCode);
    }
}
