using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibs;

namespace StockModels
{
    [DataModels]
    public class FinStatement : Entity<int>
    {
        [DataView]
        public string Symbol { get; set; }

        [DataView]
        public DateTime EndDate { get; set; } = DateTime.MinValue;
        //public string periodType { get; set; }
        //public int PeriodLength { get; set; }
        [DataView]
        public int FiscalYear { get; set; }
        [DataView]
        public string FiscalPeriod { get; set; }

        [DataView]
        public string coaCode { get; set; }
        [DataView]
        public float Value { get; set; } = 0f;
    }
}
