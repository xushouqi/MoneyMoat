using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibs;

namespace StockModels
{
    [DataModels]
    public class Financal : Entity<int>
    {
        [DataView]
        public string Symbol { get; set; }

        [DataView]
        public string type { get; set; }

        [DataView]
        public string periodType { get; set; }

        [DataView]
        public Int64 fYear { get; set; }
        [DataView]
        public Int64 endMonth { get; set; }

        [DataView]
        public float Value { get; set; }
    }
}
