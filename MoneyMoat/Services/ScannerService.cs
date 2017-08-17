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
    class ScannerService
    {
        private readonly ILogger m_logger;
        private readonly IBClient ibClient;
        private int activeReqId = 0;

        public ScannerService(IBClient ibclient,
                        ILogger<IBManager> logger)
        {
            m_logger = logger;
            ibClient = ibclient;

            ibClient.ScannerParameters += xml => HandleScannerParameters(new ScannerParametersMessage(xml));
            ibClient.ScannerData += HandleScannerData;
            ibClient.ScannerDataEnd += reqId => { m_logger.LogInformation("ScannerDataEnd. " + reqId + "\r\n"); activeReqId = 0; };
        }
        public void AddRequest(ScanCodeEnum scanCode, SecTypeEnum secType, StockTypeFilterEnum filter, int count)
        {
            if (activeReqId == 0)
            {
                activeReqId = Common.GetReqId(scanCode.ToString());

                ScannerSubscription subscription = new ScannerSubscription();
                subscription.ScanCode = scanCode.ToString();
                subscription.Instrument = secType.ToString();
                subscription.LocationCode = secType.ToString() + ".US.MAJOR";
                subscription.StockTypeFilter = filter.ToString();
                subscription.NumberOfRows = count;

                ibClient.ClientSocket.reqScannerSubscription(activeReqId, subscription, new List<TagValue>());
            }
        }
        public void CancelRequest()
        {
            if (activeReqId > 0)
            {
                ibClient.ClientSocket.cancelScannerSubscription(activeReqId);
                activeReqId = 0;
            }
        }

        public void RequestParameters()
        {
            ibClient.ClientSocket.reqScannerParameters();
        }

        private void HandleScannerParameters(ScannerParametersMessage scanParamsMessage)
        {
            m_logger.LogInformation("HandleScannerParameters: {0}", scanParamsMessage.XmlData);

            string filepath = Path.Combine(Directory.GetCurrentDirectory(), "Parameters", "ScannerParameters.xml");
            Common.WriteFile(filepath, scanParamsMessage.XmlData);
        }
        private void HandleScannerData(ScannerMessage scannMessage)
        {
            string scanCode = "";
            if (Common.CheckValidReqId(scannMessage.RequestId, out scanCode))
            {
                m_logger.LogInformation("HandleScannerData: Rank={0}, Summary={1}, Distance={2}, Benchmark={3}, Projection={4}, LegsStr={5}",
                    scannMessage.Rank, scannMessage.ContractDetails.Summary, scannMessage.Distance, scannMessage.Benchmark, scannMessage.Projection, scannMessage.LegsStr);
            }
        }
    }
}
