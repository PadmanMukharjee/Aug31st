﻿using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class DepositLogMonthlyDetails
    {
        public int CloseMonthId { get; set; }
        public int? ClientId { get; set; }
        public int? MonthId { get; set; }
        public int Year { get; set; }
        public decimal? TotalDepositAmount { get; set; }
        public string MonthStatus { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public decimal? ProjectedCash { get; set; }

        public Client Client { get; set; }
        public Month Month { get; set; }
    }
}