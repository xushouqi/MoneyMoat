using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CommonLibs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using AutoMapper;
using Microsoft.Extensions.Logging;
using StockModels;
using StockModels.ViewModels;

namespace MoneyMoat.Controllers
{
    [EnableCors("AllowSameDomain")]
    [ServiceFilter(typeof(ApiExceptionFilter))]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class AnalyserController : Controller
    {
        private readonly MoneyMoat.Services.AnalyserService _actionService;

        public AnalyserController(MoneyMoat.Services.AnalyserService actionService)
        {
            _actionService = actionService;
        }
		
        private int GetCurrentAccountId()
        {
            int accountid = int.Parse(User.Claims.First().Value);
            return accountid;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> StopAllTasks(int delay, string sign)
        {
			string design = string.Empty;
            if (!string.IsNullOrEmpty(sign))
            {
                design = RsaService.DecryptToString(sign);
            }
            else if (Request.ContentLength != null)
            {
                byte[] datas = new byte[(int)Request.ContentLength];
                var ret = Request.Body.Read(datas, 0, (int)Request.ContentLength);
                design = RsaService.DecryptToString(datas);
            }
            if (!string.IsNullOrEmpty(design))
            {
				var tmp = Common.QueryStringToData(design);
				if (tmp != null && tmp.ContainsKey("delay"))
				{
					var retData = await _actionService.StopAllTasks(int.Parse(tmp["delay"]));
					var data = retData;

					if (data != null)
					{
						return new OkObjectResult(data);
					}
					else
						return NoContent();
				}
				else
					return new UnauthorizedResult();
            }
            else
            {
				var retData = await _actionService.StopAllTasks(delay);
				var data = retData;

				if (data != null)
				{
					return new OkObjectResult(data);
				}
				else
					return NoContent();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateAllFundamentals(bool forceUpdate, string sign)
        {
			string design = string.Empty;
            if (!string.IsNullOrEmpty(sign))
            {
                design = RsaService.DecryptToString(sign);
            }
            else if (Request.ContentLength != null)
            {
                byte[] datas = new byte[(int)Request.ContentLength];
                var ret = Request.Body.Read(datas, 0, (int)Request.ContentLength);
                design = RsaService.DecryptToString(datas);
            }
            if (!string.IsNullOrEmpty(design))
            {
				var tmp = Common.QueryStringToData(design);
				if (tmp != null && tmp.ContainsKey("forceUpdate"))
				{
					var retData = await _actionService.UpdateAllFundamentals(bool.Parse(tmp["forceUpdate"]));
					var data = retData;

					if (data != null)
					{
						return new OkObjectResult(data);
					}
					else
						return NoContent();
				}
				else
					return new UnauthorizedResult();
            }
            else
            {
				var retData = await _actionService.UpdateAllFundamentals(forceUpdate);
				var data = retData;

				if (data != null)
				{
					return new OkObjectResult(data);
				}
				else
					return NoContent();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateAllHistoricals(string sign)
        {
			string design = string.Empty;
            if (!string.IsNullOrEmpty(sign))
            {
                design = RsaService.DecryptToString(sign);
            }
            else if (Request.ContentLength != null)
            {
                byte[] datas = new byte[(int)Request.ContentLength];
                var ret = Request.Body.Read(datas, 0, (int)Request.ContentLength);
                design = RsaService.DecryptToString(datas);
            }
            if (!string.IsNullOrEmpty(design))
            {
				var tmp = Common.QueryStringToData(design);
				if (tmp != null)
				{
					var retData = await _actionService.UpdateAllHistoricals();
					var data = retData;

					if (data != null)
					{
						return new OkObjectResult(data);
					}
					else
						return NoContent();
				}
				else
					return new UnauthorizedResult();
            }
            else
            {
				var retData = await _actionService.UpdateAllHistoricals();
				var data = retData;

				if (data != null)
				{
					return new OkObjectResult(data);
				}
				else
					return NoContent();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CalcFinSummary(string symbol, string sign)
        {
			string design = string.Empty;
            if (!string.IsNullOrEmpty(sign))
            {
                design = RsaService.DecryptToString(sign);
            }
            else if (Request.ContentLength != null)
            {
                byte[] datas = new byte[(int)Request.ContentLength];
                var ret = Request.Body.Read(datas, 0, (int)Request.ContentLength);
                design = RsaService.DecryptToString(datas);
            }
            if (!string.IsNullOrEmpty(design))
            {
				var tmp = Common.QueryStringToData(design);
				if (tmp != null && tmp.ContainsKey("symbol"))
				{
					var retData = await _actionService.CalcFinSummary(tmp["symbol"]);
					var dataList = new List<FinSummaryData>();
                    for (int i = 0; i < retData.Data.Count; i++)
                        dataList.Add(Mapper.Map<FinSummaryData>(retData.Data[i]));
                    var data = new ReturnData<List<FinSummaryData>>{
                        ErrorCode = retData.ErrorCode,
                        Data = dataList,
                    };

					if (data != null)
					{
						return new OkObjectResult(data);
					}
					else
						return NoContent();
				}
				else
					return new UnauthorizedResult();
            }
            else
            {
				var retData = await _actionService.CalcFinSummary(symbol);
				var dataList = new List<FinSummaryData>();
                    for (int i = 0; i < retData.Data.Count; i++)
                        dataList.Add(Mapper.Map<FinSummaryData>(retData.Data[i]));
                    var data = new ReturnData<List<FinSummaryData>>{
                        ErrorCode = retData.ErrorCode,
                        Data = dataList,
                    };

				if (data != null)
				{
					return new OkObjectResult(data);
				}
				else
					return NoContent();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateAndCalcFundamental(string symbol, string sign)
        {
			string design = string.Empty;
            if (!string.IsNullOrEmpty(sign))
            {
                design = RsaService.DecryptToString(sign);
            }
            else if (Request.ContentLength != null)
            {
                byte[] datas = new byte[(int)Request.ContentLength];
                var ret = Request.Body.Read(datas, 0, (int)Request.ContentLength);
                design = RsaService.DecryptToString(datas);
            }
            if (!string.IsNullOrEmpty(design))
            {
				var tmp = Common.QueryStringToData(design);
				if (tmp != null && tmp.ContainsKey("symbol"))
				{
					var retData = await _actionService.UpdateAndCalcFundamental(tmp["symbol"]);
					var data = retData;

					if (data != null)
					{
						return new OkObjectResult(data);
					}
					else
						return NoContent();
				}
				else
					return new UnauthorizedResult();
            }
            else
            {
				var retData = await _actionService.UpdateAndCalcFundamental(symbol);
				var data = retData;

				if (data != null)
				{
					return new OkObjectResult(data);
				}
				else
					return NoContent();
            }
        }

		
    }
}
