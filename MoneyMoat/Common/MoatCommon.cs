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
    public class MoatCommon
    {
        public static long GetTimestamp(DateTime theTime)
        {
            System.DateTime startTime = new DateTime(1970, 1, 1).Add(TimeZoneInfo.Local.BaseUtcOffset); // 当地时区
            long timeStamp = (long)(theTime - startTime).TotalMilliseconds; // 相差毫秒数
            return timeStamp;
        }
        public static DateTime GetDateTime(long timeStamp)
        {
            System.DateTime time = new DateTime(1970, 1, 1).AddMilliseconds(timeStamp); // 当地时区
            return time;
        }

        public static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public static async Task<string> GetXueQiuContent(string url)
        {
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

            string result = string.Empty;
            HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            httpRequest.Method = "GET";
            httpRequest.Timeout = 10000;
            httpRequest.CookieContainer = new CookieContainer();
            httpRequest.CookieContainer.Add(new Cookie("xq_a_token", "f8846a20e3a8074cd781524d47619ba6879990e2", "/", "xueqiu.com"));
            httpRequest.CookieContainer.Add(new Cookie("xq_r_token", "562674ae525074f694a9961c5a49a644275e3b53", "/", "xueqiu.com"));

            try
            {                
                var res = await httpRequest.GetResponseAsync();
                using (var sr = new StreamReader(res.GetResponseStream(), System.Text.Encoding.UTF8))
                {
                    result = sr.ReadToEnd();
                }
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)wex.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            string error = reader.ReadToEnd();
                            Console.WriteLine("GetHttp {0} Error: {1}", url, error);
                        }
                    }
                }
            }
            return result;
        }

        public static async Task<string> GetHttpContent(string url, Encoding encoding)
        {
            string result = string.Empty;
            HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            httpRequest.Method = "GET";
            //httpRequest.Credentials = CredentialCache.DefaultCredentials;
            httpRequest.Timeout = 10000;

            try
            {
                var res = await httpRequest.GetResponseAsync();
                using (var sr = new StreamReader(res.GetResponseStream(), encoding))
                {
                    result = sr.ReadToEnd();
                    var test = result;
                }
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)wex.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            string error = reader.ReadToEnd();
                            Console.WriteLine("GetHttp {0} Error: {1}", url, error);
                        }
                    }
                }
            }
            return result;
        }

        public static string GetFundamentalFilePath(string symbol, FundamentalsReportEnum ftype)
        {
            string filepath = Path.Combine(Directory.GetCurrentDirectory(), "Fundamentals", symbol, ftype.ToString() + ".xml");
            return filepath;
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
            Console.WriteLine("Write: " + filepath);
        }
        public static async Task<string> ReadFile(string filepath)
        {
            string content = string.Empty;
            string path = filepath.Substring(0, filepath.LastIndexOf(@"\"));
            if (File.Exists(filepath))
            {
                content = await File.ReadAllTextAsync(filepath, System.Text.Encoding.UTF8);
            }
            Console.WriteLine("Read: " + filepath);
            return content;
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
