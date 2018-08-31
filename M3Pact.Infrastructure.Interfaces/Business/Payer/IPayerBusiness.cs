using M3Pact.ViewModel;
using M3Pact.ViewModel.Client;
using M3Pact.ViewModel.Common;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Business.Payer
{
    public interface IPayerBusiness
    {
        PayerViewModelList GetPayers(bool fromClient);
        ValidationViewModel SavePayers(List<PayerViewModel> payers);
        PayerDataViewModelList GetTopPayersData(string clientCode, int month, int year);
        ClientPayerViewModelList GetClientPayers(string clientCode);
        PayerViewModelList GetActivePayersToAssignForClient(string clientCode);
        ValidationViewModel SaveClientPayers(List<ClientPayerViewModel> clientPayers);
        StringList GetClientsAssignedtoPayer(string payerCode, bool isRecordStatus = true);
        ValidationViewModel ActivateOrDeactivatePayer(PayerViewModel payerViewModel);
        ValidationViewModel ActivateOrDeactivateClientPayer(ClientPayerViewModel clientPayer);
    }
}
