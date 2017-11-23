using ProtoBuf;
using StockModels;

namespace StockModels.ViewModels
{
    [ProtoContract]
    public class FinancalData
    {
        [ProtoMember(1)]
        public int Key { get; set; }
        [ProtoMember(2)]
        public string Symbol { get; set; }
        [ProtoMember(3)]
        public string type { get; set; }
        [ProtoMember(4)]
        public string periodType { get; set; }
        [ProtoMember(5)]
        public int fYear { get; set; }
        [ProtoMember(6)]
        public int endMonth { get; set; }
        [ProtoMember(7)]
        public float Value { get; set; }

    }
}
