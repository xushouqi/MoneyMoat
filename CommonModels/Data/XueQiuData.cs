using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyModels
{
    public class XueQiuData : Entity
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

        public int volume { get; set; }
        public int lot_volume { get; set; }
        public long timestamp { get; set; }

        public float open { get; set; }
        public float high { get; set; }
        public float close { get; set; }
        public float low { get; set; }
        public float chg { get; set; }
        public float percent { get; set; }
        public float turnrate { get; set; }

        public float ma5 { get; set; }
        public float ma10 { get; set; }
        public float ma20 { get; set; }
        public float ma30 { get; set; }
        public float dif { get; set; }
        public float dea { get; set; }
        public float macd { get; set; }
        public DateTime time { get; set; }
    }

}
