using ProtoBuf;
using StockModels;

namespace StockModels.ViewModels
{
    [ProtoContract]
    public class FinStatementData
    {
        [ProtoMember(1)]
        public int Key { get; set; }
        [ProtoMember(2)]
        public string Symbol { get; set; }
        [ProtoMember(3)]
        public System.DateTime EndDate { get; set; }
        [ProtoMember(4)]
        public int FiscalYear { get; set; }
        [ProtoMember(5)]
        public string FiscalPeriod { get; set; }
        [ProtoMember(6)]
        public string coaCode { get; set; }
        [ProtoMember(7)]
        public float Value { get; set; }

    }
}
