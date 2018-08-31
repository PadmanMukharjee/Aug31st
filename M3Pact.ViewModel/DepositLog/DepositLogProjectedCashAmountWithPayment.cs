using System.Collections.Generic;

namespace M3Pact.ViewModel.DepositLog
{
    public class DepositLogProjectedCashAmountWithPayment
    {
        public List<DepositLogProjectedCashViewModel> DepositLogProjectedCashViewModelList { get; set; }
        public List<DepositLogProjectedCashViewModel> Payments { get; set; }
    }
}
