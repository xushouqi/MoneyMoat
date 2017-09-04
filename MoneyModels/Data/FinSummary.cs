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

        public DateTime asofDate { get; set; } = DateTime.MinValue;

        public float TotalRevenue { get; set; } = 0f;
        public float EPS { get; set; } = 0f;
        public float PE { get; set; } = 0f;
        /// <summary>
        /// 同比PE增长
        /// </summary>
        public float PEYoY { get; set; } = 0f;
        /// <summary>
        /// 同比
        /// </summary>
        public float PEG { get; set; } = 0f;
        public float Price { get; set; } = 0f;
        /// <summary>
        /// 同比增长
        /// </summary>
        public float PriceYoY { get; set; } = 0f;
        public float PB { get; set; } = 0f;
        /// <summary>
        /// 市销率=总市值/销售收入
        /// </summary>
        public float PS { get; set; } = 0f;

        /// <summary>
        /// 现金及等价物
        /// </summary>
        public float SCSI { get; set; } = 0f;
        /// <summary>
        /// 现金变化 = 本期SCSI - 上期SCSI
        /// </summary>
        public float SNCC { get; set; } = 0f;
        /// <summary>
        /// 经营现金流
        /// </summary>
        public float OTLO { get; set; } = 0f;
        /// <summary>
        /// 经营现金流同比增长
        /// </summary>
        public float OTLOYoY { get; set; } = 0f;
        /// <summary>
        /// 资本性支出
        /// </summary>
        public float SCEX { get; set; } = 0f;
        /// <summary>
        /// 自由现金流 = OTLO - SCEX
        /// </summary>
        public float FreeCashFlow { get; set; } = 0f;
        /// <summary>
        /// 自由现金流同比增长
        /// </summary>
        public float FreeCashFlowYoY { get; set; } = 0f;
    }
}
