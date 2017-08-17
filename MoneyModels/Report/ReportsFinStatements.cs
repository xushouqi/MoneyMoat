using System;
using System.Collections.Generic;
using YAXLib;

namespace MoneyModels
{
    public class ReportsFinStatements
    {
        [YAXAttributeForClass]
        public string Revision { get; set; }

        public class Issue
        {
            [YAXAttributeFor(".")]
            [YAXSerializeAs("Type")]
            public string IssueType { get; set; }

            [YAXAttributeFor(".")]
            [YAXSerializeAs("Order")]
            public int IssueOrder { get; set; }

            //[YAXAttributeFor(".")]
            //[YAXSerializeAs("ID")]
            //public int IssueID { get; set; }

            [YAXAttributeFor(".")]
            [YAXSerializeAs("Desc")]
            public string IssueDesc { get; set; }

            [YAXAttributeFor("Exchange")]
            [YAXSerializeAs("Country")]
            public string ExchangeCountry { get; set; }

            [YAXAttributeFor("Exchange")]
            [YAXSerializeAs("Code")]
            public string ExchangeCode { get; set; }

            public string Exchange { get; set; }

            public string GlobalListingType { get; set; }

            [YAXAttributeFor("GlobalListingType")]
            public float SharesPerListing { get; set; }


            [YAXDictionary(EachPairName = "IssueID", KeyName = "Type",
                      SerializeKeyAs = YAXNodeTypes.Attribute, SerializeValueAs = YAXNodeTypes.Content)]
            [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement)]
            public Dictionary<string, string> IssueIDs { get; set; }
        }

        public List<Issue> Issues { get; set; }

        public class GeneralInfo
        {
            public string CoStatus { get; set; }
            public DateTime LastModified { get; set; }
            public DateTime LatestAvailableAnnual { get; set; }
            public DateTime LatestAvailableInterim { get; set; }


            [YAXAttributeFor("ReportingCurrency")]
            [YAXSerializeAs("Code")]
            public string ReportingCurrency { get; set; }

            [YAXAttributeFor("MostRecentExchange")]
            [YAXSerializeAs("Date")]
            public DateTime MostRecentExchangeDate { get; set; }
            public string MostRecentExchange { get; set; }
        }
        public GeneralInfo CoGeneralInfo { get; set; }

        public class Statement
        {
            [YAXAttributeFor("COAType")]
            [YAXSerializeAs("Code")]
            public string COAType { get; set; }
        }
        public Statement StatementInfo { get; set; }

        public class FinancialStatement
        {
            public class COAMap
            {
                [YAXDictionary(EachPairName = "mapItem", KeyName = "coaItem",
                          SerializeKeyAs = YAXNodeTypes.Attribute, SerializeValueAs = YAXNodeTypes.Content)]
                [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement)]
                public Dictionary<string, string> Items { get; set; }
            }
            [YAXSerializeAs("COAMap")]
            public COAMap COAMapItems { get; set; }

            public string GetCOAString(string key)
            {
                string value = "";
                COAMapItems.Items.TryGetValue(key, out value);
                return value;
            }

            public class FiscalPeriod
            {
                [YAXAttributeFor(".")]
                public DateTime EndDate { get; set; }
                [YAXAttributeFor(".")]
                public string FiscalYear { get; set; }
                [YAXAttributeFor(".")]
                public string Type { get; set; }

                public class Statement
                {
                    [YAXAttributeFor(".")]
                    public string Type { get; set; }

                    [YAXDictionary(EachPairName = "lineItem", KeyName = "coaCode",
                              SerializeKeyAs = YAXNodeTypes.Attribute, SerializeValueAs = YAXNodeTypes.Content)]
                    [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement)]
                    public Dictionary<string, float> Values { get; set; }
                }

                [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement, EachElementName = "Statement")]
                public List<Statement> Statements { get; set; }
            }

            [YAXSerializeAs("AnnualPeriods")]
            [YAXCollection(YAXCollectionSerializationTypes.Recursive, EachElementName = "FiscalPeriod")]
            public List<FiscalPeriod> FiscalPeriods { get; set; }
        }
        public FinancialStatement FinancialStatements { get; set; }

    }
}
