using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using IBConnector.Messages;
using StockModels;
using CommonLibs;
using Polly;
using Polly.Bulkhead;

namespace IBConnector.Services
{
    [WebSocket]
    [WebApi]
    public class AnalyserService
    {
        private readonly ILogger m_logger;
        private readonly SymbolService m_symbolService;
        private readonly AppSettings m_config;

        private readonly IServiceProvider _services;

        private ConcurrentBag<CancellationTokenSource> m_cancelTokens = new ConcurrentBag<CancellationTokenSource>();

        public AnalyserService(IServiceProvider services,
                        SymbolService symbolService,
                        IOptions<AppSettings> options,
                        ILogger<IBManager> logger)
        {
            m_logger = logger;
            _services = services;
            m_symbolService = symbolService;
            m_config = options.Value;
        }

        //[Api(ActionId = 1000, Tips = "獲取股票代碼")]
        //public async Task<int> UpdateStockSymbolsFromSina(bool saveToDb)
        //{
        //    return await m_symbolService.UpdateSymbolsFromSina(saveToDb);
        //}

        /// <summary>
        /// 停止所有后台任务
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        [Api(ActionId = 1001, Tips = "停止所有后台任务")]
        public async Task<ReturnData<int>> StopAllTasks(int delay)
        {
            var retData = new ReturnData<int>(0);
            int count = m_cancelTokens.Count;
            foreach (var item in m_cancelTokens)
            {
                item.Cancel();
            }
            m_cancelTokens.Clear();
            retData.Data = count;
            return retData;
        }

        /// <summary>
        /// 更新所有基本面数据
        /// </summary>
        /// <param name="interval">启动间隔</param>
        /// <param name="count">同时启动数量</param>
        /// <returns></returns>
        //[Api(ActionId = 1002, Tips = "更新所有基本面数据")]
        //public async Task<ReturnData<int>> UpdateAllFundamentals(bool forceUpdate)
        //{
        //    var retData = new ReturnData<int>(0);
        //    var datalist = await m_symbolService.GetAllAsync();
        //    var cts = new CancellationTokenSource();
        //    try
        //    {
        //        Task task = new Task(() => DoUpdateAllFundamentals(datalist, forceUpdate, cts.Token).Wait(), cts.Token);
        //        task.Start();
        //    }
        //    catch (TaskCanceledException e)
        //    {
        //        m_logger.LogWarning("UpdateAllFundamentals Canceled! {0}", e.Message);
        //    }
        //    m_cancelTokens.Add(cts);
        //    retData.Data = datalist.Count;
        //    return retData;
        //}
        //private async Task DoUpdateAllFundamentals(List<Stock> datalist, bool forceUpdate, CancellationToken cancelToken)
        //{
        //    List<Action> actionlist = new List<Action>();
        //    var tasklist = new List<Task>();
        //    for (int i = 0; i < datalist.Count; i++)
        //    {
        //        var stock = datalist[i];
        //        var fundamentalService = (FundamentalService)_services.GetService(typeof(FundamentalService));

        //        var action = new Action(() => fundamentalService.UpdateAllFromIB(stock, forceUpdate, cancelToken).Wait());
        //        actionlist.Add(action);

        //        //Task task = fundamentalService.UpdateAllFromIB(stock, forceUpdate);
        //        //tasklist.Add(task);
        //        //await Task.Delay(m_config.TaskInterval);
        //        //if (tasklist.Count >= m_config.TaskMaxCount)
        //        //{
        //        //    int idx = Task.WaitAny(tasklist.ToArray());
        //        //    tasklist.RemoveAt(idx);
        //        //}
        //        //if (token.IsCancellationRequested)
        //        //    break;
        //    }
        //    Parallel.Invoke(new ParallelOptions(){ CancellationToken = cancelToken, MaxDegreeOfParallelism = m_config.TaskMaxCount }, actionlist.ToArray());
        //}

