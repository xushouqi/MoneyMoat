using ProtoBuf;

namespace StockModels.ViewModels
{
    [ProtoContract]
    public class StockData
    {
        [ProtoMember(1)]
        public int ConId { get; set; }
        [ProtoMember(2)]
        public System.DateTime UpdateTime { get; set; }
        [ProtoMember(3)]
        public string Symbol { get; set; }
        [ProtoMember(4)]
        public string Name { get; set; }
        [ProtoMember(5)]
        public string Exchange { get; set; }
        [ProtoMember(6)]
        public string Currency { get; set; }
        [ProtoMember(7)]
        public string Category { get; set; }
        [ProtoMember(8)]
        public System.DateTime EarliestDate { get; set; }
        [ProtoMember(9)]
        public int SharesOut { get; set; }
        [ProtoMember(10)]
        public int CommonShareholders { get; set; }
        [ProtoMember(11)]
        public int Employees { get; set; }
        [ProtoMember(12)]
        public float MarketCap { get; set; }

    }
}
