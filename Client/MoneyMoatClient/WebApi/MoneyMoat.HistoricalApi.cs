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
    public class HistoricalApi
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
        public static async Task<HistoricalData> UpdateHistoricalDataFromXueQiuAsync(string symbol)
        {
            string qstr = "symbol=" + symbol.ToString();
            byte[] datas = RsaService.EncryptFromString(qstr, "");
            string url = ClientCommon.GetUrl("MoneyMoat") + "/Historical/UpdateHistoricalDataFromXueQiu";
            HistoricalData retData = new HistoricalData();
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
					retData = JsonConvert.DeserializeObject<HistoricalData>(tmp);
                }
            }
            return retData;
        }

    }
}
