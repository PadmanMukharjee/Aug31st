using M3Pact.BusinessModel.BusinessModels;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Repository
{
    public interface IPayerRepository
    {
        List<Payer> GetPayers();
        List<ClientPayer> GetTopPayersData(string clientCode , int month , int year);
        bool SavePayers(List<Payer> payers);
        List<ClientPayer> GetClientPayers(string clientCode);
        List<Payer> GetActivePayersToAssignForClient(string clientCode);
        bool SaveClientPayers(List<ClientPayer> clientPayers);
        List<string> GetClientsAssignedtoPayer(string payerCode,bool isRecordStatus);
        void ActivateOrDeactivatePayer(BusinessModel.BusinessModels.Payer payer);
        void ActivateOrDeactivateClientPayer(ClientPayer clientPayer);
    }
}
