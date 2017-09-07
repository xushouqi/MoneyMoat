using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using CommonLibs;
using CommonNetwork;
using StockModels.ViewModels;
using Newtonsoft.Json;

namespace ClientApi.MoneyMoat
{
    public class SymbolApi
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
        /// FindAsync
        /// </summary>
        public static async Task<StockData> FindAsync(string symbol)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
				{ "symbol", symbol.ToString()}
            });
			
            string url = ClientCommon.GetUrl("MoneyMoat") + "/Symbol/FindAsync";
            HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Clear();
            
            var response = await client.PostAsync(url, content);
            StockData retData = default(StockData);
            if (response.IsSuccessStatusCode)
            {
                string tmp = response.Content.ReadAsStringAsync().Result;
				retData = JsonConvert.DeserializeObject<StockData>(tmp);
            }
            return retData;
        }
		/// <summary>
        /// GetAllAsync
        /// </summary>
        public static async Task<List<StockData>> GetAllAsync()
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
				
            });
			
            string url = ClientCommon.GetUrl("MoneyMoat") + "/Symbol/GetAllAsync";
            HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Clear();
            
            var response = await client.PostAsync(url, content);
            List<StockData> retData = default(List<StockData>);
            if (response.IsSuccessStatusCode)
            {
                string tmp = response.Content.ReadAsStringAsync().Result;
				retData = JsonConvert.DeserializeObject<List<StockData>>(tmp);
            }
            return retData;
        }
		/// <summary>
        /// UpdateStock
        /// </summary>
        public static async Task<StockData> UpdateStock(string name, string symbol, string category, bool saveToDb)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
				{ "name", name.ToString()}, { "symbol", symbol.ToString()}, { "category", category.ToString()}, { "saveToDb", saveToDb.ToString()}
            });
			
            string url = ClientCommon.GetUrl("MoneyMoat") + "/Symbol/UpdateStock";
            HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Clear();
            
            var response = await client.PostAsync(url, content);
            StockData retData = default(StockData);
            if (response.IsSuccessStatusCode)
            {
                string tmp = response.Content.ReadAsStringAsync().Result;
				retData = JsonConvert.DeserializeObject<StockData>(tmp);
            }
            return retData;
        }

    }
}
