using System;
using System.Collections.Generic;
using YAXLib;

namespace MoneyMoat
{
    public class FinancialSummary
    {
        public class Revenue
        {
            [YAXAttributeFor(".")]
            public DateTime asofDate { get; set; }
            [YAXAttributeFor(".")]
            public string reportType { get; set; }
            [YAXAttributeFor(".")]
            public string period { get; set; }

            [YAXValueForClass]
            public float Value { get; set; }
        }

        [YAXAttributeFor("TotalRevenues")]
        public string currency { get; set; }

        [YAXCollection(YAXCollectionSerializationTypes.Recursive, EachElementName = "TotalRevenue")]
        public List<Revenue> TotalRevenues { get; set; }


        [YAXCollection(YAXCollectionSerializationTypes.Recursive, EachElementName = "EPS")]
        public List<Revenue> EPSs { get; set; }
    }
}
