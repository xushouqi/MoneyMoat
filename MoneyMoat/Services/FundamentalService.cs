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
using Newtonsoft.Json.Linq;
using CommonLibs;

namespace MoneyMoat.Services
{
    [WebApi]
    public class FundamentalService : IBServiceBase<string>
    {
        private readonly ILogger m_logger;
        private readonly IRepository<Stock> m_repoStock;
        private readonly IRepository<Financal> m_repoFin;
        private readonly IRepository<FYEstimate> m_repoEst;
        private readonly IRepository<NPEstimate> m_repoNPE;
        private readonly IRepository<XueQiuData> m_repoDatas;
        private readonly IRepository<FinSummary> m_repoSummary;
        private readonly IRepository<FinStatement> m_repoStatement;

        public FundamentalService(IBManager ibmanager,
                        IRepository<Stock> repoStock,
                        IRepository<Financal> repoFin,
                        IRepository<FYEstimate> repoEst,
                        IRepository<NPEstimate> repoNPE,
                        IRepository<XueQiuData> repoDatas,
                        IRepository<FinSummary> repoSummary,
                        IRepository<FinStatement> repoStatement,
                        ILogger<IBManager> logger) : base(ibmanager)
        {
            m_logger = logger;
            m_repoStock = repoStock;
            m_repoFin = repoFin;
            m_repoEst = repoEst;
            m_repoNPE = repoNPE;
            m_repoDatas = repoDatas;
            m_repoSummary = repoSummary;
            m_repoStatement = repoStatement;

            ibClient.FundamentalData += HandleFundamentalsData;
        }

        [Api]
        public async Task<int> UpdateAllStocks()
        {
            int ret = 0;
            bool readXml = !ibClient.ClientSocket.IsConnected();

            var stocklist = m_repoStock.GetAll();
            for (int i = 0; i < stocklist.Count; i++)
            {
                var stock = stocklist[i];
                await UpdateFundamentalsFromIB(stock.Symbol, stock.Exchange, readXml);
                //await UpdateFundamentalsFromXueQiu(stock.Symbol);
                ret++;
            }
            return ret;
        }

        [Api]
        public async Task<string> UpdateFundamentalsFromXueQiu(string symbol)
        {
            string source = string.Empty;
            var stock = m_repoStock.Find(symbol);
            if (stock != null)
            {
                var url = "http://xueqiu.com/v4/stock/quote.json?code=" + symbol;
                source = await MoatCommon.GetXueQiuContent(url);
                if (!string.IsNullOrEmpty(source))
                {
                    JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                    {
                        DateTimeZoneHandling = DateTimeZoneHandling.Local,
                        DateFormatString = "ddd MMM dd HH:mm:ss zzz yyyy",
                    };

                    try
                    {
                        JObject jObj = JObject.Parse(source);
                        var content = jObj[symbol];
                        var data = JsonConvert.DeserializeObject<XueQiuQuote>(content.ToString());

                        //var last = await m_repoQuote.MaxAsync(t => t.Symbol == symbol, t => t.updateAt);
                        //if (last == null || last.updateAt < data.updateAt)
                        //{
                        //    m_repoQuote.Add(data);
                        //    await m_repoQuote.SaveChangesAsync();
                        //}
                    }
                    catch (Exception e)
                    {
                        m_logger.LogError("UpdateHistoricalData Error: {0}\n{1}",
                             e.Message, e.StackTrace);
                    }
                }
            }
            return source;
        }

        [Api]
        public async Task<int> UpdateFundamentalsFromIB(string symbol, bool readXml)
        {
            int ret = 0;
            var stock = m_repoStock.Find(symbol);
            if (stock != null)
            {
                 ret = await UpdateFundamentalsFromIB(symbol, stock.Exchange, readXml);
            }
            return ret;
        }
        public async Task<int> UpdateFundamentalsFromIB(string symbol, string exchange, bool readXml)
        {
            int ret = 0;
            //List<Task<string>> tasklist = new List<Task<string>>();
            foreach (FundamentalsReportEnum ftype in Enum.GetValues(typeof(FundamentalsReportEnum)))
            {
                await ReadAndParseFundamentalsAsync(symbol, ftype);
                ret ++;
                //Task<string> task = null;
                //if (readXml)
                //    task = ReadAndParseFundamentalsAsync(symbol, ftype);
                //else
                //    task = RequestAndParseFundamentalsAsync(symbol, exchange, ftype);
                //tasklist.Add(task);
            }
            //await Task.WhenAll(tasklist.ToArray());
            //for (int t = 0; t < tasklist.Count; t++)
            //{
            //    var data = tasklist[t].Result;
            //}
            return ret;
        }

        private async Task UpdateCalcDatas(string symbol)
        {
            var datalist = await m_repoSummary.WhereToListAsync(t => t.Symbol == symbol);
            for (int i = 0; i < datalist.Count; i++)
            {
                var data = datalist[i];
                //data.period
                //var statement = await m_repoStatement.MaxAsync
            }
        }

