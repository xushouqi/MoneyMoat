using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibs;

namespace StockModels
{
    [DataModels]
    public class Historical : Entity
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
        public int volume { get; set; }
        [DataView]
        public int lot_volume { get; set; }
        [DataView]
        public long timestamp { get; set; }

        [DataView]
        public float open { get; set; }
        [DataView]
        public float high { get; set; }
        [DataView]
        public float close { get; set; }
        [DataView]
        public float low { get; set; }
        [DataView]
        public float chg { get; set; }
        [DataView]
        public float percent { get; set; }
        [DataView]
        public float turnrate { get; set; }

        [DataView]
        public float ma5 { get; set; }
        [DataView]
        public float ma10 { get; set; }
        [DataView]
        public float ma20 { get; set; }
        [DataView]
        public float ma30 { get; set; }
        [DataView]
        public float dif { get; set; }
        [DataView]
        public float dea { get; set; }
        [DataView]
        public float macd { get; set; }
        [DataView]
        public DateTime time { get; set; }
    }

}
