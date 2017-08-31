using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
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
    public class IBServiceBase<TResult>
    {
        protected readonly IBManager ibManager;
        protected readonly IBClient ibClient;

        public IBServiceBase(IBManager ibmanager)
        {
            ibManager = ibmanager;
            ibManager.Connect();

            ibClient = ibManager.ibClient;
            ibClient.Error += ibClient_Error;
        }

        protected Dictionary<int, AutoResetEvent> m_handles = new Dictionary<int, AutoResetEvent>();
        protected Dictionary<int, TResult> m_results = new Dictionary<int, TResult>();

        void ibClient_Error(int reqId, int errorCode, string str, Exception ex)
        {
            if (m_handles.ContainsKey(reqId))
            {
                m_handles[reqId].Set();
            }
        }

        protected async Task<TResult> SendRequestAsync(int reqId)
        {
            m_handles[reqId] = new AutoResetEvent(false);
            m_handles[reqId].WaitOne(60000);
            m_handles.Remove(reqId);

            TResult result = default(TResult);
            if (m_results.TryGetValue(reqId, out result))
                m_results.Remove(reqId);
            return await Task.FromResult(result);
        }
        protected bool HandleResponse(int reqId)
        {
            bool ret = false;
            string symbol = string.Empty;
            if (MoatCommon.CheckValidReqId(reqId, out symbol))
            {
                if (m_handles.ContainsKey(reqId))
                {
                    ret = true;
                    m_handles[reqId].Set();
                }
            }
            return ret;
        }
    }

}
