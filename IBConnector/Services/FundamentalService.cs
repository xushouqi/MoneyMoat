﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using IBConnector.Messages;
using IBApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CommonLibs;
using StockModels;

namespace IBConnector.Services
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


    }
}
