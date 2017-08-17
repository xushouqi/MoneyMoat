using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using MoneyMoat.Messages;
using IBApi;

namespace MoneyMoat.Services
{
    class SymbolSamplesService
    {
        private readonly ILogger m_logger;
        private readonly IBClient ibClient;
        private int activeReqId = 0;

        public SymbolSamplesService(IBClient ibclient,
                        ILogger<IBManager> logger)
        {
            m_logger = logger;
            ibClient = ibclient;

            ibClient.SymbolSamples += HandleSymbolSamplesData;
        }

        public void RequestSymbols(string pattern)
        {
            ibClient.ClientSocket.reqMatchingSymbols(Common.GetReqId(pattern), pattern);                        
        }
        private void HandleSymbolSamplesData(SymbolSamplesMessage dataMessage)
        {
            var datalist = new List<ContractDescription>();
            string symbol = string.Empty;
            if (Common.CheckValidReqId(dataMessage.ReqId, out symbol))
            {
                foreach (ContractDescription contractDescription in dataMessage.ContractDescriptions)
                {
                    datalist.Add(contractDescription);

                    var derivSecTypes = "";
                    foreach (var derivSecType in contractDescription.DerivativeSecTypes)
                    {
                        derivSecTypes += derivSecType;
                        derivSecTypes += " ";
                    }

                    m_logger.LogInformation("Contract: conId - {0}, symbol - {1}, secType - {2}, primExchange - {3}, currency - {4}, derivativeSecTypes - {5}",
                         contractDescription.Contract.ConId, contractDescription.Contract.Symbol, contractDescription.Contract.SecType,
                         contractDescription.Contract.PrimaryExch, contractDescription.Contract.Currency, derivSecTypes);
                }
            }
        }
    }
}
