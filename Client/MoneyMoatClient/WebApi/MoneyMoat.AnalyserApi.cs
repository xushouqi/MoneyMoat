using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using CommonLibs;
using CommonNetwork;
using StockModels.ViewModels;

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
        /// 獲取股票代碼
        /// </summary>
        public static async Task<int> UpdateStockSymbolsFromSinaAsync(bool saveToDb)
        {
            string qstr = "saveToDb=" + saveToDb.ToString();
            byte[] datas = RsaService.EncryptFromString(qstr, "");
            string url = ClientCommon.GetUrl("MoneyMoat") + "/Analyser/UpdateStockSymbolsFromSina";
            int retData = new int();
            retData.ErrorCode = ErrorCodeEnum.ResponseError;

            var response = await HttpWebResponseUtility.CreatePostHttpResponse(url, datas, null);
            if (response.IsSuccessStatusCode())
            {
				int length = 0;
                byte[] buffer = new byte[1000];
                using (var responseStream = response.GetResponseStream())
                {
                    length = await responseStream.ReadAsync(buffer, 0, buffer.Length);
                }
                if (length > 0)
                {
                    byte[] result = new byte[length];
                    Array.Copy(buffer, result, length);
                    var tmp = Encoding.UTF8.GetString(result);
					retData = JsonConvert.DeserializeObject<int>(tmp);
                }
            }
            return retData;
        }
		/// <summary>
        /// 停止所有后台任务
        /// </summary>
        public static async Task<ReturnData<int>> StopAllTasksAsync(int delay)
        {
            string qstr = "delay=" + delay.ToString();
            byte[] datas = RsaService.EncryptFromString(qstr, "");
            string url = ClientCommon.GetUrl("MoneyMoat") + "/Analyser/StopAllTasks";
            ReturnData<int> retData = new ReturnData<int>();
            retData.ErrorCode = ErrorCodeEnum.ResponseError;

            var response = await HttpWebResponseUtility.CreatePostHttpResponse(url, datas, null);
            if (response.IsSuccessStatusCode())
            {
				int length = 0;
                byte[] buffer = new byte[1000];
                using (var responseStream = response.GetResponseStream())
                {
                    length = await responseStream.ReadAsync(buffer, 0, buffer.Length);
                }
                if (length > 0)
                {
                    byte[] result = new byte[length];
                    Array.Copy(buffer, result, length);
                    var tmp = Encoding.UTF8.GetString(result);
					retData = JsonConvert.DeserializeObject<ReturnData<int>>(tmp);
                }
            }
            return retData;
        }
		/// <summary>
        /// 更新所有基本面数据
        /// </summary>
        public static async Task<ReturnData<int>> UpdateAllFundamentalsAsync(bool forceUpdate)
        {
            string qstr = "forceUpdate=" + forceUpdate.ToString();
            byte[] datas = RsaService.EncryptFromString(qstr, "");
            string url = ClientCommon.GetUrl("MoneyMoat") + "/Analyser/UpdateAllFundamentals";
            ReturnData<int> retData = new ReturnData<int>();
            retData.ErrorCode = ErrorCodeEnum.ResponseError;

            var response = await HttpWebResponseUtility.CreatePostHttpResponse(url, datas, null);
            if (response.IsSuccessStatusCode())
            {
				int length = 0;
                byte[] buffer = new byte[1000];
                using (var responseStream = response.GetResponseStream())
                {
                    length = await responseStream.ReadAsync(buffer, 0, buffer.Length);
                }
                if (length > 0)
                {
                    byte[] result = new byte[length];
                    Array.Copy(buffer, result, length);
                    var tmp = Encoding.UTF8.GetString(result);
					retData = JsonConvert.DeserializeObject<ReturnData<int>>(tmp);
                }
            }
            return retData;
        }
		/// <summary>
        /// 更新所有历史报价
        /// </summary>
        public static async Task<ReturnData<int>> UpdateAllHistoricalsAsync()
        {
            string qstr = "";
            byte[] datas = RsaService.EncryptFromString(qstr, "");
            string url = ClientCommon.GetUrl("MoneyMoat") + "/Analyser/UpdateAllHistoricals";
            ReturnData<int> retData = new ReturnData<int>();
            retData.ErrorCode = ErrorCodeEnum.ResponseError;

            var response = await HttpWebResponseUtility.CreatePostHttpResponse(url, datas, null);
            if (response.IsSuccessStatusCode())
            {
				int length = 0;
                byte[] buffer = new byte[1000];
                using (var responseStream = response.GetResponseStream())
                {
                    length = await responseStream.ReadAsync(buffer, 0, buffer.Length);
                }
                if (length > 0)
                {
                    byte[] result = new byte[length];
                    Array.Copy(buffer, result, length);
                    var tmp = Encoding.UTF8.GetString(result);
					retData = JsonConvert.DeserializeObject<ReturnData<int>>(tmp);
                }
            }
            return retData;
        }
		/// <summary>
        /// CalcFinSummary
        /// </summary>
        public static async Task<ReturnData<List<FinSummaryData>>> CalcFinSummaryAsync(string symbol)
        {
            string qstr = "symbol=" + symbol.ToString();
            byte[] datas = RsaService.EncryptFromString(qstr, "");
            string url = ClientCommon.GetUrl("MoneyMoat") + "/Analyser/CalcFinSummary";
            ReturnData<List<FinSummaryData>> retData = new ReturnData<List<FinSummaryData>>();
            retData.ErrorCode = ErrorCodeEnum.ResponseError;

            var response = await HttpWebResponseUtility.CreatePostHttpResponse(url, datas, null);
            if (response.IsSuccessStatusCode())
            {
				int length = 0;
                byte[] buffer = new byte[1000];
                using (var responseStream = response.GetResponseStream())
                {
                    length = await responseStream.ReadAsync(buffer, 0, buffer.Length);
                }
                if (length > 0)
                {
                    byte[] result = new byte[length];
                    Array.Copy(buffer, result, length);
                    var tmp = Encoding.UTF8.GetString(result);
					retData = JsonConvert.DeserializeObject<ReturnData<List<FinSummaryData>>>(tmp);
                }
            }
            return retData;
        }
		/// <summary>
        /// UpdateAndCalcFundamental
        /// </summary>
        public static async Task<ReturnData<int>> UpdateAndCalcFundamentalAsync(string symbol)
        {
            string qstr = "symbol=" + symbol.ToString();
            byte[] datas = RsaService.EncryptFromString(qstr, "");
            string url = ClientCommon.GetUrl("MoneyMoat") + "/Analyser/UpdateAndCalcFundamental";
            ReturnData<int> retData = new ReturnData<int>();
            retData.ErrorCode = ErrorCodeEnum.ResponseError;

            var response = await HttpWebResponseUtility.CreatePostHttpResponse(url, datas, null);
            if (response.IsSuccessStatusCode())
            {
				int length = 0;
                byte[] buffer = new byte[1000];
                using (var responseStream = response.GetResponseStream())
                {
                    length = await responseStream.ReadAsync(buffer, 0, buffer.Length);
                }
                if (length > 0)
                {
                    byte[] result = new byte[length];
                    Array.Copy(buffer, result, length);
                    var tmp = Encoding.UTF8.GetString(result);
					retData = JsonConvert.DeserializeObject<ReturnData<int>>(tmp);
                }
            }
            return retData;
        }

    }
}
