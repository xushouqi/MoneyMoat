using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibs;

namespace StockModels
{
    [DataModels]
    public class NPEstimate : Entity<int>
    {
        public string Symbol { get; set; }

        public string type { get; set; }

        public float High { get; set; }
        public float Low { get; set; }
        public float Mean { get; set; }
        public float Median { get; set; }
        public float StdDev { get; set; }
        public int NumOfEst { get; set; }

        //public int UpGradings { get; set; }
        //public int DnGradings { get; set; }
        //public float LTGROWTH { get; set; }
        //public float TARGETPRICE { get; set; }

    }
}
