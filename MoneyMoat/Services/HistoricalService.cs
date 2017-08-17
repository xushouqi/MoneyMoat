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
    class HistoricalService
    {
        private readonly ILogger m_logger;
        protected readonly IBClient ibClient;
        protected Dictionary<int, string> m_reqIds;
        protected int activeReqId = 0;

        public HistoricalService(IBClient ibclient,
                        ILogger<IBManager> logger)
        {
            m_logger = logger;
            ibClient = ibclient;

            ibClient.HeadTimestamp += HandleEarliestDataPoint;
        }

        public void RequestEarliestDataPoint(string symbol, ExchangeEnum exchange)
        {
            var contract = Common.GetStockContract(symbol, exchange);
            ibClient.ClientSocket.reqHeadTimestamp(Common.GetReqId(symbol), contract, "TRADES", 1, 1);
        }
        private void HandleEarliestDataPoint(HeadTimestampMessage message)
        {
            string symbol = string.Empty;
            if (Common.CheckValidReqId(message.ReqId, out symbol))
            {
                m_logger.LogInformation("HandleEarliestDataPoint: {0}={1}",
                     symbol, message.HeadTimestamp);
            }
        }
    }
}
