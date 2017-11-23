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
        public static async Task<string> UpdateFundamentalsFromXueQiuAsync(string symbol)
        {
            string qstr = "symbol=" + symbol.ToString();
            byte[] datas = RsaService.EncryptFromString(qstr, "");
            string url = ClientCommon.GetUrl("MoneyMoat") + "/Fundamental/UpdateFundamentalsFromXueQiu";
            string retData = new string();
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
					retData = JsonConvert.DeserializeObject<string>(tmp);
                }
            }
            return retData;
        }
		/// <summary>
        /// UpdateAllFromIB
        /// </summary>
        public static async Task<int> UpdateAllFromIBAsync(string symbol, bool forceUpdate)
        {
            string qstr = "symbol=" + symbol.ToString()+"&" + "forceUpdate=" + forceUpdate.ToString();
            byte[] datas = RsaService.EncryptFromString(qstr, "");
            string url = ClientCommon.GetUrl("MoneyMoat") + "/Fundamental/UpdateAllFromIB";
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
        /// ReadFromXmlAsync
        /// </summary>
        public static async Task<string> ReadFromXmlAsyncAsync(string symbol, StockModels.FundamentalsReportEnum ftype)
        {
            string qstr = "symbol=" + symbol.ToString()+"&" + "ftype=" + ((int)ftype).ToString();
            byte[] datas = RsaService.EncryptFromString(qstr, "");
            string url = ClientCommon.GetUrl("MoneyMoat") + "/Fundamental/ReadFromXmlAsync";
            string retData = new string();
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
					retData = JsonConvert.DeserializeObject<string>(tmp);
                }
            }
            return retData;
        }
		/// <summary>
        /// RequestFromIBAsync
        /// </summary>
        public static async Task<string> RequestFromIBAsyncAsync(string symbol, string exchange, StockModels.FundamentalsReportEnum ftype, bool forceUpdate)
        {
            string qstr = "symbol=" + symbol.ToString()+"&" + "exchange=" + exchange.ToString()+"&" + "ftype=" + ((int)ftype).ToString()+"&" + "forceUpdate=" + forceUpdate.ToString();
            byte[] datas = RsaService.EncryptFromString(qstr, "");
            string url = ClientCommon.GetUrl("MoneyMoat") + "/Fundamental/RequestFromIBAsync";
            string retData = new string();
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
					retData = JsonConvert.DeserializeObject<string>(tmp);
                }
            }
            return retData;
        }
		/// <summary>
        /// ReadParseFundamentalToDbBackend
        /// </summary>
        public static async Task<string> ReadParseFundamentalToDbBackendAsync(string symbol, StockModels.FundamentalsReportEnum ftype)
        {
            string qstr = "symbol=" + symbol.ToString()+"&" + "ftype=" + ((int)ftype).ToString();
            byte[] datas = RsaService.EncryptFromString(qstr, "");
            string url = ClientCommon.GetUrl("MoneyMoat") + "/Fundamental/ReadParseFundamentalToDbBackend";
            string retData = new string();
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
					retData = JsonConvert.DeserializeObject<string>(tmp);
                }
            }
            return retData;
        }

    }
}
