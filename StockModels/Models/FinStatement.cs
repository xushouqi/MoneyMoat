using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibs;

namespace StockModels
{
    [DataModels]
    public class FinStatement : Entity
    {
        public override int GetId()
        {
            return Id;
        }
        public override string GetKey()
        {
            throw new NotImplementedException();
        }
        public override DateTime TryUpdateTime()
        {
            return UpdateTime = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime UpdateTime { get; set; } = DateTime.Now;

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
