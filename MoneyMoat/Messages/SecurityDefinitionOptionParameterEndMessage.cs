﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoneyMoat.Messages
{
    class SecurityDefinitionOptionParameterEndMessage
    {
        private int reqId;

        public SecurityDefinitionOptionParameterEndMessage(int reqId)
        {
            this.reqId = reqId;
        }
    }
}
