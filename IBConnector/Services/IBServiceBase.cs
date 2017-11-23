using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using IBApi;
using Polly;
using CommonLibs;

namespace IBConnector.Services
{
    public class IBServiceBase<TResult>
    {
        protected readonly IBManager ibManager;
        protected readonly IBClient ibClient;
        protected readonly ILogger m_logger;
        protected readonly CommonManager m_commonManager;

        protected ConcurrentDictionary<int, TResult> m_results = new ConcurrentDictionary<int, TResult>();

        protected EClientSocket m_clientSocket
        {
            get
            {
                if (!ibManager.IsConnected)
                    ibManager.Connect();
                return ibClient.ClientSocket;
            }
        }
        
        public IBServiceBase(IBManager ibmanager, ILogger logger, CommonManager commonManager)
        {
            ibManager = ibmanager;
            m_logger = logger;
            m_commonManager = commonManager;

            ibClient = ibManager.ibClient;
            ibClient.Error += ibClient_Error;
        }

        void ibClient_Error(int reqId, int errorCode, string str, Exception ex)
        {
            AutoResetEvent handle = null;
            if (m_commonManager.RequestHnadles.TryGetValue(reqId, out handle))
            {
                handle.Set();
            }
        }
        
        protected async Task<TResult> SendRequestAsync(int reqId, Action action = null)
        {
            TResult result = default(TResult);
            var handle = new AutoResetEvent(false);
            if (m_commonManager.RequestHnadles.TryAdd(reqId, handle))
            {
                if (action != null)
                    action();

                handle.WaitOne(60000);
                m_commonManager.RequestHnadles.TryRemove(reqId, out handle);

                if (m_results.TryGetValue(reqId, out result))
                    m_results.TryRemove(reqId, out result);
            }
            return await Task.FromResult(result);
        }

        protected bool HandleResponse(int reqId)
        {
            bool ret = false;
            string symbol = string.Empty;
            if (m_commonManager.CheckValidReqId(reqId, out symbol))
            {
                AutoResetEvent handle = null;
                ret = m_commonManager.RequestHnadles.TryGetValue(reqId, out handle);
                if (ret)
                    handle.Set();
            }
            return ret;
        }
    }

}
