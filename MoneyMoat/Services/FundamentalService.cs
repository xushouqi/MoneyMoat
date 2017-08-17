using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using MoneyMoat.Messages;
using MoneyMoat.Types;
using MoneyModels;
using IBApi;
using YAXLib;

namespace MoneyMoat.Services
{
    class FundamentalService
    {

        private readonly ILogger m_logger;
        protected readonly IBClient ibClient;
        protected Dictionary<int, string> m_reqIds;
        protected int activeReqId = 0;

        public FundamentalService(IBClient ibclient,
                        ILogger<IBManager> logger)
        {
            m_logger = logger;
            ibClient = ibclient;

            ibClient.FundamentalData += HandleFundamentalsData;
        }

        private Dictionary<string, FundamentalsReportEnum> m_fundamentalReqs = new Dictionary<string, FundamentalsReportEnum>();
        public void RequestFundamentals(string symbol, ExchangeEnum exchange, FundamentalsReportEnum ftype)
        {
            if (m_fundamentalReqs.Count == 0)
            {
                m_fundamentalReqs[symbol] = ftype;

                var contract = Common.GetStockContract(symbol, exchange);
                ibClient.ClientSocket.reqFundamentalData(Common.GetReqId(symbol), contract, ftype.ToString(), new List<TagValue>());
            }
        }
        private void HandleFundamentalsData(FundamentalsMessage message)
        {
            foreach (var item in m_fundamentalReqs)
            {
                string symbol = item.Key;
                var ftype = item.Value;
                m_logger.LogInformation("HandleFundamentalsData: {0}: {1}", symbol, ftype.ToString());

                if (ftype == FundamentalsReportEnum.ReportsFinStatements)
                {
                    var ser = new YAXSerializer(typeof(ReportsFinStatements));
                    var obj = (ReportsFinStatements)ser.Deserialize(message.Data);
                }
                else if (ftype == FundamentalsReportEnum.ReportsFinSummary)
                {
                    var ser = new YAXSerializer(typeof(FinancialSummary));
                    var obj = (FinancialSummary)ser.Deserialize(message.Data);
                }
                else if (ftype == FundamentalsReportEnum.ReportSnapshot)
                {
                    var ser = new YAXSerializer(typeof(ReportSnapshot));
                    var obj = (ReportSnapshot)ser.Deserialize(message.Data);
                    var test = obj;
                }
                else if (ftype == FundamentalsReportEnum.RESC)
                {
                    var ser = new YAXSerializer(typeof(RESC));
                    var obj = (RESC)ser.Deserialize(message.Data);
                    var test = obj;
                }

                string filepath = Path.Combine(Directory.GetCurrentDirectory(), "Fundamentals", symbol, ftype.ToString() + ".xml");
                Common.WriteFile(filepath, message.Data);
                break;
            }
        }
    }
}
