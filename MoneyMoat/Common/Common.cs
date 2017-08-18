using System;
using System.IO;
using System.Collections.Generic;
using IBApi;
using MoneyModels;
using System.Text;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;

namespace MoneyMoat
{
    public class Common
    {

        public static async Task<string> GetHttpContent(string url, Encoding encoding)
        {
            string result = string.Empty;
            HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            httpRequest.Method = "GET";
            httpRequest.Credentials = CredentialCache.DefaultCredentials;
            httpRequest.Timeout = 10000;

            try
            {
                var res = await httpRequest.GetResponseAsync();
                using (var sr = new StreamReader(res.GetResponseStream(), encoding))
                {
                    result = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetHttp {0} Error: {1}", url, e.Message);
            }
            return result;
        }

        public static void WriteFile(string filepath, string content)
        {
            string path = filepath.Substring(0, filepath.LastIndexOf(@"\"));
            if (Directory.Exists(path))
            {
                if (File.Exists(filepath))
                    File.Delete(filepath);
            }
            else
                Directory.CreateDirectory(path);
            using (var sw = File.CreateText(filepath))
            {
                sw.Write(content);
            }
            Console.WriteLine("Write ClientFile: " + filepath);
        }

        private static int activeReqId = 0;
        private static Dictionary<int, string> m_reqIds = new Dictionary<int, string>();

        public static int GetReqId(string pattern = "")
        {
            var id = System.Threading.Interlocked.Increment(ref activeReqId);
            m_reqIds[id] = pattern;
            return id;
        }
        public static bool CheckValidReqId(int reqId, out string pattern)
        {
            if (m_reqIds.ContainsKey(reqId))
            {
                pattern = m_reqIds[reqId];
                m_reqIds.Remove(reqId);
                return true;
            }
            else
            {
                pattern = string.Empty;
                return false;
            }
        }

        public static Contract GetStockContract(string symbol, ExchangeEnum exchange)
        {
            return GetStockContract(symbol, exchange.ToString());
        }
        public static Contract GetStockContract(string symbol, string exchange)
        {
            var currency = "USD";
            if (exchange.Equals(ExchangeEnum.SEHK.ToString()))
                currency = "HKD";
            else if (exchange.Equals(ExchangeEnum.SHSE.ToString()) || exchange.Equals(ExchangeEnum.SZSE.ToString()))
                currency = "CNY";

            var contract = new Contract
            {
                Symbol = symbol,
                SecType = "STK",
                Currency = currency,
                Exchange = "SMART",
                //Specify the Primary Exchange attribute to avoid contract ambiguity
                // (there is an ambiguity because there is also a MSFT contract with primary exchange = "AEB")
                PrimaryExch = exchange,
            };
            return contract;
        }
    }
}
