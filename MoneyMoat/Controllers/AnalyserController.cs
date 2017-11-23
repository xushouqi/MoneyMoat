using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CommonLibs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
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
        private readonly ILogger _logger;

        public AnalyserController(ILoggerFactory logFactory, 
            MoneyMoat.Services.AnalyserService actionService)
        {
            _actionService = actionService;
            _logger = logFactory.CreateLogger("Error");
        }
		
        private int GetCurrentAccountId()
        {
            int accountid = -1;
            int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out accountid);
            return accountid;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateStockSymbolsFromSina(bool saveToDb, string sign)
        {
            try
            {
				string design = string.Empty;
				if (!string.IsNullOrEmpty(sign))
				{
					design = RsaService.DecryptToString(sign, "");
				}
				else if (Request.ContentLength != null)
				{
					byte[] datas = new byte[(int)Request.ContentLength];
					var ret = Request.Body.Read(datas, 0, (int)Request.ContentLength);
					design = RsaService.DecryptToString(datas, "");
				}
				if (!string.IsNullOrEmpty(design))
				{
					var tmp = Common.QueryStringToData(design);
					if (tmp != null && tmp.ContainsKey("saveToDb"))
					{
						var retData = await _actionService.UpdateStockSymbolsFromSina(bool.Parse(tmp["saveToDb"]));
						var data = new ReturnData<int>(retData);

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
					var retData = await _actionService.UpdateStockSymbolsFromSina(saveToDb);
					var data = new ReturnData<int>(retData);

					if (data != null)
					{
						return new OkObjectResult(data);
					}
					else
						return NoContent();
				}
            }
            catch(Exception e)
            {
                _logger.LogError(string.Format("Exception={0}\n ExceptionSource={1}\n StackTrace={2}",
                    e.Message, e.Source, e.StackTrace));
                return new BadRequestResult();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> StopAllTasks(int delay, string sign)
        {
            try
            {
				string design = string.Empty;
				if (!string.IsNullOrEmpty(sign))
				{
					design = RsaService.DecryptToString(sign, "");
				}
				else if (Request.ContentLength != null)
				{
					byte[] datas = new byte[(int)Request.ContentLength];
					var ret = Request.Body.Read(datas, 0, (int)Request.ContentLength);
					design = RsaService.DecryptToString(datas, "");
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
            catch(Exception e)
            {
                _logger.LogError(string.Format("Exception={0}\n ExceptionSource={1}\n StackTrace={2}",
                    e.Message, e.Source, e.StackTrace));
                return new BadRequestResult();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateAllFundamentals(bool forceUpdate, string sign)
        {
            try
            {
				string design = string.Empty;
				if (!string.IsNullOrEmpty(sign))
				{
					design = RsaService.DecryptToString(sign, "");
				}
				else if (Request.ContentLength != null)
				{
					byte[] datas = new byte[(int)Request.ContentLength];
					var ret = Request.Body.Read(datas, 0, (int)Request.ContentLength);
					design = RsaService.DecryptToString(datas, "");
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
            catch(Exception e)
            {
                _logger.LogError(string.Format("Exception={0}\n ExceptionSource={1}\n StackTrace={2}",
                    e.Message, e.Source, e.StackTrace));
                return new BadRequestResult();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateAllHistoricals(string sign)
        {
            try
            {
				string design = string.Empty;
				if (!string.IsNullOrEmpty(sign))
				{
					design = RsaService.DecryptToString(sign, "");
				}
				else if (Request.ContentLength != null)
				{
					byte[] datas = new byte[(int)Request.ContentLength];
					var ret = Request.Body.Read(datas, 0, (int)Request.ContentLength);
					design = RsaService.DecryptToString(datas, "");
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
            catch(Exception e)
            {
                _logger.LogError(string.Format("Exception={0}\n ExceptionSource={1}\n StackTrace={2}",
                    e.Message, e.Source, e.StackTrace));
                return new BadRequestResult();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CalcFinSummary(string symbol, string sign)
        {
            try
            {
				string design = string.Empty;
				if (!string.IsNullOrEmpty(sign))
				{
					design = RsaService.DecryptToString(sign, "");
				}
				else if (Request.ContentLength != null)
				{
					byte[] datas = new byte[(int)Request.ContentLength];
					var ret = Request.Body.Read(datas, 0, (int)Request.ContentLength);
					design = RsaService.DecryptToString(datas, "");
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
            catch(Exception e)
            {
                _logger.LogError(string.Format("Exception={0}\n ExceptionSource={1}\n StackTrace={2}",
                    e.Message, e.Source, e.StackTrace));
                return new BadRequestResult();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateAndCalcFundamental(string symbol, string sign)
        {
            try
            {
				string design = string.Empty;
				if (!string.IsNullOrEmpty(sign))
				{
					design = RsaService.DecryptToString(sign, "");
				}
				else if (Request.ContentLength != null)
				{
					byte[] datas = new byte[(int)Request.ContentLength];
					var ret = Request.Body.Read(datas, 0, (int)Request.ContentLength);
					design = RsaService.DecryptToString(datas, "");
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
            catch(Exception e)
            {
                _logger.LogError(string.Format("Exception={0}\n ExceptionSource={1}\n StackTrace={2}",
                    e.Message, e.Source, e.StackTrace));
                return new BadRequestResult();
            }
        }

		
    }
}
