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
using StockModels;
using MoneyMoat.Types;
using MoneyMoat.Messages;
using Polly;

namespace MoneyMoat.Services
{
    public class IBManager
    {
        private readonly ILogger m_logger;
        private readonly AppSettings m_config;
        protected EReader m_ereader;
        public IBClient ibClient { get; set; }

        private const int CONTRACT_ID_BASE = 60000000;
        private const int CONTRACT_DETAILS_ID = CONTRACT_ID_BASE + 1;
        private const int FUNDAMENTALS_ID = CONTRACT_ID_BASE + 2;

        public IBManager(IBClient ibclient,
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

        public bool IsConnected
        {
            get
            {
                return ibClient.ClientSocket.IsConnected();
            }
        }

        void ibClient_Error(int reqId, int errorCode, string str, Exception ex)
        {
            m_isConnecting = false;

            if (ex != null)
                m_logger.LogError("ibClient_Error: id={0}, code={1}, {2}\n {3}\n {4}", reqId, errorCode, str, ex.Message, ex.StackTrace);
            else
                m_logger.LogError("ibClient_Error: id={0}, code={1}, {2}", reqId, errorCode, str);

            //之前连接成功着
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
            m_isConnecting = false;
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

        object m_lock_me = new object();
        bool m_isConnecting = false;

        public void Connect()
        {
            lock (m_lock_me)
            {
                if (!IsConnected && !m_isConnecting)
                {
                    m_isConnecting = true;

                    Action<DelegateResult<bool>, TimeSpan> onRetryForever = (exception, timespan) =>
                    {
                        m_logger.LogError("ibManager.Connect onRetry: wait: {0}s", timespan.TotalSeconds);
                    };
                    Action<DelegateResult<bool>, int> onRetry = (exception, timespan) =>
                    {
                        m_logger.LogError("ibManager.Connect onRetry: wait: {0}s", timespan);
                    };
                    Action<DelegateResult<bool>, TimeSpan> onBreak = (exception, timespan) =>
                    {
                        m_logger.LogError("ibManager.Connect onBreak: wait: {0}s", timespan.TotalSeconds);
                    };
                    Action onReset = () => { m_logger.LogError("ibManager.Connect onReset"); };

                    try
                    {
                        //反复重试
                        var retry = Policy
                                        .Handle<Exception>()
                                        .OrResult<bool>(t => t == false)
                                         //.HandleResult<bool>(t => t == false)
                                         //.WaitAndRetryForever(retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                         //                    onRetryForever)
                                         .WaitAndRetry(5, retryAttempt =>
                                                            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                                        ;
                        //重试次数过多后先断开，稍后再试
                        var circuitBreak = Policy
                                        .Handle<Exception>()
                                        .OrResult<bool>(t => t == false)
                                        .CircuitBreaker(10, TimeSpan.FromMinutes(15), onBreak, onReset)
                                        ;
                        Policy.Wrap(retry, circuitBreak).Execute(() => DoConnect());
                    }
                    catch (Exception e) { }
                }
            }
        }

        private bool DoConnect()
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

                    //连接成功，如果之后报错需要重连
                    m_needReconnect = IsConnected;

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
            return IsConnected;
        }

        public void Disconnect()
        {
            if (IsConnected)
            {
                //手动断连
                m_needReconnect = false;
                ibClient.ClientSocket.eDisconnect();
            }
        }

    }
}
