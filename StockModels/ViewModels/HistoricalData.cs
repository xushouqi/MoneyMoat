using ProtoBuf;

namespace StockModels.ViewModels
{
    [ProtoContract]
    public class HistoricalData
    {
        [ProtoMember(1)]
        public string Symbol { get; set; }
        [ProtoMember(2)]
        public int volume { get; set; }
        [ProtoMember(3)]
        public int lot_volume { get; set; }
        [ProtoMember(4)]
        public int timestamp { get; set; }
        [ProtoMember(5)]
        public float open { get; set; }
        [ProtoMember(6)]
        public float high { get; set; }
        [ProtoMember(7)]
        public float close { get; set; }
        [ProtoMember(8)]
        public float low { get; set; }
        [ProtoMember(9)]
        public float chg { get; set; }
        [ProtoMember(10)]
        public float percent { get; set; }
        [ProtoMember(11)]
        public float turnrate { get; set; }
        [ProtoMember(12)]
        public float ma5 { get; set; }
        [ProtoMember(13)]
        public float ma10 { get; set; }
        [ProtoMember(14)]
        public float ma20 { get; set; }
        [ProtoMember(15)]
        public float ma30 { get; set; }
        [ProtoMember(16)]
        public float dif { get; set; }
        [ProtoMember(17)]
        public float dea { get; set; }
        [ProtoMember(18)]
        public float macd { get; set; }
        [ProtoMember(19)]
        public System.DateTime time { get; set; }

    }
}
