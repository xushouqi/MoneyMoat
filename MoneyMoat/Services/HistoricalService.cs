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
        private readonly IBClient ibClient;
        private int activeReqId = 0;

        public HistoricalService(IBClient ibclient,
                        ILogger<IBManager> logger)
        {
            m_logger = logger;
            ibClient = ibclient;

            ibClient.HeadTimestamp += HandleEarliestDataPoint;
        }

        public void RequestEarliestDataPoint(string symbol, ExchangeEnum exchange)
        {
            if (activeReqId == 0)
            {
                activeReqId = Common.GetReqId(symbol);
                var contract = Common.GetStockContract(symbol, exchange);
                ibClient.ClientSocket.reqHeadTimestamp(activeReqId, contract, "TRADES", 1, 1);
            }
        }
        public void CancelHeadTimestamp()
        {
            if (activeReqId > 0)
            {
                ibClient.ClientSocket.cancelHeadTimestamp(activeReqId);
                activeReqId = 0;
            }
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
