using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using MoneyMoat.Messages;
using MoneyMoat.Types;
using MoneyModels;
using IBApi;
using Newtonsoft.Json;

namespace MoneyMoat.Services
{
    class HistoricalService : IBServiceBase<string>
    {
        public const int HISTORICAL_ID_BASE = 30000000;

        private readonly IRepository<Stock> m_repoStock;
        private readonly IRepository<XueQiuData> m_repoData;
        private readonly ILogger m_logger;
        private int activeReqId = 0;

        public HistoricalService(IBClient ibclient,
                        IRepository<Stock> repoStock,
                        IRepository<XueQiuData> repoData,
                        ILogger<IBManager> logger) : base(ibclient)
        {
            m_logger = logger;
            m_repoStock = repoStock;
            m_repoData = repoData;

            ibClient.HeadTimestamp += HandleEarliestDataPoint;
            ibClient.HistoricalData += HandleHistoricalData;
            ibClient.HistoricalDataEnd += HandleHistoricalDataEnd;
        }

        public async Task UpdateAllStocks()
        {
            var stocklist = m_repoStock.GetAll();
            for (int i = 0; i < stocklist.Count; i++)
            {
                var stock = stocklist[i];
                await UpdateHistoricalDataFromXueQiu(stock.Symbol);
            }
        }

        public async Task UpdateHistoricalDataFromXueQiu(string symbol)
        {
            var stock = m_repoStock.Find(symbol);
            if (stock != null)
            {
                long to = Common.GetTimestamp(DateTime.Now);
                long from = Common.GetTimestamp(stock.EarliestDate);

                //取已有数据的最新一条
                var retLas = await m_repoData.MaxAsync(t => t.Symbol == symbol, t => t.time);
                if (retLas != null)
                    from = Common.GetTimestamp(retLas.time.AddDays(1));

                var url = "https://xueqiu.com/stock/forchartk/stocklist.json?symbol=" + symbol + "&period=1day&type=normal&begin="+ from + "&end="+ to;
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
                        var obj = JsonConvert.DeserializeObject<XueQiuHistorical>(source);
                        if (obj.chartlist.Count > 0)
                        {
                            for (int i = 0; i < obj.chartlist.Count; i++)
                            {
                                var data = obj.chartlist[i];
                                data.Symbol = symbol;
                                m_repoData.Add(data);
                            }
                            await m_repoData.SaveChangesAsync();
                        }
                        Console.WriteLine("UpdateHistoricalData {0} Count={1}",
                             symbol, obj.chartlist.Count);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("UpdateHistoricalData Error: {0}\n{1}",
                             e.Message, e.StackTrace);
                    }
                }
            }
        }
        
        public async Task<string> RequestEarliestDataPointAsync(string symbol, string exchange)
        {
            var reqId = Common.GetReqId(symbol);
            var contract = Common.GetStockContract(symbol, exchange);
            ibClient.ClientSocket.reqHeadTimestamp(reqId, contract, "TRADES", 1, 1);
            return await SendRequestAsync(reqId);
        }

        private void HandleEarliestDataPoint(HeadTimestampMessage message)
        {
            string symbol = string.Empty;

            m_results[message.ReqId] = message.HeadTimestamp;
            HandleResponse(message.ReqId);

            Console.WriteLine("HandleEarliestDataPoint: {0}={1}",
                 symbol, message.HeadTimestamp);
        }
        public void CancelHeadTimestamp()
        {
            if (activeReqId > 0)
            {
                ibClient.ClientSocket.cancelHeadTimestamp(activeReqId);
                activeReqId = 0;
            }
        }

        private List<HistoricalDataMessage> historicalData;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="endDateTime"></param>
        /// <param name="durationString">? S D W M Y</param>
        /// <param name="barSizeSetting">
        /// 1 secs	5 secs	10 secs	15 secs	30 secs
        /// 1 min	2 mins	3 mins	5 mins	10 mins	15 mins	20 mins	30 mins
        /// 1 hour	2 hours	3 hours	4 hours	8 hours
        /// 1 day
        /// 1 week
        /// 1 month
        /// </param>
        /// <param name="whatToShow"></param>
        /// <param name="keepUpToDate">Whether a subscription is made to return updates of unfinished real time bars as they are available (True), or all data is returned on a one-time basis (False). Available starting with API v973.03+ and TWS v965+. If True, and endDateTime cannot be specified</param>
        public void AddRequest(string symbol, ExchangeEnum exchange, DateTime endDateTime, string durationString, string barSizeSetting, WhatToShowEnum whatToShow, bool keepUpToDate)
        {
            if (activeReqId == 0)
            {
                historicalData = new List<HistoricalDataMessage>();
                activeReqId = Common.GetReqId(symbol) + HISTORICAL_ID_BASE;
                var contract = Common.GetStockContract(symbol, exchange);
                var endStr = endDateTime.ToString("yyyyMMdd HH:mm:ss");
                //Whether (1) or not (0) to retrieve data generated only within Regular Trading Hours (RTH)
                int useRTH = 1;
                //The format in which the incoming bars' date should be presented. Note that for day bars, only yyyyMMdd format is available.
                int formatDate = 1;
                ibClient.ClientSocket.reqHistoricalData(activeReqId, contract, endStr, durationString, barSizeSetting, whatToShow.ToString(), useRTH, formatDate, keepUpToDate, new List<TagValue>());
            }
        }

        public void cancelHistoricalData()
        {
            if (activeReqId > 0)
            {
                ibClient.ClientSocket.cancelHistoricalData(activeReqId);
                activeReqId = 0;
            }
        }
        private void HandleHistoricalData(HistoricalDataMessage message)
        {
            historicalData.Add(message);
        }

        private void HandleHistoricalDataEnd(HistoricalDataEndMessage message)
        {
            string symbol = string.Empty;
            if (Common.CheckValidReqId(message.RequestId, out symbol))
            {
                activeReqId = 0;
                for (int i = 0; i < historicalData.Count; i++)
                {
                    var data = historicalData[i];
                    Console.WriteLine("HistoricalData: {0}, Date={1}, Open={2}, Close={3}, Count={4}, HasGaps={5}, High={6}, Low={7}, Volume={8}, Wap={9}", 
                        symbol, data.Date, data.Open, data.Close, data.Count, data.HasGaps, data.High, data.Low, data.Volume, data.Wap);
                }
            }
        }
    }
}
