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
        public async Task<IActionResult> UpdateAllStocks(string sign)
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
				if (tmp != null)
				{
					var retData = await _actionService.UpdateAllStocks();
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
				var retData = await _actionService.UpdateAllStocks();
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
        public async Task<IActionResult> UpdateFundamentalsFromIB(string symbol, bool readXml, string sign)
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
				if (tmp != null && tmp.ContainsKey("symbol") && tmp.ContainsKey("readXml"))
				{
					var retData = await _actionService.UpdateFundamentalsFromIB(tmp["symbol"], bool.Parse(tmp["readXml"]));
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
				var retData = await _actionService.UpdateFundamentalsFromIB(symbol, readXml);
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
        public async Task<IActionResult> ReadAndParseFundamentalsAsync(string symbol, MoneyModels.FundamentalsReportEnum ftype, string sign)
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
					var retData = await _actionService.ReadAndParseFundamentalsAsync(tmp["symbol"], (MoneyModels.FundamentalsReportEnum)(int.Parse(tmp["ftype"])));
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
				var retData = await _actionService.ReadAndParseFundamentalsAsync(symbol, ftype);
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
        public async Task<IActionResult> RequestAndParseFundamentalsAsync(string symbol, MoneyModels.FundamentalsReportEnum ftype, string sign)
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
					var retData = await _actionService.RequestAndParseFundamentalsAsync(tmp["symbol"], (MoneyModels.FundamentalsReportEnum)(int.Parse(tmp["ftype"])));
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
				var retData = await _actionService.RequestAndParseFundamentalsAsync(symbol, ftype);
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
