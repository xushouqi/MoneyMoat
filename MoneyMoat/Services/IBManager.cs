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
    public class IBManager
    {
        private readonly ILogger m_logger;
        private readonly AppSettings m_config;
        protected EReader m_ereader;
        public bool IsConnected = false;
        public IBClient ibClient { get; set; }

        private const int CONTRACT_ID_BASE = 60000000;
        private const int CONTRACT_DETAILS_ID = CONTRACT_ID_BASE + 1;
        private const int FUNDAMENTALS_ID = CONTRACT_ID_BASE + 2;

         IBManager(IBClient ibclient,
                        ILogger<IBManager> logger,
                        IOptions<AppSettings> commonoptions)
        {
            m_logger = logger;
            m_config = commonoptions.Value;
            ibClient = ibclient;
            
            ibClient.Error += ibClient_Error;            
            ibClient.ConnectionClosed += ibClient_ConnectionClosed;
            ibClient.CurrentTime += ibClient_CurrentTime;
            ibClient.NextValidId += ibClient_NextValidId;
        }

        void ibClient_Error(int reqId, int errorCode, string str, Exception ex)
        {
            if (ex != null)
                m_logger.LogError("ibClient_Error: id={0}, code={1}, {2}\n {3}\n {4}", reqId, errorCode, str, ex.Message, ex.StackTrace);
            else
                m_logger.LogError("ibClient_Error: id={0}, code={1}, {2}", reqId, errorCode, str);

            if (m_needReconnect)
            {
                if (!ibClient.ClientSocket.IsConnected())
                {
                    Task.Delay(1000).Wait();
                    Connect();
                }
                else
                    m_onError = true;
            }
        }

        bool m_needReconnect = false;
        bool m_onError = false;
        void ibClient_ConnectionClosed()
        {
            IsConnected = false;
            m_logger.LogInformation("ibClient_ConnectionClosed");

            if (m_onError)
            {
                m_onError = false;
                Connect();
            }
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
                    m_needReconnect = true;

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
                m_logger.LogWarning("Already connected.");
        }

        public void Disconnect()
        {
            if (IsConnected)
            {
                IsConnected = false;
                ibClient.ClientSocket.eDisconnect();
            }
        }

    }
}
