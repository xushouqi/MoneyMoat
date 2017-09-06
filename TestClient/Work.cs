using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StockModels;
using CommonLibs;
using CommonNetwork;
using CommonNetwork.MoneyMoat;

namespace TestClient
{
    public class Work
    {
        int m_idx = 0;
        string m_username = "xsq";
        string m_password = "123456";

        string m_ipAddr = "127.0.0.1:15046";
        //string m_ipAddr = "192.168.3.10:52010";
        //string m_ipAddr = "47.52.76.61:52010";

        ClientSocket m_socket = null;
        AnalyserClient m_analyser = null;

        public Work(int idx)
        {
            m_idx = idx;
            m_username += m_idx;
        }
        public void DebugLog(string words, params object[] args)
        {
            words = string.Format("Thread={0}, idx={1}: {2}", Thread.CurrentThread.ManagedThreadId, m_idx, words);
            Console.WriteLine(words, args);
        }

        public void Initial()
        {
            DebugLog("Initial");

            m_socket = new ClientSocket();
            m_analyser = new AnalyserClient(m_socket);

            m_socket.OnDisconnect += OnDisconnect;
            m_socket.OnError += OnError;
            m_socket.OnConnect += OnConnected;

            m_socket.ConnectServerAsync(m_ipAddr).Wait();
        }

        void OnConnected(bool success, string message)
        {
            DebugLog("OnConnected: {0}, {1}", success, message);

            if (success)
            {

            }
        }
        void OnConnectFail(string message)
        {
            DebugLog("OnConnectFail: {0}", message);
        }
        void OnDisconnect(string message)
        {
            DebugLog("OnDisconnect: {0}", message);
        }
        void OnError(string message)
        {
            DebugLog("OnError: {0}", message);
        }


    }
}
