using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyModels
{
    public class FinSummary : Entity
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

        public string reportType { get; set; }
        public string period { get; set; }

        public float TotalRevenue { get; set; } = 0f;
        public float EPS { get; set; } = 0f;
        public float PE { get; set; } = 0f;
        public float PEG { get; set; } = 0f;
        public float Price { get; set; } = 0f;
        public float PB { get; set; } = 0f;
        /// <summary>
        /// 市销率=总市值/销售收入
        /// </summary>
        public float PS { get; set; } = 0f;

        public DateTime asofDate { get; set; } = DateTime.MinValue;
    }
}
