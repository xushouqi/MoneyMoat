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
					if (retData != null)
					{
						return new OkObjectResult(retData);
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
				if (retData != null)
				{
					return new OkObjectResult(retData);
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
					if (retData != null)
					{
						return new OkObjectResult(retData);
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
				if (retData != null)
				{
					return new OkObjectResult(retData);
				}
				else
					return NoContent();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ReadFromXmlAsync(string symbol, MoneyModels.FundamentalsReportEnum ftype, string sign)
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
					var retData = await _actionService.ReadFromXmlAsync(tmp["symbol"], (MoneyModels.FundamentalsReportEnum)(int.Parse(tmp["ftype"])));
					if (retData != null)
					{
						return new OkObjectResult(retData);
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
				if (retData != null)
				{
					return new OkObjectResult(retData);
				}
				else
					return NoContent();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RequestFromIBAsync(string symbol, string exchange, MoneyModels.FundamentalsReportEnum ftype, bool forceUpdate, string sign)
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
					var retData = await _actionService.RequestFromIBAsync(tmp["symbol"], tmp["exchange"], (MoneyModels.FundamentalsReportEnum)(int.Parse(tmp["ftype"])), bool.Parse(tmp["forceUpdate"]));
					if (retData != null)
					{
						return new OkObjectResult(retData);
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
				if (retData != null)
				{
					return new OkObjectResult(retData);
				}
				else
					return NoContent();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ReadParseFundamentalToDbBackend(string symbol, MoneyModels.FundamentalsReportEnum ftype, string sign)
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
					var retData = await _actionService.ReadParseFundamentalToDbBackend(tmp["symbol"], (MoneyModels.FundamentalsReportEnum)(int.Parse(tmp["ftype"])));
					if (retData != null)
					{
						return new OkObjectResult(retData);
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
				if (retData != null)
				{
					return new OkObjectResult(retData);
				}
				else
					return NoContent();
            }
        }

		
    }
}
