using ProtoBuf;

namespace StockModels.ViewModels
{
    [ProtoContract]
    public class FinancalData
    {
        [ProtoMember(1)]
        public string Symbol { get; set; }
        [ProtoMember(2)]
        public string type { get; set; }
        [ProtoMember(3)]
        public string periodType { get; set; }
        [ProtoMember(4)]
        public int fYear { get; set; }
        [ProtoMember(5)]
        public int endMonth { get; set; }
        [ProtoMember(6)]
        public float Value { get; set; }

    }
}
