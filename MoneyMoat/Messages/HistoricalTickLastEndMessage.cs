﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoneyMoat.Messages
{
    public class HistoricalTickLastEndMessage
    {
        public int ReqId { get; private set; }

        public HistoricalTickLastEndMessage(int reqId)
        {
            ReqId = reqId;
        }
    }
}