        [Api(ActionId = 1004)]
        public async Task<ReturnData<List<FinSummary>>> CalcFinSummary(string symbol)
        {
            var retData = new ReturnData<List<FinSummary>>();
            var repoSummary = (IRepository<FinSummary>)_services.GetService(typeof(IRepository<FinSummary>));
            var datalist = await repoSummary.WhereToArrayAsync(t=>t.Symbol == symbol);
            if (datalist != null && datalist.Length > 0)
            {
                var tmplist = new List<FinSummary>();

                int count = 0;
                var repoStatement = (IRepository<FinStatement>)_services.GetService(typeof(IRepository<FinStatement>));
                for (int i = 0; i < datalist.Length; i++)
                {
                    var sdata = datalist[i];
                    if (sdata.OTLO <= 0f)
                    {
                        string period = sdata.period == "12M" ? "Annual" : "Interim";
                        //找财报中的数据
                        var stateData = await FindStatementBySummary(repoStatement, symbol, period, sdata.asofDate, "OTLO");
                        if (stateData != null)
                        {
                            count++;
                            sdata.OTLO = stateData.Value;
                        }
                        stateData = await FindStatementBySummary(repoStatement, symbol, period, sdata.asofDate, "SCEX");
                        if (stateData != null)
                            sdata.SCEX = stateData.Value;
                        stateData = await FindStatementBySummary(repoStatement, symbol, period, sdata.asofDate, "SCSI");
                        if (stateData != null)
                            sdata.SCSI = stateData.Value;
                        stateData = await FindStatementBySummary(repoStatement, symbol, period, sdata.asofDate, "SNCC");
                        if (stateData != null)
                            sdata.SNCC = stateData.Value;

                        if (count > 0)
                        {
                            sdata.FreeCashFlow = sdata.OTLO - sdata.SCEX;

                            repoSummary.Update(sdata);
                            m_logger.LogWarning("UpdateFinSummary: {0}[{5}], OTLO={1}, SCEX={2}, SCSI={3}, SNCC={4}, date={6}", 
                                symbol, sdata.OTLO, sdata.SCEX, sdata.SCSI, sdata.SNCC, period, sdata.asofDate.ToShortDateString());
                            tmplist.Add(sdata);
                        }
                    }
                }
                if (count > 0)
                    await repoSummary.SaveChangesAsync();

                //计算同比
                count = 0;
                for (int i = 0; i < tmplist.Count; i++)
                {
                    var sdata = tmplist[i];
                    string period = sdata.period == "12M" ? "Annual" : "Interim";
                    var yoyData = await FindStatementBySummary(repoStatement, symbol, period, sdata.asofDate.AddYears(-1), "OTLO");
                    if (yoyData != null)
                        sdata.OTLOYoY = (sdata.OTLO - yoyData.Value) / yoyData.Value;

                    var cashyoyData = await repoSummary.FirstOrDefaultAsync(t => t.Symbol == symbol && t.period == sdata.period
                                        && t.asofDate.Year == sdata.asofDate.Year - 1 && t.asofDate.Month == sdata.asofDate.Month);
                    if (cashyoyData != null)
                        sdata.FreeCashFlowYoY = (sdata.FreeCashFlowYoY - cashyoyData.FreeCashFlowYoY) / cashyoyData.FreeCashFlowYoY;
                }
                retData.Data = tmplist;
                retData.ErrorCode = ErrorCodeEnum.Success;
            }
            return retData;
        }

        private async Task<FinStatement> FindStatementBySummary(IRepository<FinStatement> repoStatement, string symbol, string period, DateTime date, string coaCode)
        {
            if (period == "Annual")
                return await repoStatement.FirstOrDefaultAsync(t => t.Symbol == symbol && t.FiscalPeriod == period
                                              && t.EndDate.Year == date.Year
                                              && t.coaCode == coaCode);
            else
                return await repoStatement.FirstOrDefaultAsync(t => t.Symbol == symbol && t.FiscalPeriod == period
                                              && t.EndDate.Year == date.Year && t.EndDate.Month == date.Month
                                              && t.coaCode == coaCode);
        }

        //[Api(ActionId = 1005)]
        //public async Task<ReturnData<int>> UpdateAndCalcFundamental(string symbol)
        //{
        //    var retData = new ReturnData<int>(0);
        //    var fundamentalService = (FundamentalService)_services.GetService(typeof(FundamentalService));
        //    var ret = await fundamentalService.UpdateAllFromIB(symbol, true);
        //    var retSum = await CalcFinSummary(symbol);
        //    if (retSum.ErrorCode == ErrorCodeEnum.Success)
        //        retData.Data = retSum.Data.Count;
        //    return retData;
        //}
    }
}
