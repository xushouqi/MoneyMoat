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
using Newtonsoft.Json;

namespace MoneyMoat.Services
{
    class FundamentalService : IBServiceBase<string>
    {
        private readonly ILogger m_logger;
        private readonly IRepository<Stock> m_repoStock;
        private readonly IRepository<Financal> m_repoFin;
        private readonly IRepository<FYEstimate> m_repoEst;
        private readonly IRepository<NPEstimate> m_repoNPE;
        private readonly IRepository<Recommendation> m_repoRecommend;

        public FundamentalService(IBClient ibclient,
                        IRepository<Stock> repoStock,
                        IRepository<Financal> repoFin,
                        IRepository<FYEstimate> repoEst,
                        IRepository<NPEstimate> repoNPE,
                        IRepository<Recommendation> repoRecommend,
                        ILogger<IBManager> logger) : base(ibclient)
        {
            m_logger = logger;
            m_repoStock = repoStock;
            m_repoFin = repoFin;
            m_repoEst = repoEst;
            m_repoNPE = repoNPE;
            m_repoRecommend = repoRecommend;

            ibClient.FundamentalData += HandleFundamentalsData;
        }

        public async Task UpdateAllStocks()
        {
            var stocklist = m_repoStock.GetAll();
            for (int i = 0; i < stocklist.Count; i++)
            {
                var stock = stocklist[i];
                await UpdateFundamentalsFromIB(stock.Symbol, stock.Exchange);
            }
        }

        public async Task UpdateFundamentalsFromXueQiu(string symbol)
        {
            var stock = m_repoStock.Find(symbol);
            if (stock != null)
            {
                var url = "http://xueqiu.com/v4/stock/quote.json?code=" + symbol;
                var source = await Common.GetXueQiuContent(url);
                if (!string.IsNullOrEmpty(source))
                {
                    JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                    {
                        DateTimeZoneHandling = DateTimeZoneHandling.Local,
                        DateFormatString = "ddd MMM dd HH:mm:ss zzz yyyy",
                    };

                    try
                    {
                        JObject o = JObject.Parse(source);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("UpdateHistoricalData Error: {0}\n{1}",
                             e.Message, e.StackTrace);
                    }
                }
            }
        }

        public async Task UpdateFundamentalsFromIB(string symbol, string exchange)
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
                        await m_repoFin.RemoveRangeAsync(t => t.Symbol == symbol);    
                        
                        //更新财务数据库
                        for (int i = 0; i < obj.Actuals.FYActuals.Count; i++)
                        {
                            var act = obj.Actuals.FYActuals[i];
                            for (int j = 0; j < act.FYPeriods.Count; j++)
                            {
                                var adata = act.FYPeriods[j];
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
                        await m_repoFin.SaveChangesAsync();

                        //先删除预测数据
                        await m_repoEst.RemoveRangeAsync(t => t.Symbol == symbol);

                        //更新预测数据库
                        for (int i = 0; i < obj.ConsEstimates.FYEstimates.Count; i++)
                        {
                            var act = obj.ConsEstimates.FYEstimates[i];
                            for (int j = 0; j < act.FYPeriods.Count; j++)
                            {
                                var adata = act.FYPeriods[j];

                                Dictionary<string, float> conValues = new Dictionary<string, float>();
                                for (int n = 0; n < adata.ConsEstimates.Count; n++)
                                {
                                    var cValue = adata.ConsEstimates[n];
                                    conValues[cValue.type] = cValue.ConsValue.ConsValue;
                                }

                                var sdata = new FYEstimate
                                {
                                    Symbol = symbol,
                                    type = act.type,
                                    fYear = adata.fYear,
                                    endMonth = adata.endMonth,
                                    periodType = adata.periodType,

                                    High = conValues["High"],
                                    Low = conValues["Low"],
                                    Mean = conValues["Mean"],
                                    Median = conValues["Median"],
                                    StdDev = conValues["StdDev"],
                                    NumOfEst = (int)conValues["NumOfEst"],
                                };
                                m_repoEst.Add(sdata);
                            }
                        }
                        await m_repoEst.SaveChangesAsync();

                        //先删除预测数据
                        await m_repoNPE.RemoveRangeAsync(t => t.Symbol == symbol);

                        for (int i = 0; i < obj.ConsEstimates.NPEstimates.Count; i++)
                        {
                            var act = obj.ConsEstimates.NPEstimates[i];
                            Dictionary<string, float> conValues = new Dictionary<string, float>();
                            for (int j = 0; j < act.ConsEstimates.Count; j++)
                            {
                                var adata = act.ConsEstimates[j];
                                if (adata.ConsValues.ContainsKey("CURR"))
                                    conValues[adata.type] = adata.ConsValues["CURR"];
                            }

                            var sdata = new NPEstimate
                            {
                                Symbol = symbol,
                                type = act.type,

                                High = conValues["High"],
                                Low = conValues["Low"],
                                Mean = conValues["Mean"],
                                Median = conValues["Median"],
                                StdDev = conValues["StdDev"],
                                NumOfEst = (int)conValues["NumOfEst"],
                                //UpGradings = (int)conValues["UpGradings"],
                                //DnGradings = (int)conValues["DnGradings"],
                            };
                            m_repoNPE.Add(sdata);
                        }
                        await m_repoEst.SaveChangesAsync();

                        //await m_repoRecommend.RemoveRangeAsync(t => t.Symbol == symbol);
                        //Dictionary<string, float> stValues = new Dictionary<string, float>();
                        //for (int i = 0; i < obj.ConsEstimates.Recommendations.STOpinion.Count; i++)
                        //{
                        //    var act = obj.ConsEstimates.Recommendations.STOpinion[i];
                        //    stValues[act.desc] = float.Parse(act.ConsOpValue.ConsValues["CURR"]);
                        //}
                        //var rdata = new Recommendation
                        //{
                        //    Symbol = symbol,

                        //    BUY = (int)stValues["BUY"],
                        //    SELL = (int)stValues["SELL"],
                        //    HOLD = (int)stValues["HOLD"],
                        //    UNDERPERFORM = (int)stValues["UNDERPERFORM"],
                        //    OUTPERFORM = (int)stValues["OUTPERFORM"],
                        //};
                        //m_repoRecommend.Add(rdata);
                        //await m_repoRecommend.SaveChangesAsync();
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
