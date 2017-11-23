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
        public static async Task<StockData> FindAsyncAsync(string symbol)
        {
            string qstr = "symbol=" + symbol.ToString();
            byte[] datas = RsaService.EncryptFromString(qstr, "");
            string url = ClientCommon.GetUrl("MoneyMoat") + "/Symbol/FindAsync";
            StockData retData = new StockData();
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
					retData = JsonConvert.DeserializeObject<StockData>(tmp);
                }
            }
            return retData;
        }
		/// <summary>
        /// GetAllAsync
        /// </summary>
        public static async Task<List<StockData>> GetAllAsyncAsync()
        {
            string qstr = "";
            byte[] datas = RsaService.EncryptFromString(qstr, "");
            string url = ClientCommon.GetUrl("MoneyMoat") + "/Symbol/GetAllAsync";
            List<StockData> retData = new List<StockData>();
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
					retData = JsonConvert.DeserializeObject<List<StockData>>(tmp);
                }
            }
            return retData;
        }
		/// <summary>
        /// UpdateStock
        /// </summary>
        public static async Task<StockData> UpdateStockAsync(string name, string symbol, string category, bool saveToDb)
        {
            string qstr = "name=" + name.ToString()+"&" + "symbol=" + symbol.ToString()+"&" + "category=" + category.ToString()+"&" + "saveToDb=" + saveToDb.ToString();
            byte[] datas = RsaService.EncryptFromString(qstr, "");
            string url = ClientCommon.GetUrl("MoneyMoat") + "/Symbol/UpdateStock";
            StockData retData = new StockData();
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
					retData = JsonConvert.DeserializeObject<StockData>(tmp);
                }
            }
            return retData;
        }

    }
}
