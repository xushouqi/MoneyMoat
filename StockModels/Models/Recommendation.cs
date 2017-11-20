using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibs;

namespace StockModels
{
    [DataModels]
    public class Recommendation : Entity<int>
    {
        public string Symbol { get; set; }

        public int BUY { get; set; }
        public int OUTPERFORM { get; set; }
        public int HOLD { get; set; }
        public int UNDERPERFORM { get; set; }
        public int SELL { get; set; }
    }
}
