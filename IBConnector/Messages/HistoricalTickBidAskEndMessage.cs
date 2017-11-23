using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBConnector.Messages
{
    public class HistoricalTickBidAskEndMessage
    {
        public int ReqId { get; private set; }

        public HistoricalTickBidAskEndMessage(int reqId)
        {
            ReqId = reqId;
        }
    }
}
