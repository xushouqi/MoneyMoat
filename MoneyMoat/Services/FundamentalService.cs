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
        private readonly IRepository<Financal> m_repoFin;

        public FundamentalService(IBClient ibclient,
                        IRepository<Stock> repoStock,
                        IRepository<Financal> repoFin,
                        ILogger<IBManager> logger) : base(ibclient)
        {
            m_logger = logger;
            m_repoStock = repoStock;
            m_repoFin = repoFin;

            ibClient.FundamentalData += HandleFundamentalsData;
        }

        public async Task UpdateAllStocksFundamentals()
        {
            var stocklist = m_repoStock.GetAll();
            for (int i = 0; i < stocklist.Count; i++)
            {
                var stock = stocklist[i];
                await DoUpdateStockFundamentals(stock.Symbol, stock.Exchange);
            }
        }

        public async Task DoUpdateStockFundamentals(string symbol, string exchange)
        {
            List<Task<string>> tasklist = new List<Task<string>>();
            foreach (FundamentalsReportEnum ftype in Enum.GetValues(typeof(FundamentalsReportEnum)))
            {
                Task<string> task = null;
                if (ibClient.ClientSocket.IsConnected())
                    task = RequestAndParseFundamentalsAsync(symbol, exchange, ftype);
                else
                    task = ReadAndParseFundamentalsAsync(symbol, ftype);
                tasklist.Add(task);
            }
            await Task.WhenAll(tasklist.ToArray());
            for (int t = 0; t < tasklist.Count; t++)
            {
                var data = tasklist[t].Result;
            }
        }

        private async Task ParseFundamentalData(string symbol, FundamentalsReportEnum ftype, string data, bool saveXml)
        {
            if (!string.IsNullOrEmpty(data) && data.Length > 100)
            {
                m_logger.LogInformation("HandleFundamentalsData: {0}: {1}", symbol, ftype.ToString());

                if (saveXml)
                {
                    string filepath = Common.GetFundamentalFilePath(symbol, ftype);
                    Common.WriteFile(filepath, data);
                }

                try
                {
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

                        //更新数据库
                        var stock = m_repoStock.Find(symbol);
                        if (stock != null)
                        {
                            stock.CommonShareholders = obj.CoGeneralInfo.CommonShareholders;
                            stock.Employees = obj.CoGeneralInfo.Employees;
                            m_repoStock.Update(stock);
                        }
                    }
                    else if (ftype == FundamentalsReportEnum.RESC)
                    {
                        var ser = new YAXSerializer(typeof(RESC));
                        var obj = (RESC)ser.Deserialize(data);
                        var test = obj;

                        //更新数据库
                        var stock = m_repoStock.Find(symbol);
                        if (stock != null)
                        {
                            if (obj.Company.SecurityInfo.Security.MarketData.ContainsKey("SHARESOUT"))
                                stock.SharesOut = (Int64)obj.Company.SecurityInfo.Security.MarketData["SHARESOUT"];
                            if (obj.Company.SecurityInfo.Security.MarketData.ContainsKey("MARKETCAP"))
                                stock.MarketCap = (Int64)obj.Company.SecurityInfo.Security.MarketData["MARKETCAP"];
                            m_repoStock.Update(stock);                            
                        }

                        //先删除财务数据
                        await m_repoFin.RemoveRangeAsync(t => t.Symbol.Equals(symbol) && t.Id > 0);    
                        
                        //更新财务数据库
                        for (int i = 0; i < obj.Actuals.FYActuals.Count; i++)
                        {
                            var act = obj.Actuals.FYActuals[i];
                            for (int j = 0; j < act.FYPeriods.Count; j++)
                            {
                                var adata = act.FYPeriods[j];
                                //bool already = await m_repoFin.Any(t => t.Symbol.Equals(symbol) && t.type.Equals(act.type)
                                //    && t.fYear == adata.fYear && t.endMonth == adata.endMonth
                                //    && t.periodType == adata.periodType);
                                //if (!already)
                                {
                                    var sdata = new Financal
                                    {
                                        Symbol = symbol,
                                        type = act.type,
                                        fYear = adata.fYear,
                                        endMonth = adata.endMonth,
                                        periodType = adata.periodType,
                                        Value = adata.ActValue,
                                    };
                                    m_repoFin.Add(sdata);
                                }
                            }
                        }
                        await m_repoFin.SaveChangesAsync();
                    }
                    else if (ftype == FundamentalsReportEnum.ReportsOwnership)
                    {
                        var ser = new YAXSerializer(typeof(OwnershipDetails));
                        var obj = (OwnershipDetails)ser.Deserialize(data);
                        var test = obj;
                    }
                }
                catch(Exception e)
                {
                    m_logger.LogError("ParseFundamentalData Error: {0}, \r\n{1}", e.Message, e.StackTrace);
                }
            }
        }

        public async Task<string> ReadAndParseFundamentalsAsync(string symbol, FundamentalsReportEnum ftype)
        {
            string filepath = Common.GetFundamentalFilePath(symbol, ftype);            
            string data = await Common.ReadFile(filepath);
            if (!string.IsNullOrEmpty(data))
            {
                await ParseFundamentalData(symbol, ftype, data, false);
                return data;
            }
            else
                return string.Empty;
        }
        public async Task<string> RequestAndParseFundamentalsAsync(string symbol, string exchange, FundamentalsReportEnum ftype)
        {
            var data = await RequestFundamentalsAsync(symbol, exchange.ToString(), ftype);
            if (!string.IsNullOrEmpty(data))
            {
                await ParseFundamentalData(symbol, ftype, data, true);
                return data;
            }
            else
                return string.Empty;
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
