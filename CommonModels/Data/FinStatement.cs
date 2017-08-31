using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyModels
{
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

        public string Symbol { get; set; }

        public DateTime EndDate { get; set; } = DateTime.MinValue;
        //public string periodType { get; set; }
        //public int PeriodLength { get; set; }
        public int FiscalYear { get; set; }
        public string FiscalPeriod { get; set; }

        public string coaCode { get; set; }
        public float Value { get; set; } = 0f;
    }
}
