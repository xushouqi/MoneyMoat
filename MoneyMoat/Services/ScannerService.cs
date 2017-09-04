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
using CommonLibs;

namespace MoneyMoat.Services
{
    [WebApi]
    public class ScannerService : IBServiceBase<string>
    {
        private int activeReqId = 0;

        public ScannerService(IBManager ibmanager,
                        CommonManager commonManager,
                        ILogger<IBManager> logger) : base(ibmanager, logger, commonManager)
        {
            ibClient.ScannerParameters += xml => HandleScannerParameters(new ScannerParametersMessage(xml));
            ibClient.ScannerData += HandleScannerData;
            ibClient.ScannerDataEnd += reqId => { m_logger.LogInformation("ScannerDataEnd. " + reqId + "\r\n"); activeReqId = 0; };
        }

        public void AddRequest(ScanCodeEnum scanCode, SecTypeEnum secType, StockTypeFilterEnum filter, int count)
        {
            if (activeReqId == 0)
            {
                activeReqId = MoatCommon.GetReqId(scanCode.ToString());

                ScannerSubscription subscription = new ScannerSubscription();
                subscription.ScanCode = scanCode.ToString();
                subscription.Instrument = secType.ToString();
                subscription.LocationCode = secType.ToString() + ".US";
                subscription.StockTypeFilter = filter.ToString();
                subscription.NumberOfRows = count;

                m_clientSocket.reqScannerSubscription(activeReqId, subscription, new List<TagValue>());
            }
        }
        public void CancelRequest()
        {
            if (activeReqId > 0)
            {
                m_clientSocket.cancelScannerSubscription(activeReqId);
                activeReqId = 0;
            }
        }

        public void RequestParameters()
        {
            m_clientSocket.reqScannerParameters();
        }

        private void HandleScannerParameters(ScannerParametersMessage scanParamsMessage)
        {
            m_logger.LogInformation("HandleScannerParameters: {0}", scanParamsMessage.XmlData);

            string filepath = Path.Combine(Directory.GetCurrentDirectory(), "Parameters", "ScannerParameters.xml");
            MoatCommon.WriteFile(filepath, scanParamsMessage.XmlData);
        }
        private void HandleScannerData(ScannerMessage scannMessage)
        {
            string scanCode = "";
            if (MoatCommon.CheckValidReqId(scannMessage.RequestId, out scanCode))
            {
                m_logger.LogInformation("HandleScannerData: Rank={0}, Summary={1}, Distance={2}, Benchmark={3}, Projection={4}, LegsStr={5}",
                    scannMessage.Rank, scannMessage.ContractDetails.Summary, scannMessage.Distance, scannMessage.Benchmark, scannMessage.Projection, scannMessage.LegsStr);
            }
        }
    }
}
