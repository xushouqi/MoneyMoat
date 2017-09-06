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
    public class AnalyserApi
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
        /// 停止所有后台任务
        /// </summary>
        public static async Task<ReturnData<int>> StopAllTasks(int delay)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
				{ "delay", delay.ToString()}
            });
			
            string url = ClientCommon.GetUrl("MoneyMoat") + "/Analyser/StopAllTasks";
            HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Clear();
            
            var response = await client.PostAsync(url, content);
            ReturnData<int> retData = default(ReturnData<int>);
            if (response.IsSuccessStatusCode)
            {
                string tmp = response.Content.ReadAsStringAsync().Result;
				retData = JsonConvert.DeserializeObject<ReturnData<int>>(tmp);
            }
            return retData;
        }
		/// <summary>
        /// 更新所有基本面数据
        /// </summary>
        public static async Task<ReturnData<int>> UpdateAllFundamentals(bool forceUpdate)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
				{ "forceUpdate", forceUpdate.ToString()}
            });
			
            string url = ClientCommon.GetUrl("MoneyMoat") + "/Analyser/UpdateAllFundamentals";
            HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Clear();
            
            var response = await client.PostAsync(url, content);
            ReturnData<int> retData = default(ReturnData<int>);
            if (response.IsSuccessStatusCode)
            {
                string tmp = response.Content.ReadAsStringAsync().Result;
				retData = JsonConvert.DeserializeObject<ReturnData<int>>(tmp);
            }
            return retData;
        }
		/// <summary>
        /// 更新所有历史报价
        /// </summary>
        public static async Task<ReturnData<int>> UpdateAllHistoricals()
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
				
            });
			
            string url = ClientCommon.GetUrl("MoneyMoat") + "/Analyser/UpdateAllHistoricals";
            HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Clear();
            
            var response = await client.PostAsync(url, content);
            ReturnData<int> retData = default(ReturnData<int>);
            if (response.IsSuccessStatusCode)
            {
                string tmp = response.Content.ReadAsStringAsync().Result;
				retData = JsonConvert.DeserializeObject<ReturnData<int>>(tmp);
            }
            return retData;
        }
		/// <summary>
        /// CalcFinSummary
        /// </summary>
        public static async Task<ReturnData<List<FinSummaryData>>> CalcFinSummary(string symbol)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
				{ "symbol", symbol.ToString()}
            });
			
            string url = ClientCommon.GetUrl("MoneyMoat") + "/Analyser/CalcFinSummary";
            HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Clear();
            
            var response = await client.PostAsync(url, content);
            ReturnData<List<FinSummaryData>> retData = default(ReturnData<List<FinSummaryData>>);
            if (response.IsSuccessStatusCode)
            {
                string tmp = response.Content.ReadAsStringAsync().Result;
				retData = JsonConvert.DeserializeObject<ReturnData<List<FinSummaryData>>>(tmp);
            }
            return retData;
        }
		/// <summary>
        /// UpdateAndCalcFundamental
        /// </summary>
        public static async Task<ReturnData<int>> UpdateAndCalcFundamental(string symbol)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
				{ "symbol", symbol.ToString()}
            });
			
            string url = ClientCommon.GetUrl("MoneyMoat") + "/Analyser/UpdateAndCalcFundamental";
            HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Clear();
            
            var response = await client.PostAsync(url, content);
            ReturnData<int> retData = default(ReturnData<int>);
            if (response.IsSuccessStatusCode)
            {
                string tmp = response.Content.ReadAsStringAsync().Result;
				retData = JsonConvert.DeserializeObject<ReturnData<int>>(tmp);
            }
            return retData;
        }

    }
}
