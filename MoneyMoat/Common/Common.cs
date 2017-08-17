using System;
using System.IO;
using System.Collections.Generic;
using IBApi;
using MoneyMoat.Types;

namespace MoneyMoat
{
    public class Common
    {
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
            var contract = new Contract
            {
                Symbol = symbol,
                SecType = "STK",
                Currency = "USD",
                Exchange = "SMART",
                //Specify the Primary Exchange attribute to avoid contract ambiguity
                // (there is an ambiguity because there is also a MSFT contract with primary exchange = "AEB")
                PrimaryExch = exchange.ToString(),
            };
            return contract;
        }
    }
}