        private async Task ParseFundamentalData(string symbol, FundamentalsReportEnum ftype, string data, bool saveXml)
        {
            if (!string.IsNullOrEmpty(data) && data.Length > 100)
            {
                m_logger.LogWarning("ParseFundamentalData: {0}: {1}", symbol, ftype.ToString());

                if (saveXml)
                {
                    string filepath = MoatCommon.GetFundamentalFilePath(symbol, ftype);
                    MoatCommon.WriteFile(filepath, data);
                }

                try
                {
                    if (ftype == FundamentalsReportEnum.ReportsFinStatements)
                    {
                        var ser = new YAXSerializer(typeof(ReportsFinStatements));
                        var obj = (ReportsFinStatements)ser.Deserialize(data);

                        //先删除数据
                        await m_repoStatement.RemoveRangeAsync(t => t.Symbol == symbol);

                        for (int i = 0; i < obj.FinancialStatements.FiscalPeriods.Count; i++)
                        {
                            var fdata = obj.FinancialStatements.FiscalPeriods[i];
                            for (int j = 0; j < fdata.Statements.Count; j++)
                            {
                                var act = fdata.Statements[j];
                                foreach (var item in act.Values)
                                {
                                    string skey = item.Key;
                                    float svalue = item.Value;

                                    int fiscalYear = fdata.EndDate.Year;
                                    int.TryParse(fdata.FiscalYear, out fiscalYear);

                                    var sdata = new FinStatement
                                    {
                                        Symbol = symbol,
                                        FiscalPeriod = fdata.Type,
                                        FiscalYear = fiscalYear,
                                        EndDate = fdata.EndDate,
                                        coaCode = skey,
                                        Value = svalue,
                                    };
                                    m_repoStatement.Add(sdata);
                                }
                            }
                        }
                        await m_repoStatement.SaveChangesAsync();
                    }
                    else if (ftype == FundamentalsReportEnum.ReportsFinSummary)
                    {
                        var ser = new YAXSerializer(typeof(FinancialSummary));
                        var obj = (FinancialSummary)ser.Deserialize(data);

                        //先删除数据
                        await m_repoSummary.RemoveRangeAsync(t => t.Symbol == symbol);

                        Dictionary<string, FinSummary> tmpDatas = new Dictionary<string, FinSummary>();

                        for (int i = 0; i < obj.TotalRevenues.Count; i++)
                        {
                            var act = obj.TotalRevenues[i];
                            if (act.reportType != "P")
                            {
                                var sdata = new FinSummary
                                {
                                    Symbol = symbol,
                                    asofDate = act.asofDate,
                                    reportType = act.reportType,
                                    period = act.period,
                                    TotalRevenue = act.Value,
                                };
                                string skey = string.Concat(symbol, act.asofDate.ToShortDateString(), act.reportType, act.period);
                                tmpDatas[skey] = sdata;
                            }
                        }

                        List<FinSummary> annualList = new List<FinSummary>();
                        for (int i = 0; i < obj.EPSs.Count; i++)
                        {
                            var act = obj.EPSs[i];
                            string skey = string.Concat(symbol, act.asofDate.ToShortDateString(), act.reportType, act.period);
                            if (tmpDatas.ContainsKey(skey))
                            {
                                var sdata = tmpDatas[skey];
                                sdata.EPS = act.Value;

                                var lastData = await m_repoDatas.MaxAsync(t => t.Symbol == sdata.Symbol && t.time <= sdata.asofDate.AddDays(1), t => t.time);
                                if (lastData != null)
                                {
                                    sdata.Price = lastData.close;
                                    sdata.PE = sdata.Price / sdata.EPS;
                                }
                                m_repoSummary.Add(sdata);
                                if (sdata.PE > 0)
                                {
                                    if (sdata.reportType == "A")
                                        annualList.Add(sdata);
                                    else if (sdata.reportType == "R")
                                        annualList.Add(sdata);
                                }
                            }
                        }
                        await m_repoSummary.SaveChangesAsync();

                        for (int i = 0; i < annualList.Count; i++)
                        {
                            var anData = annualList[i];
                            var lastData = await m_repoSummary.MaxAsync(t => t.Symbol == anData.Symbol
                                                                && t.reportType == anData.reportType && t.period == anData.period
                                                                && t.asofDate < anData.asofDate, t => t.asofDate);
                            if (lastData != null && lastData.EPS > 0)
                            {
                                var peg = anData.PE / (100 * (anData.EPS - lastData.EPS) / lastData.EPS);
                                if (!float.IsNaN(peg))
                                {
                                    anData.PEG = peg;
                                    m_logger.LogError("PEG {0}={1}", symbol, peg);
                                    m_repoSummary.Update(anData);
                                }
                            }
                        }
                        await m_repoSummary.SaveChangesAsync();
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
                            await m_repoStock.SaveChangesAsync();
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
                            await m_repoStock.SaveChangesAsync();
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
                        await m_repoNPE.SaveChangesAsync();

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
                    m_logger.LogError("ParseFundamentalData Error: {0}\r\n{1}\r\n{2}", e.Message, e.StackTrace, e.InnerException.Message);
                }
            }
        }

        [Api]
        public async Task<string> ReadAndParseFundamentalsAsync(string symbol, FundamentalsReportEnum ftype)
        {
            string filepath = MoatCommon.GetFundamentalFilePath(symbol, ftype);            
            string data = await MoatCommon.ReadFile(filepath);
            if (!string.IsNullOrEmpty(data))
            {
                await ParseFundamentalData(symbol, ftype, data, false);
                return data;
            }
            else
                return string.Empty;
        }

        [Api]
        public async Task<string> RequestAndParseFundamentalsAsync(string symbol, FundamentalsReportEnum ftype)
        {
            var stock = m_repoStock.Find(symbol);
            if (stock != null)
                return await RequestAndParseFundamentalsAsync(symbol, stock.Exchange, ftype);
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
        public async Task<string> RequestFundamentalsAsync(string symbol, string exchange, FundamentalsReportEnum ftype)
        {
            int reqId = MoatCommon.GetReqId(symbol);
            var contract = MoatCommon.GetStockContract(symbol, exchange);
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
