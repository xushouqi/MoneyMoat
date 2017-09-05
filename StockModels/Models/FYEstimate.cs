using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibs;

namespace StockModels
{
    [DataModels]
    public class FYEstimate : Entity
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
        public string type { get; set; }

        [DataView]
        public string periodType { get; set; }

        [DataView]
        public Int64 fYear { get; set; }
        [DataView]
        public Int64 endMonth { get; set; }

        [DataView]
        public float High { get; set; }
        [DataView]
        public float Low { get; set; }
        [DataView]
        public float Mean { get; set; }
        [DataView]
        public float Median { get; set; }
        [DataView]
        public float StdDev { get; set; }
        [DataView]
        public int NumOfEst { get; set; }

    }
}
