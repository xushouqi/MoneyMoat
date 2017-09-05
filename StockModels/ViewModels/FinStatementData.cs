using ProtoBuf;

namespace StockModels.ViewModels
{
    [ProtoContract]
    public class FinStatementData
    {
        [ProtoMember(1)]
        public string Symbol { get; set; }
        [ProtoMember(2)]
        public System.DateTime EndDate { get; set; }
        [ProtoMember(3)]
        public int FiscalYear { get; set; }
        [ProtoMember(4)]
        public string FiscalPeriod { get; set; }
        [ProtoMember(5)]
        public string coaCode { get; set; }
        [ProtoMember(6)]
        public float Value { get; set; }

    }
}
