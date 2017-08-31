using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyModels
{
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
        public int ConId { get; set; }
        public DateTime UpdateTime { get; set; }

        //[ConcurrencyCheck]
        //public DateTime RowVersion { get; set; }

        [Key]
        public string Symbol { get; set; }
        public string Name { get; set; }

        public string Exchange { get; set; }

        public string Currency { get; set; }

        public string Category { get; set; }

        public DateTime EarliestDate { get; set; }
        
        public Int64 SharesOut { get; set; }
        public int CommonShareholders { get; set; }
        public int Employees { get; set; }
        public float MarketCap { get; set; }
    }
}
