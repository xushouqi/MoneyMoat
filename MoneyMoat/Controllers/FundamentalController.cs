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
    public class FundamentalController : Controller
    {
        private readonly MoneyMoat.Services.FundamentalService _actionService;

        public FundamentalController(MoneyMoat.Services.FundamentalService actionService)
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
        public async Task<IActionResult> UpdateFundamentalsFromXueQiu(string symbol, string sign)
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
                design = RsaService.DecryptToString(datas, 0);
            }
            if (!string.IsNullOrEmpty(design))
            {
				var tmp = Common.QueryStringToData(design);
				if (tmp != null && tmp.ContainsKey("symbol"))
				{
					var retData = await _actionService.UpdateFundamentalsFromXueQiu(tmp["symbol"]);
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
				var retData = await _actionService.UpdateFundamentalsFromXueQiu(symbol);
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
        public async Task<IActionResult> UpdateAllFromIB(string symbol, bool forceUpdate, string sign)
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
                design = RsaService.DecryptToString(datas, 0);
            }
            if (!string.IsNullOrEmpty(design))
            {
				var tmp = Common.QueryStringToData(design);
				if (tmp != null && tmp.ContainsKey("symbol") && tmp.ContainsKey("forceUpdate"))
				{
					var retData = await _actionService.UpdateAllFromIB(tmp["symbol"], bool.Parse(tmp["forceUpdate"]));
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
				var retData = await _actionService.UpdateAllFromIB(symbol, forceUpdate);
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
        public async Task<IActionResult> ReadFromXmlAsync(string symbol, CommonLibs.FundamentalsReportEnum ftype, string sign)
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
                design = RsaService.DecryptToString(datas, 0);
            }
            if (!string.IsNullOrEmpty(design))
            {
				var tmp = Common.QueryStringToData(design);
				if (tmp != null && tmp.ContainsKey("symbol") && tmp.ContainsKey("ftype"))
				{
					var retData = await _actionService.ReadFromXmlAsync(tmp["symbol"], (CommonLibs.FundamentalsReportEnum)(int.Parse(tmp["ftype"])));
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
				var retData = await _actionService.ReadFromXmlAsync(symbol, ftype);
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
        public async Task<IActionResult> RequestFromIBAsync(string symbol, string exchange, CommonLibs.FundamentalsReportEnum ftype, bool forceUpdate, string sign)
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
                design = RsaService.DecryptToString(datas, 0);
            }
            if (!string.IsNullOrEmpty(design))
            {
				var tmp = Common.QueryStringToData(design);
				if (tmp != null && tmp.ContainsKey("symbol") && tmp.ContainsKey("exchange") && tmp.ContainsKey("ftype") && tmp.ContainsKey("forceUpdate"))
				{
					var retData = await _actionService.RequestFromIBAsync(tmp["symbol"], tmp["exchange"], (CommonLibs.FundamentalsReportEnum)(int.Parse(tmp["ftype"])), bool.Parse(tmp["forceUpdate"]));
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
				var retData = await _actionService.RequestFromIBAsync(symbol, exchange, ftype, forceUpdate);
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
        public async Task<IActionResult> ReadParseFundamentalToDbBackend(string symbol, CommonLibs.FundamentalsReportEnum ftype, string sign)
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
                design = RsaService.DecryptToString(datas, 0);
            }
            if (!string.IsNullOrEmpty(design))
            {
				var tmp = Common.QueryStringToData(design);
				if (tmp != null && tmp.ContainsKey("symbol") && tmp.ContainsKey("ftype"))
				{
					var retData = await _actionService.ReadParseFundamentalToDbBackend(tmp["symbol"], (CommonLibs.FundamentalsReportEnum)(int.Parse(tmp["ftype"])));
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
				var retData = await _actionService.ReadParseFundamentalToDbBackend(symbol, ftype);
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
