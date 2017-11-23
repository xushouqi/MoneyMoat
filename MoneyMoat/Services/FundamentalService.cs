using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using MoneyMoat.Messages;
using IBApi;
using YAXLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CommonLibs;
using StockModels;

namespace MoneyMoat.Services
{
    [WebApi]
    public class FundamentalService : IBServiceBase<string>
    {
        private readonly SymbolService m_symbolService;
        private readonly HistoricalService m_historicalService;
        private readonly IRepository<Financal> m_repoFin;
        private readonly IRepository<FYEstimate> m_repoEst;
        private readonly IRepository<NPEstimate> m_repoNPE;
        private readonly IRepository<Historical> m_repoDatas;
        private readonly IRepository<FinSummary> m_repoSummary;
        private readonly IRepository<FinStatement> m_repoStatement;

        public FundamentalService(IBManager ibmanager,
                        CommonManager commonManager,
                        SymbolService symbolService,
                        HistoricalService historicalService,
                        IRepository<Financal> repoFin,
                        IRepository<FYEstimate> repoEst,
                        IRepository<NPEstimate> repoNPE,
                        IRepository<Historical> repoDatas,
                        IRepository<FinSummary> repoSummary,
                        IRepository<FinStatement> repoStatement,
                        ILogger<IBManager> logger) : base(ibmanager, logger, commonManager)
        {
            m_symbolService = symbolService;
            m_historicalService = historicalService;
            m_repoFin = repoFin;
            m_repoEst = repoEst;
            m_repoNPE = repoNPE;
            m_repoDatas = repoDatas;
            m_repoSummary = repoSummary;
            m_repoStatement = repoStatement;

            ibClient.FundamentalData += HandleFundamentalsData;
        }

