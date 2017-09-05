using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibs;

namespace StockModels
{
    [DataModels]
    public class Stock : Entity
    {
        public override int GetId()
        {
            return ConId;
        }
        public override string GetKey()
        {
            return Symbol;
        }
        public override DateTime TryUpdateTime()
        {
            return UpdateTime = DateTime.Now;
        }

        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataView]
        public int ConId { get; set; }
        [DataView]
        public DateTime UpdateTime { get; set; }

        //[ConcurrencyCheck]
        //public DateTime RowVersion { get; set; }

        [DataView]
        [Key]
        public string Symbol { get; set; }
        [DataView]
        public string Name { get; set; }

        [DataView]
        public string Exchange { get; set; }

        [DataView]
        public string Currency { get; set; }

        [DataView]
        public string Category { get; set; }

        [DataView]
        public DateTime EarliestDate { get; set; }

        [DataView]
        public Int64 SharesOut { get; set; }
        [DataView]
        public int CommonShareholders { get; set; }
        [DataView]
        public int Employees { get; set; }
        [DataView]
        public float MarketCap { get; set; }
    }
}
