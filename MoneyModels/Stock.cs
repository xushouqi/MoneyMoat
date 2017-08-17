using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyModels
{
    public class Stock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StockId { get; set; }

        [ConcurrencyCheck]
        public DateTime RowVersion { get; set; }

        public DateTime CreateTime { get; set; }


        public string gid { get; set; }/*股票编号*/
        public string name { get; set; }/*股票名称*/

        public int increPer { get; set; } /*涨跌百分比*/
        public int increase { get; set; }/*涨跌额*/
        public int todayStartPri { get; set; }/*今日开盘价*/
        public int yestodEndPri { get; set; }    /*昨日收盘价*/
        public int nowPri { get; set; }  /*当前价格*/
        public int todayMax { get; set; }    /*今日最高价*/
        public int todayMin { get; set; }    /*今日最低价*/
        public int competitivePri { get; set; }  /*竞买价*/
        public int reservePri { get; set; }  /*竞卖价*/
        public int traNumber { get; set; }   /*成交量*/
        public int traAmount { get; set; }   /*成交金额*/
        public int buyOne { get; set; }   /*买一*/
        public int buyOnePri { get; set; }   /*买一报价*/
        public int buyTwo { get; set; }   /*买二*/
        public int buyTwoPri { get; set; }   /*买二报价*/
        public int buyThree { get; set; }   /*买三*/
        public int buyThreePri { get; set; }   /*买三报价*/
        public int buyFour { get; set; }   /*买四*/
        public int buyFourPri { get; set; }   /*买四报价*/
        public int buyFive { get; set; }   /*买五*/
        public int buyFivePri { get; set; }   /*买五报价*/
        public int sellOne { get; set; }   /*卖一*/
        public int sellOnePri { get; set; }   /*卖一报价*/
        public int sellTwo { get; set; }   /*卖二*/
        public int sellTwoPri { get; set; }   /*卖二报价*/
        public int sellThree { get; set; }   /*卖三*/
        public int sellThreePri { get; set; }   /*卖三报价*/
        public int sellFour { get; set; }   /*卖四*/
        public int sellFourPri { get; set; }   /*卖四报价*/
        public int sellFive { get; set; }   /*卖五*/
        public int sellFivePri { get; set; }   /*卖五报价*/

        public DateTime dateTime { get; set; }   /*时间*/

        public string date { get; set; }   /*日期*/
        public string time { get; set; }   /*时间*/
    }
}
