using ProtoBuf;

namespace StockModels.ViewModels
{
    [ProtoContract]
    public class FinSummaryData
    {
        [ProtoMember(1)]
        public string Symbol { get; set; }
        [ProtoMember(2)]
        public string reportType { get; set; }
        [ProtoMember(3)]
        public string period { get; set; }
        [ProtoMember(4)]
        public System.DateTime asofDate { get; set; }
        [ProtoMember(5)]
        public float TotalRevenue { get; set; }
        [ProtoMember(6)]
        public float EPS { get; set; }
        [ProtoMember(7)]
        public float PE { get; set; }
        [ProtoMember(8)]
        public float PEYoY { get; set; }
        [ProtoMember(9)]
        public float PEG { get; set; }
        [ProtoMember(10)]
        public float Price { get; set; }
        [ProtoMember(11)]
        public float PriceYoY { get; set; }
        [ProtoMember(12)]
        public float PB { get; set; }
        [ProtoMember(13)]
        public float PS { get; set; }
        [ProtoMember(14)]
        public float SCSI { get; set; }
        [ProtoMember(15)]
        public float SNCC { get; set; }
        [ProtoMember(16)]
        public float OTLO { get; set; }
        [ProtoMember(17)]
        public float OTLOYoY { get; set; }
        [ProtoMember(18)]
        public float SCEX { get; set; }
        [ProtoMember(19)]
        public float FreeCashFlow { get; set; }
        [ProtoMember(20)]
        public float FreeCashFlowYoY { get; set; }

    }
}
