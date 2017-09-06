using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using CommonLibs;
using CommonNetwork;
using StockModels.ViewModels;
using System.Text;
using Newtonsoft.Json;

namespace ClientApi.MoneyMoat
{
    public class FundamentalApi
    {
		/*
        private static HttpClient _client = new HttpClient();
        private static HttpClient client
        {
            get
            {
                if (_client == null)
                    _client = new HttpClient();
                return _client;
            }
        }
		*/

				/// <summary>
        /// UpdateFundamentalsFromXueQiu
        /// </summary>
        public static async Task<string> UpdateFundamentalsFromXueQiu(string symbol)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
				{ "symbol", symbol.ToString()}
            });
			
            string url = ClientCommon.GetUrl("MoneyMoat") + "/Fundamental/UpdateFundamentalsFromXueQiu";
            HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Clear();
            
            var response = await client.PostAsync(url, content);
            string retData = default(string);
            if (response.IsSuccessStatusCode)
            {
                string tmp = response.Content.ReadAsStringAsync().Result;
				retData = JsonConvert.DeserializeObject<string>(tmp);
            }
            return retData;
        }
		/// <summary>
        /// UpdateAllFromIB
        /// </summary>
        public static async Task<int> UpdateAllFromIB(string symbol, bool forceUpdate)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
				{ "symbol", symbol.ToString()}, { "forceUpdate", forceUpdate.ToString()}
            });
			
            string url = ClientCommon.GetUrl("MoneyMoat") + "/Fundamental/UpdateAllFromIB";
            HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Clear();
            
            var response = await client.PostAsync(url, content);
            int retData = default(int);
            if (response.IsSuccessStatusCode)
            {
                string tmp = response.Content.ReadAsStringAsync().Result;
				retData = JsonConvert.DeserializeObject<int>(tmp);
            }
            return retData;
        }
		/// <summary>
        /// ReadFromXmlAsync
        /// </summary>
        public static async Task<string> ReadFromXmlAsync(string symbol, CommonLibs.FundamentalsReportEnum ftype)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
				{ "symbol", symbol.ToString()}, { "ftype", ((int)ftype).ToString()}
            });
			
            string url = ClientCommon.GetUrl("MoneyMoat") + "/Fundamental/ReadFromXmlAsync";
            HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Clear();
            
            var response = await client.PostAsync(url, content);
            string retData = default(string);
            if (response.IsSuccessStatusCode)
            {
                string tmp = response.Content.ReadAsStringAsync().Result;
				retData = JsonConvert.DeserializeObject<string>(tmp);
            }
            return retData;
        }
		/// <summary>
        /// RequestFromIBAsync
        /// </summary>
        public static async Task<string> RequestFromIBAsync(string symbol, string exchange, CommonLibs.FundamentalsReportEnum ftype, bool forceUpdate)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
				{ "symbol", symbol.ToString()}, { "exchange", exchange.ToString()}, { "ftype", ((int)ftype).ToString()}, { "forceUpdate", forceUpdate.ToString()}
            });
			
            string url = ClientCommon.GetUrl("MoneyMoat") + "/Fundamental/RequestFromIBAsync";
            HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Clear();
            
            var response = await client.PostAsync(url, content);
            string retData = default(string);
            if (response.IsSuccessStatusCode)
            {
                string tmp = response.Content.ReadAsStringAsync().Result;
				retData = JsonConvert.DeserializeObject<string>(tmp);
            }
            return retData;
        }
		/// <summary>
        /// ReadParseFundamentalToDbBackend
        /// </summary>
        public static async Task<string> ReadParseFundamentalToDbBackend(string symbol, CommonLibs.FundamentalsReportEnum ftype)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
				{ "symbol", symbol.ToString()}, { "ftype", ((int)ftype).ToString()}
            });
			
            string url = ClientCommon.GetUrl("MoneyMoat") + "/Fundamental/ReadParseFundamentalToDbBackend";
            HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Clear();
            
            var response = await client.PostAsync(url, content);
            string retData = default(string);
            if (response.IsSuccessStatusCode)
            {
                string tmp = response.Content.ReadAsStringAsync().Result;
				retData = JsonConvert.DeserializeObject<string>(tmp);
            }
            return retData;
        }

    }
}
