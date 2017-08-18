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
    class FundamentalService : IBServiceBase<string>
    {
        private readonly ILogger m_logger;
        private readonly IRepository<Stock> m_repoStock;

        public FundamentalService(IBClient ibclient,
                        IRepository<Stock> repoStock,
                        ILogger<IBManager> logger) : base(ibclient)
        {
            m_logger = logger;
            m_repoStock = repoStock;

            ibClient.FundamentalData += HandleFundamentalsData;
        }

        public async Task UpdateAllStocksFundamentals()
        {
            var stocklist = m_repoStock.GetAll();
            for (int i = 0; i < stocklist.Count; i++)
            {
                var stock = stocklist[i];
                List<Task<string>> tasklist = new List<Task<string>>();
                foreach (FundamentalsReportEnum ftype in Enum.GetValues(typeof(FundamentalsReportEnum)))
                {
                    Task<string> task = RequestAndParseFundamentalsAsync(stock.Symbol, stock.Exchange, ftype);
                    tasklist.Add(task);
                }
                await Task.WhenAll(tasklist.ToArray());
                for (int t = 0; t < tasklist.Count; t++)
                {
                    var data = tasklist[t].Result;
                }
            }
        }

        private void ParseFundamentalData(string symbol, FundamentalsReportEnum ftype, string data)
        {
            if (!string.IsNullOrEmpty(data) && data.Length > 100)
            {
                m_logger.LogInformation("HandleFundamentalsData: {0}: {1}", symbol, ftype.ToString());

                string filepath = Path.Combine(Directory.GetCurrentDirectory(), "Fundamentals", symbol, ftype.ToString() + ".xml");
                Common.WriteFile(filepath, data);

                if (ftype == FundamentalsReportEnum.ReportsFinStatements)
                {
                    var ser = new YAXSerializer(typeof(ReportsFinStatements));
                    var obj = (ReportsFinStatements)ser.Deserialize(data);
                }
                else if (ftype == FundamentalsReportEnum.ReportsFinSummary)
                {
                    var ser = new YAXSerializer(typeof(FinancialSummary));
                    var obj = (FinancialSummary)ser.Deserialize(data);
                }
                else if (ftype == FundamentalsReportEnum.ReportSnapshot)
                {
                    var ser = new YAXSerializer(typeof(ReportSnapshot));
                    var obj = (ReportSnapshot)ser.Deserialize(data);
                    var test = obj;
                }
                else if (ftype == FundamentalsReportEnum.RESC)
                {
                    var ser = new YAXSerializer(typeof(RESC));
                    var obj = (RESC)ser.Deserialize(data);
                    var test = obj;
                }
                else if (ftype == FundamentalsReportEnum.ReportsOwnership)
                {
                    var ser = new YAXSerializer(typeof(OwnershipDetails));
                    var obj = (OwnershipDetails)ser.Deserialize(data);
                    var test = obj;
                }
            }
        }

        public async Task<string> RequestAndParseFundamentalsAsync(string symbol, string exchange, FundamentalsReportEnum ftype)
        {
            var data = await RequestFundamentalsAsync(symbol, exchange.ToString(), ftype);
            if (!string.IsNullOrEmpty(data))
            {
                ParseFundamentalData(symbol, ftype, data);
                return data;
            }
            else
                return "";
        }
        public async Task<string> RequestFundamentalsAsync(string symbol, ExchangeEnum exchange, FundamentalsReportEnum ftype)
        {
            return await RequestFundamentalsAsync(symbol, exchange.ToString(), ftype);
        }
        public async Task<string> RequestFundamentalsAsync(string symbol, string exchange, FundamentalsReportEnum ftype)
        {
            int reqId = Common.GetReqId(symbol);
            var contract = Common.GetStockContract(symbol, exchange);
            ibClient.ClientSocket.reqFundamentalData(reqId, contract, ftype.ToString(), new List<TagValue>());
            return await SendRequestAsync(reqId);
        }
        private void HandleFundamentalsData(FundamentalsMessage message)
        {
            m_results[message.ReqId] = message.Data;
            HandleResponse(message.ReqId);
        }

    }
}
