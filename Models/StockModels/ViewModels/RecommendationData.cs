using ProtoBuf;
using StockModels;

namespace StockModels.ViewModels
{
    [ProtoContract]
    public class RecommendationData
    {
        [ProtoMember(1)]
        public int Key { get; set; }

    }
}
