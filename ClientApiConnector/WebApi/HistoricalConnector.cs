using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using CommonLibs;
using CommonNetwork;
using StockModels;
using StockModels.ViewModels;
using System.Text;
using Newtonsoft.Json;

namespace ClientApiConnector.MoneyMoat
{
    public class HistoricalConnector
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
        /// UpdateHistoricalDataFromXueQiu
        /// </summary>
        public static async Task<HistoricalData> UpdateHistoricalDataFromXueQiu(string symbol)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
				{ "symbol", symbol.ToString()}
            });
			
            string url = ClientCommon.GetUrl("MoneyMoat") + "/Historical/UpdateHistoricalDataFromXueQiu";
            HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Clear();
            
            var response = await client.PostAsync(url, content);
            HistoricalData retData = default(HistoricalData);
            if (response.IsSuccessStatusCode)
            {
                string tmp = response.Content.ReadAsStringAsync().Result;
				retData = JsonConvert.DeserializeObject<HistoricalData>(tmp);
            }
            return retData;
        }

    }
}
