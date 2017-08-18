using System;
using System.Collections.Generic;
using YAXLib;

namespace MoneyModels
{
    public class RESC
    {
        public class CompanyData
        {
            public class SecurityInfoData
            {
                public class SecurityData
                {
                    [YAXDictionary(EachPairName = "MarketDataItem", KeyName = "type",
                              SerializeKeyAs = YAXNodeTypes.Attribute, SerializeValueAs = YAXNodeTypes.Content)]
                    [YAXCollection(YAXCollectionSerializationTypes.Recursive)]
                    public Dictionary<string, float> MarketData { get; set; }
                }
                public SecurityData Security { get; set; }
            }
            public SecurityInfoData SecurityInfo { get; set; }
        }
        public CompanyData Company { get; set; }

        public class ActualsData
        {
            public class FYActual
            {
                [YAXAttributeForClass]
                public string type { get; set; }
                [YAXAttributeForClass]
                public string unit { get; set; }

                public class FYPeriod
                {
                    [YAXAttributeForClass]
                    public Int64 endCalYear { get; set; }
                    [YAXAttributeForClass]
                    public Int64 endMonth { get; set; }
                    [YAXAttributeForClass]
                    public Int64 fYear { get; set; }
                    [YAXAttributeForClass]
                    public string periodType { get; set; }

                    public float ActValue { get; set; }
                }
                [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement, EachElementName = "FYPeriod")]
                public List<FYPeriod> FYPeriods { get; set; }
            }

            [YAXCollection(YAXCollectionSerializationTypes.Recursive, EachElementName = "FYActual")]
            public List<FYActual> FYActuals { get; set; }
        }
        public ActualsData Actuals { get; set; }

        public class ConsEstimatesData
        {
            public class FYEstimateData
            {
                [YAXAttributeForClass]
                public string type { get; set; }
                [YAXAttributeForClass]
                public string unit { get; set; }

                public class FYPeriod
                {
                    [YAXAttributeForClass]
                    public Int64 endCalYear { get; set; }
                    [YAXAttributeForClass]
                    public Int64 endMonth { get; set; }
                    [YAXAttributeForClass]
                    public Int64 fYear { get; set; }
                    [YAXAttributeForClass]
                    public string periodType { get; set; }

                    public class ConsEstimate
                    {
                        [YAXAttributeForClass]
                        public string dateType { get; set; }
                        [YAXValueForClass]
                        public float ConsValue { get; set; }
                    }

                    [YAXDictionary(EachPairName = "ConsEstimate", KeyName = "type",
                              SerializeKeyAs = YAXNodeTypes.Attribute, SerializeValueAs = YAXNodeTypes.Content)]
                    [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement)]
                    public Dictionary<string, ConsEstimate> ConsEstimates { get; set; }
                }

                [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement, EachElementName = "FYPeriod")]
                public List<FYPeriod> FYPeriods { get; set; }
            }

            [YAXCollection(YAXCollectionSerializationTypes.Recursive, EachElementName = "FYEstimate")]
            public List<FYEstimateData> FYEstimates { get; set; }

            public class NPEstimateData
            {
                [YAXAttributeForClass]
                public string type { get; set; }
                [YAXAttributeForClass]
                public string unit { get; set; }

                public class ConsEstimate
                {
                    [YAXAttributeForClass]
                    public string type { get; set; }

                    [YAXDictionary(EachPairName = "ConsValue", KeyName = "dateType",
                              SerializeKeyAs = YAXNodeTypes.Attribute, SerializeValueAs = YAXNodeTypes.Content)]
                    [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement)]
                    public Dictionary<string, float> ConsValues { get; set; }
                }

                [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement, EachElementName = "ConsEstimate")]
                public List<ConsEstimate> ConsEstimates { get; set; }
            }

            [YAXCollection(YAXCollectionSerializationTypes.Recursive, EachElementName = "NPEstimate")]
            public List<NPEstimateData> NPEstimates { get; set; }

            public class RecommendationsData
            {
                public class ConsOpinion
                {
                    [YAXAttributeForClass]
                    public string set { get; set; }

                    public class ConsOpValueData
                    {
                        [YAXAttributeForClass]
                        public string type { get; set; }

                        [YAXDictionary(EachPairName = "ConsValue", KeyName = "dateType",
                                  SerializeKeyAs = YAXNodeTypes.Attribute, SerializeValueAs = YAXNodeTypes.Content)]
                        [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement)]
                        public Dictionary<string, string> ConsValues { get; set; }
                    }
                    public ConsOpValueData ConsOpValue { get; set; }
                }
                [YAXCollection(YAXCollectionSerializationTypes.Recursive, EachElementName = "ConsOpinion")]
                public List<ConsOpinion> STOpinion { get; set; }
            }
            public RecommendationsData Recommendations { get; set; }
        }
        public ConsEstimatesData ConsEstimates { get; set; }
    }
}
