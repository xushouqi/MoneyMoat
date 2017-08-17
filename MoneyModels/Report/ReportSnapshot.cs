using System;
using System.Collections.Generic;
using YAXLib;

namespace MoneyModels
{
    public class ReportSnapshot
    {
        [YAXAttributeForClass]
        public string Revision { get; set; }

        public class GeneralInfo
        {
            public string CoStatus { get; set; }
            public DateTime LastModified { get; set; }
            public DateTime LatestAvailableAnnual { get; set; }
            public DateTime LatestAvailableInterim { get; set; }

            public int Employees { get; set; }
            public float SharesOut { get; set; }

            [YAXAttributeFor("ReportingCurrency")]
            [YAXSerializeAs("Code")]
            public string ReportingCurrency { get; set; }

            [YAXAttributeFor("MostRecentExchange")]
            [YAXSerializeAs("Date")]
            public DateTime MostRecentExchangeDate { get; set; }
            public string MostRecentExchange { get; set; }
        }
        public GeneralInfo CoGeneralInfo { get; set; }

        public class peerInfo
        {
            [YAXAttributeFor(".")]
            public DateTime lastUpdated { get; set; }

            public class Industry
            {
                [YAXAttributeFor(".")]
                public string type { get; set; }
                [YAXAttributeFor(".")]
                public int order { get; set; }
                [YAXAttributeFor(".")]
                public int reported { get; set; }
                [YAXAttributeFor(".")]
                public string code { get; set; }
                [YAXValueForClass]
                public string Name { get; set; }
            }
            [YAXCollection(YAXCollectionSerializationTypes.Recursive, EachElementName = "Industry")]
            public List<Industry> IndustryInfo { get; set; }
        }

        public class RatioData
        {
            [YAXAttributeFor(".")]
            public string PriceCurrency { get; set; }
            [YAXAttributeFor(".")]
            public string ReportingCurrency { get; set; }
            [YAXAttributeFor(".")]
            public float ExchangeRate { get; set; }
            [YAXAttributeFor(".")]
            public DateTime LatestAvailableDate { get; set; }

            public class Group
            {
                [YAXAttributeFor(".")]
                public string ID { get; set; }

                [YAXDictionary(EachPairName = "Ratio", KeyName = "FieldName",
                          SerializeKeyAs = YAXNodeTypes.Attribute, SerializeValueAs = YAXNodeTypes.Content)]
                [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement)]
                public Dictionary<string, string> Datas { get; set; }
            }
            [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement, EachElementName = "Group")]
            public List<Group> RatioDatas { get; set; }
        }
        public RatioData Ratios { get; set; }

        public class Forecast
        {
            [YAXAttributeFor(".")]
            public int CurFiscalYear { get; set; }
            [YAXAttributeFor(".")]
            public int CurFiscalYearEndMonth { get; set; }
            [YAXAttributeFor(".")]
            public int CurInterimEndCalYear { get; set; }
            [YAXAttributeFor(".")]
            public int CurInterimEndMonth { get; set; }
            [YAXAttributeFor(".")]
            public string EarningsBasis { get; set; }

            public class ForecastRatio
            {
                [YAXAttributeFor(".")]
                public string PeriodType { get; set; }

                [YAXValueForClass]
                public float Value { get; set; }
            }
            [YAXDictionary(EachPairName = "Ratio", KeyName = "FieldName",
                      SerializeKeyAs = YAXNodeTypes.Attribute, SerializeValueAs = YAXNodeTypes.Content)]
            [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement)]
            public Dictionary<string, ForecastRatio> Datas { get; set; }
        }
        public Forecast ForecastData { get; set; }
    }
}