        [Api]
        public async Task<string> UpdateFundamentalsFromXueQiu(string symbol)
        {
            string source = string.Empty;
            var stock = await m_symbolService.FindAsync(symbol);
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

        /// <summary>
        /// 从IB更新指定股票的所有文件
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        [Api]
        public async Task<int> UpdateAllFromIB(string symbol, bool forceUpdate = false)
        {
            int ret = 0;
            var stock = await m_symbolService.FindAsync(symbol);
            if (stock != null)
                 ret = await UpdateAllFromIB(stock, forceUpdate);
            return ret;
        }
        public async Task<int> UpdateAllFromIB(Stock stock, bool forceUpdate)
        {
            return await UpdateAllFromIB(stock, forceUpdate, CancellationToken.None);
        }
        public async Task<int> UpdateAllFromIB(Stock stock, bool forceUpdate, CancellationToken cancelToken)
        {
            int ret = 0;
            foreach (FundamentalsReportEnum ftype in Enum.GetValues(typeof(FundamentalsReportEnum)))
            {
                await RequestFromIBAsync(stock.Symbol, stock.Exchange, ftype, forceUpdate);
                ret ++;
                if (cancelToken != null && cancelToken.IsCancellationRequested)
                    break;
            }
            return ret;
        }
        
        /// <summary>
        /// 从xml读取文件
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="ftype"></param>
        /// <returns></returns>
        [Api]
        public async Task<string> ReadFromXmlAsync(string symbol, FundamentalsReportEnum ftype)
        {
            string filepath = MoatCommon.GetFundamentalFilePath(symbol, ftype);
            return await MoatCommon.ReadFile(filepath);
        }
        
        /// <summary>
        /// 从IB请求文件
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="exchange"></param>
        /// <param name="ftype"></param>
        /// <returns></returns>
        [Api]
        public async Task<string> RequestFromIBAsync(string symbol, string exchange, FundamentalsReportEnum ftype, bool forceUpdate = false)
        {
            int reqId = m_commonManager.GetReqId(symbol);
            var contract = MoatCommon.GetStockContract(symbol, exchange);
            var data = await SendRequestAsync(reqId, ()=> m_clientSocket.reqFundamentalData(reqId, contract, ftype.ToString(), new List<TagValue>()));
            if (!string.IsNullOrEmpty(data))
            {
                var md5 = MD5.Create();
                byte[] result = md5.ComputeHash(System.Text.Encoding.Unicode.GetBytes(data));
                var strResult = BitConverter.ToString(result);

                //是否需要保存xml
                bool needSave = true;
                string filename = MoatCommon.GetFundamentalFilePath(symbol, ftype);
                if (File.Exists(filename))
                {
                    string xmlData = await MoatCommon.ReadFile(filename);
                    byte[] result2 = md5.ComputeHash(System.Text.Encoding.Unicode.GetBytes(xmlData));
                    var strResult2 = BitConverter.ToString(result2);
                    needSave = !strResult.Equals(strResult2);
                }
                if (needSave || forceUpdate)
                {
                    MoatCommon.WriteFile(filename, data);
                    //后台分析并保存数据库
                    await ParseFundamentalToDb(symbol, ftype, data);
                }
                else
                    m_logger.LogWarning("{0}.{1} already exists!", symbol, ftype.ToString());
            }
            return data;
        }

        private void HandleFundamentalsData(FundamentalsMessage message)
        {
            m_results[message.ReqId] = message.Data;
            HandleResponse(message.ReqId);
        }

        /// <summary>
        /// 分析xml并保存数据库
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="ftype"></param>
        /// <returns></returns>
        [Api]
        public async Task<string> ReadParseFundamentalToDbBackend(string symbol, FundamentalsReportEnum ftype)
        {
            var data = await ReadFromXmlAsync(symbol, ftype);
            if (!string.IsNullOrEmpty(data))
            {
                await ParseFundamentalToDb(symbol, ftype, data);
            }
            return data;
        }

        private async Task ParseFundamentalToDb(string symbol, FundamentalsReportEnum ftype, string data)
        {
            if (!string.IsNullOrEmpty(data) && data.Length > 100)
            {
                try
                {
                    if (ftype == FundamentalsReportEnum.ReportsFinStatements)
                    {
                        var ser = new YAXSerializer(typeof(ReportsFinStatements));
                        var obj = (ReportsFinStatements)ser.Deserialize(data);

                        //先删除数据
                        await m_repoStatement.RemoveWhereAsync(t => t.Symbol == symbol);

                        int count = 0;
                        for (int i = 0; i < obj.FinancialStatements.AnnualPeriods.Count; i++)
                        {
                            var fdata = obj.FinancialStatements.AnnualPeriods[i];
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
                                    count++;
                                    if (count % 10 == 0)
                                    {
                                        await m_repoStatement.SaveChangesAsync();
                                        m_logger.LogWarning("{0}.FinStatement Saved={1}", symbol, count);
                                        count = 0;
                                    }
                                }
                            }
                        }

                        for (int i = 0; i < obj.FinancialStatements.InterimPeriods.Count; i++)
                        {
                            var fdata = obj.FinancialStatements.InterimPeriods[i];
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
                                    count++;
                                    if (count % 10 == 0)
                                    {
                                        await m_repoStatement.SaveChangesAsync();
                                        m_logger.LogWarning("{0}.FinStatement Saved={1}", symbol, count);
                                        count = 0;
                                    }
                                }
                            }
                        }
                        if (count > 0)
                            await m_repoStatement.SaveChangesAsync();
                        m_logger.LogWarning("{0}.FinStatement Saved={1}", symbol, count);
                    }
                    else if (ftype == FundamentalsReportEnum.ReportsFinSummary)
                    {
                        var ser = new YAXSerializer(typeof(FinancialSummary));
                        var obj = (FinancialSummary)ser.Deserialize(data);

                        //先删除数据
                        await m_repoSummary.RemoveWhereAsync(t => t.Symbol == symbol);

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

                        //最新的历史
                        var lastHistorical = await m_historicalService.UpdateHistoricalDataFromXueQiu(symbol);

                        int count = 0;
                        //保存EPS
                        List<FinSummary> annualList = new List<FinSummary>();
                        for (int i = 0; i < obj.EPSs.Count; i++)
                        {
                            var act = obj.EPSs[i];
                            string skey = string.Concat(symbol, act.asofDate.ToShortDateString(), act.reportType, act.period);
                            if (tmpDatas.ContainsKey(skey))
                            {
                                var sdata = tmpDatas[skey];
                                if (sdata.reportType == "R")
                                {
                                    sdata.EPS = act.Value;

                                    //最新的历史价格大于此日期，尝试找当时的价格
                                    if (lastHistorical != null && lastHistorical.time >= sdata.asofDate)
                                    {
                                        var lastData = await m_repoDatas.MaxAsync(t => t.Symbol == sdata.Symbol && t.time <= sdata.asofDate.AddDays(1), t => t.time);
                                        if (lastData != null)
                                        {
                                            sdata.Price = lastData.close;
                                            sdata.PE = sdata.Price / sdata.EPS;
                                        }
                                    }

                                    m_repoSummary.Add(sdata);
                                    count++;
                                    if (count % 10 == 0)
                                    {
                                        await m_repoSummary.SaveChangesAsync();
                                        m_logger.LogWarning("{0}.FinSummary Saved={1}", symbol, count);
                                        count = 0;
                                    }

                                    //需要计算PEG
                                    if (sdata.PE > 0)
                                        annualList.Add(sdata);
                                }
                            }
                        }
                        if (count > 0)
                            await m_repoSummary.SaveChangesAsync();
                        m_logger.LogWarning("{0}.FinSummary Saved={1}", symbol, count);

                        if (lastHistorical != null)
                        {
                            count = 0;
                            //计算同比数值
                            for (int i = 0; i < annualList.Count; i++)
                            {
                                var anData = annualList[i];
                                if (anData.PE > 0)
                                {
                                    //同比
                                    var lastData = await m_repoSummary.MaxAsync(t => t.Symbol == anData.Symbol
                                                                        && t.reportType == anData.reportType && t.period == anData.period
                                                                        && t.asofDate.Year == anData.asofDate.Year - 1
                                                                        && t.asofDate.Month == anData.asofDate.Month
                                                                        , t => t.asofDate);
                                    if (lastData != null && lastData.EPS > 0)
                                    {
                                        var priceYoY = (anData.Price - lastData.Price) / lastData.Price;
                                        if (!float.IsNaN(priceYoY) && !float.IsInfinity(priceYoY))
                                            anData.PriceYoY = priceYoY;
                                        var peYoY = (anData.PE - lastData.PE) / lastData.PE;
                                        if (!float.IsNaN(peYoY) && !float.IsInfinity(peYoY))
                                            anData.PEYoY = peYoY;
                                        var peg = anData.PE / (100 * (anData.EPS - lastData.EPS) / lastData.EPS);
                                        if (!float.IsNaN(peg) && !float.IsInfinity(peg))
                                            anData.PEG = peg;

                                        m_logger.LogWarning("{0}: PEYoY={5} PriceYoY={6} PEG={1}, reportType={2}, period={3}, asofDate={4}",
                                                symbol, peg, lastData.reportType, lastData.period, lastData.asofDate.ToShortDateString(), peYoY, priceYoY);
                                        m_repoSummary.Update(anData);
                                        count++;
                                        if (count % 10 == 0)
                                        {
                                            await m_repoSummary.SaveChangesAsync();
                                            m_logger.LogWarning("{0}.FinSummary Updated={1}", symbol, count);
                                            count = 0;
                                        }
                                    }
                                }
                            }
                            if (count > 0)
                                await m_repoSummary.SaveChangesAsync();
                            m_logger.LogWarning("{0}.FinSummary Update PEGs={1}", symbol, count);
                        }
                    }
                    else if (ftype == FundamentalsReportEnum.ReportSnapshot)
                    {
                        var ser = new YAXSerializer(typeof(ReportSnapshot));
                        var obj = (ReportSnapshot)ser.Deserialize(data);

                        //更新数据库
                        var stock = await m_symbolService.FindAsync(symbol);
                        if (stock != null)
                        {
                            stock.CommonShareholders = obj.CoGeneralInfo.CommonShareholders;
                            stock.Employees = obj.CoGeneralInfo.Employees;
                            await m_symbolService.Update(stock);
                        }
                    }
                    else if (ftype == FundamentalsReportEnum.RESC)
                    {
                        var ser = new YAXSerializer(typeof(RESC));
                        var obj = (RESC)ser.Deserialize(data);
                        var test = obj;

                        //更新数据库
                        var stock = await m_symbolService.FindAsync(symbol);
                        if (stock != null)
                        {
                            if (obj.Company.SecurityInfo.Security.MarketData.ContainsKey("SHARESOUT"))
                                stock.SharesOut = (Int64)obj.Company.SecurityInfo.Security.MarketData["SHARESOUT"];
                            if (obj.Company.SecurityInfo.Security.MarketData.ContainsKey("MARKETCAP"))
                                stock.MarketCap = (Int64)obj.Company.SecurityInfo.Security.MarketData["MARKETCAP"];
                            await m_symbolService.Update(stock);
                        }

                        //先删除财务数据
                        await m_repoFin.RemoveWhereAsync(t => t.Symbol == symbol);

                        int count = 0;
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
                                count++;
                            }
                        }
                        await m_repoFin.SaveChangesAsync();
                        m_logger.LogWarning("{0}.Financal Saved={1}", symbol, count);

                        //先删除预测数据
                        await m_repoEst.RemoveWhereAsync(t => t.Symbol == symbol);

                        count = 0;
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
                                count++;
                            }
                        }
                        await m_repoEst.SaveChangesAsync();
                        m_logger.LogWarning("{0}.FYEstimate Saved={1}", symbol, count);

                        //先删除预测数据
                        await m_repoNPE.RemoveWhereAsync(t => t.Symbol == symbol);

                        count = 0;
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
                            count++;
                        }
                        await m_repoNPE.SaveChangesAsync();
                        m_logger.LogWarning("{0}.FYEstimate Saved={1}", symbol, count);

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
                catch (Exception e)
                {
                    m_logger.LogError("ParseFundamentalData Error: {0}\r\n{1}\r\n{2}", e.Message, e.StackTrace, e.InnerException.Message);
                }
            }
        }

    }
}
