using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibs;

namespace StockModels
{
    [DataModels]
    public class FinSummary : Entity<int>
    {
        [DataView]
        public string Symbol { get; set; }

        [DataView]
        public string reportType { get; set; }
        [DataView]
        public string period { get; set; }

        [DataView]
        public DateTime asofDate { get; set; } = DateTime.MinValue;

        [DataView]
        public float TotalRevenue { get; set; } = 0f;
        [DataView]
        public float EPS { get; set; } = 0f;
        [DataView]
        public float PE { get; set; } = 0f;
        /// <summary>
        /// 同比PE增长
        /// </summary>
        [DataView]
        public float PEYoY { get; set; } = 0f;
        /// <summary>
        /// 同比
        /// </summary>
        [DataView]
        public float PEG { get; set; } = 0f;
        [DataView]
        public float Price { get; set; } = 0f;
        /// <summary>
        /// 同比增长
        /// </summary>
        [DataView]
        public float PriceYoY { get; set; } = 0f;
        [DataView]
        public float PB { get; set; } = 0f;
        /// <summary>
        /// 市销率=总市值/销售收入
        /// </summary>
        [DataView]
        public float PS { get; set; } = 0f;

        /// <summary>
        /// 现金及等价物
        /// </summary>
        [DataView]
        public float SCSI { get; set; } = 0f;
        /// <summary>
        /// 现金变化 = 本期SCSI - 上期SCSI
        /// </summary>
        [DataView]
        public float SNCC { get; set; } = 0f;
        /// <summary>
        /// 经营现金流
        /// </summary>
        [DataView]
        public float OTLO { get; set; } = 0f;
        /// <summary>
        /// 经营现金流同比增长
        /// </summary>
        [DataView]
        public float OTLOYoY { get; set; } = 0f;
        /// <summary>
        /// 资本性支出
        /// </summary>
        [DataView]
        public float SCEX { get; set; } = 0f;
        /// <summary>
        /// 自由现金流 = OTLO - SCEX
        /// </summary>
        [DataView]
        public float FreeCashFlow { get; set; } = 0f;
        /// <summary>
        /// 自由现金流同比增长
        /// </summary>
        [DataView]
        public float FreeCashFlowYoY { get; set; } = 0f;
    }
}
