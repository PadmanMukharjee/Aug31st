using System;

namespace M3Pact.BusinessModel.BusinessModels
{
    public class DepositLogWeekDays
    {
        public string WeekName { get; set; }
        public decimal DepositAmount { get; set; }
        public int WeekDaysCompleted { get; set; }
        public int WeekDaysLeft { get; set; }

        public DateTime DepositStartDate { get; set; }

    }
}
