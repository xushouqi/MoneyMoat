using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Xml.Serialization;
using IBApi;
using MoneyModels;
using MoneyMoat.Types;
using MoneyMoat.Messages;
using YAXLib;

namespace MoneyMoat.Services
{
    class IBManager
    {
        private readonly ILogger m_logger;
        private readonly AppSettings m_config;
        protected readonly IBClient ibClient;
        protected EReader m_ereader;
        private bool IsConnected = false;

        protected readonly SymbolSamplesService m_symbolSampleService;
        protected readonly FundamentalService m_fundamentalService;
        protected readonly AccountService m_accountService;
        protected readonly HistoricalService m_historicalService;


        private const int CONTRACT_ID_BASE = 60000000;
        private const int CONTRACT_DETAILS_ID = CONTRACT_ID_BASE + 1;
        private const int FUNDAMENTALS_ID = CONTRACT_ID_BASE + 2;

        public IBManager(IBClient ibclient,
                        SymbolSamplesService symbolService,
                        FundamentalService fundamentalService,
                        AccountService accountService,
                        HistoricalService historicalService,
                        ILogger<IBManager> logger,
                        IOptions<AppSettings> commonoptions)
        {
            m_logger = logger;
            m_config = commonoptions.Value;
            ibClient = ibclient;
            m_symbolSampleService = symbolService;
            m_fundamentalService = fundamentalService;
            m_accountService = accountService;
            m_historicalService = historicalService;

            ibClient.Error += ibClient_Error;
            ibClient.ConnectionClosed += ibClient_ConnectionClosed;
            ibClient.CurrentTime += ibClient_CurrentTime;
            ibClient.NextValidId += ibClient_NextValidId;
        }

        void ibClient_Error(int id, int errorCode, string str, Exception ex)
        {
            if (ex != null)
                m_logger.LogError("ibClient_Error: id={0}, code={1}, {2}\n {3}\n {4}", id, errorCode, str, ex.Message, ex.StackTrace);
            else
                m_logger.LogError("ibClient_Error: id={0}, code={1}, {2}", id, errorCode, str);
        }
        void ibClient_ConnectionClosed()
        {
            IsConnected = false;
            m_logger.LogInformation("ibClient_ConnectionClosed");
        }
        void ibClient_CurrentTime(long time)
        {
            m_logger.LogInformation("ibClient_CurrentTime: ", time);
        }
        void ibClient_NextValidId(ConnectionStatusMessage statusMessage)
        {
            m_logger.LogInformation("ibClient_NextValidId: Connected={0}, ClientId={1}", statusMessage.IsConnected, ibClient.ClientId);
        }

        public void Connect()
        {
            if (!IsConnected)
            {
                int port = m_config.GatewayPort;
                string host = m_config.GatewayHost;

                if (host == null || host.Equals(""))
                    host = "127.0.0.1";
                try
                {
                    ibClient.ClientId = 1;
                    ibClient.ClientSocket.eConnect(host, port, ibClient.ClientId);
                    IsConnected = true;

                    m_ereader = new EReader(ibClient.ClientSocket, ibClient.Signal);
                    m_ereader.Start();

                    new Thread(() => { while (ibClient.ClientSocket.IsConnected()) { ibClient.Signal.waitForSignal(); m_ereader.processMsgs(); } }) { IsBackground = true }.Start();
                }
                catch (Exception e)
                {
                    m_logger.LogError("Connect Error: {0}\n{1}", e.Message, e.StackTrace);
                }
            }
            else
            {
                IsConnected = false;
                ibClient.ClientSocket.eDisconnect();
            }
        }

        public void Test()
        {
            //RequestAccountSummery();
            //RequestSymbols("IGG");
            //m_historicalService.RequestEarliestDataPoint("MOMO", ExchangeEnum.ISLAND);
            m_fundamentalService.RequestFundamentals("MOMO", ExchangeEnum.ISLAND, FundamentalsReportEnum.ReportsFinStatements);
            //RequestFundamentals("MOMO", FundamentalsReportEnum.ReportsFinSummary);
            //RequestFundamentals("MOMO", FundamentalsReportEnum.ReportSnapshot);
            //RequestFundamentals("MOMO", FundamentalsReportEnum.RESC);
        }


    }
}
