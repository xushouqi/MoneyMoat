using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoneyMoat.Messages
{
    public class HistoricalTickEndMessage
    {
        public int ReqId { get; private set; }

        public HistoricalTickEndMessage(int reqId)
        {
            ReqId = reqId;
        }
    }
}
